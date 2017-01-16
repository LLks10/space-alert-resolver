﻿namespace BLL.Threats.External.Minor.White
{
	public class Destroyer : MinorWhiteExternalThreat
	{
		public Destroyer()
			: base(2, 5, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackCurrentZone(1, ThreatDamageType.DoubleDamageThroughShields);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackCurrentZone(2, ThreatDamageType.DoubleDamageThroughShields);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(2, ThreatDamageType.DoubleDamageThroughShields);
		}

		public override string Id { get; } = "E1-02";
		public override string DisplayName { get; } = "Destroyer";
		public override string FileName { get; } = "Destroyer";
	}
}
