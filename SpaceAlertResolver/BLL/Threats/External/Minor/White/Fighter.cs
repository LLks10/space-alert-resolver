﻿namespace BLL.Threats.External.Minor.White
{
	public class Fighter : MinorWhiteExternalThreat
	{
		public Fighter()
			: base(2, 4, 3)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackCurrentZone(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackCurrentZone(2);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(3);
		}

		public override string Id { get; } = "E1-07";
		public override string DisplayName { get; } = "Fighter";
		public override string FileName { get; } = "Fighter";
	}
}
