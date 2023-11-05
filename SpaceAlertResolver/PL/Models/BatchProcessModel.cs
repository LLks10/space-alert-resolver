using System.Collections.Generic;

namespace PL.Models
{
	public class BatchProcessModel
	{
		public List<NewGameModel> Games { get; set; }
	}

	public class GameResultModel
	{
		public int Score { get; set; }
		public bool Won { get; set; }
	}

	public class BatchGameResultsModel
	{
		public List<GameResultModel> Results { get; set; } = new List<GameResultModel>();
	}
}
