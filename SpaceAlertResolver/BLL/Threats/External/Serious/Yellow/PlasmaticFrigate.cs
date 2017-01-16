﻿namespace BLL.Threats.External.Serious.Yellow
{
	public class PlasmaticFrigate : SeriousYellowExternalThreat
	{
		public PlasmaticFrigate()
			: base(2, 7, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackCurrentZone(2, ThreatDamageType.Plasmatic);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackCurrentZone(2, ThreatDamageType.Plasmatic);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(4, ThreatDamageType.Plasmatic);
		}

		public override string Id { get; } = "SE2-101";
		public override string DisplayName { get; } = "Plasmatic Frigate";
		public override string FileName { get; } = "PlasmaticFrigate";
	}
}
