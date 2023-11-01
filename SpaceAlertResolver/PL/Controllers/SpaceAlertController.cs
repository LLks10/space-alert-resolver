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
		static IList<GameTurnModel> storedModel;
		static TaskCompletionSource<bool> storedModelSource;
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
			storedModel = GameToTurnModel(newGameModel);
			lock (lck)
			{
				storedModelSource?.SetResult(true);
			}
		}

		[HttpGet]
		[Route("Ping")]
		public async Task<IList<GameTurnModel>> Ping(CancellationToken ct)
		{
			IList<GameTurnModel> sm;
			lock (lck)
			{
				if (storedModel != null)
				{
					sm = storedModel;
					storedModel = null;
					return sm;
				}

				storedModelSource ??= new TaskCompletionSource<bool>();
			}

			var lct = CancellationTokenSource.CreateLinkedTokenSource(ct);
			lct.CancelAfter(TimeSpan.FromSeconds(60 *2));

			var timeout = Task.Delay(-1, lct.Token);
			var completed = await Task.WhenAny(storedModelSource.Task, timeout);

			if (completed == timeout)
				return null;

			lock (lck)
			{
				storedModelSource = null;

				if (storedModel == null)
					return null;

				sm = storedModel;
				storedModel = null;
			}
			return sm;
		}

		[HttpPost]
		[Route("SendGameMessage")]
		public void SendGameMessage([FromBody] SendGameMessageModel model, string senderEmailAddress)
		{
			EmailService.SendEmail(model.MessageText, senderEmailAddress);
		}


		private IList<GameTurnModel> GameToTurnModel(NewGameModel newGameModel)
		{
			var game = newGameModel.ConvertToGame();

			game.StartGame();
			var turnModels = new List<GameTurnModel>();

			game.PhaseStarting += (sender, eventArgs) =>
			{
				var lastPhase = turnModels.Last().Phases.LastOrDefault();
				lastPhase?.SubPhases.Add(new GameSnapshotModel(game, "End of Phase"));
				turnModels.Last().Phases.Add(new GamePhaseModel { Description = eventArgs.PhaseHeader });
				turnModels.Last().Phases.Last().SubPhases.Add(new GameSnapshotModel(game, "Start of Phase"));
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
				turnModels.Last().Phases.Last().SubPhases.Add(new GameSnapshotModel(game, "End of Phase"));
			}
			return turnModels;
		}
	}
}
