using BLL;
using BLL.Players;
using BLL.ShipComponents;
using BLL.Threats.External;
using BLL.Threats.Internal;
using BLL.Tracks;
using Microsoft.AspNetCore.Mvc;
using PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PL.Controllers
{
	public class SpaceAlertController : Controller
	{
		static IList<GameTurnModel> storedTurnModel;
		static TaskCompletionSource<bool> storedTurnModelSource;
		static object lck = new object();

		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}

		[HttpGet]
		[Route("NewGameInput")]
		public InputModel NewGameInput()
		{
			var allExternalThreats = ExternalThreatFactory.AllExternalThreats
				.Select(threat => new ExternalThreatModel(threat))
				.ToList();
			var allInternalThreats = InternalThreatFactory.AllInternalThreats
				.Select(threat => new InternalThreatModel(threat))
				.ToList();
			var inputModel = new InputModel
			{
				SingleActions = ActionModel.AllSingleActionModels.OrderBy(action => action.FirstAction).ThenBy(action => action.SecondAction),
				DoubleActions = ActionModel.AllSelectableDoubleActionModels.OrderBy(action => action.FirstAction).ThenBy(action => action.SecondAction),
				SpecializationActions = PlayerSpecializationActionModel.AllPlayerSpecializationActionModels
					.OrderBy(action => action.PlayerSpecialization)
					.ThenBy(player => player.Hotkey),
				Tracks = TrackFactory.CreateAllTracks()
					.Select(track => new TrackSnapshotModel(track, new List<int>()))
					.ToList(),
				AllInternalThreats = new AllThreatsModel(allInternalThreats),
				AllExternalThreats = new AllThreatsModel(allExternalThreats),
				PlayerSpecializations = EnumFactory.All<PlayerSpecialization>().ToList(),
				AllDamageTokens = EnumFactory.All<DamageToken>(),
				DamageableZones = EnumFactory.All<ZoneLocation>()
			};
			return inputModel;
		}

		[HttpPost]
		[Route("ProcessGame")]
		public IList<GameTurnModel> ProcessGame([FromBody] NewGameModel newGameModel) => GameToTurnModel(newGameModel);

		[HttpPost]
		[Route("LoadGame")]
		public void LoadGame([FromBody] NewGameModel newGameModel)
		{
			storedTurnModel = GameToTurnModel(newGameModel);
			lock (lck)
			{
				storedTurnModelSource?.SetResult(true);
			}
		}

		[HttpGet]
		[Route("Ping")]
		public async Task<IList<GameTurnModel>> Ping(CancellationToken ct)
		{
			IList<GameTurnModel> sm;
			lock (lck)
			{
				if (storedTurnModel != null)
				{
					sm = storedTurnModel;
					storedTurnModel = null;
					return sm;
				}

				storedTurnModelSource ??= new TaskCompletionSource<bool>();
			}

			var lct = CancellationTokenSource.CreateLinkedTokenSource(ct);
			lct.CancelAfter(TimeSpan.FromSeconds(60 *2));

			var timeout = Task.Delay(-1, lct.Token);
			var completed = await Task.WhenAny(storedTurnModelSource.Task, timeout);

			if (completed == timeout)
				return null;

			lock (lck)
			{
				storedTurnModelSource = null;

				if (storedTurnModel == null)
					return null;

				sm = storedTurnModel;
				storedTurnModel = null;
			}
			return sm;
		}

		[HttpPost]
		[Route("SendGameMessage")]
		public void SendGameMessage([FromBody] SendGameMessageModel model, string senderEmailAddress)
		{
			EmailService.SendEmail(model.MessageText, senderEmailAddress);
		}

		[HttpPost]
		[Route("BatchProcessGames")]
		public IActionResult BatchProcessGames([FromBody] BatchProcessModel batchProcessModel)
		{
			if (batchProcessModel.Games is null || batchProcessModel.Games.Count == 0)
				return BadRequest("No games given");

			BatchGameResultsModel result = new BatchGameResultsModel();

			foreach(var gameModel in batchProcessModel.Games)
			{
				var game = gameModel.ConvertToGame();
				game.StartGame();
				while (game.GameStatus == GameStatus.InProgress)
					game.PerformTurn();

				result.Results.Add(new GameResultModel
				{
					Won = game.GameStatus == GameStatus.Won,
					Score = new GameSnapshotModel(game, "").TotalScore,
				});
			}

			return Ok(result);
		}

		private IList<GameTurnModel> GameToTurnModel(NewGameModel newGameModel)
		{
			var game = newGameModel.ConvertToGame();

			game.StartGame();
			var turnModels = new List<GameTurnModel>();

			game.PhaseStarting += (sender, eventArgs) =>
			{
				var lastPhase = turnModels.Last().Phases.LastOrDefault();
				if(lastPhase != null && lastPhase.SubPhases.Count == 0)
					lastPhase.SubPhases.Add(new GameSnapshotModel(game, ""));
				
				turnModels.Last().Phases.Add(new GamePhaseModel { Description = eventArgs.PhaseHeader });
			};
			game.EventMaster.EventTriggered += (sender, eventArgs) =>
			{
				turnModels.Last().Phases.Last().SubPhases.Add(new GameSnapshotModel(game, eventArgs.PhaseHeader));
			};
			game.LostGame += (sender, args) =>
			{
				turnModels.Last().Phases.Last().SubPhases.Add(new GameSnapshotModel(game, "Lost!"));
			};

			while (game.GameStatus == GameStatus.InProgress)
			{
				turnModels.Add(new GameTurnModel { Turn = game.CurrentTurn });
				game.PerformTurn();
				if (turnModels.Last().Phases.Last().SubPhases.Count == 0)
					turnModels.Last().Phases.Last().SubPhases.Add(new GameSnapshotModel(game, ""));
			}
			return turnModels;
		}
	}
}
