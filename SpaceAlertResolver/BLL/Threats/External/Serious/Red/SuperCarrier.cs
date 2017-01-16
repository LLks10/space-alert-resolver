﻿using System.Linq;

namespace BLL.Threats.External.Serious.Red
{
	public class SuperCarrier : SeriousRedExternalThreat
	{
		public SuperCarrier()
			: base(5, 13, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackCurrentZone(2, ThreatDamageType.ReducedByTwoAgainstInterceptors);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackOtherTwoZones(4, ThreatDamageType.ReducedByTwoAgainstInterceptors);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackAllZones(5, ThreatDamageType.ReducedByTwoAgainstInterceptors);
		}

		public override string Id { get; } = "SE3-103";
		public override string DisplayName { get; } = "Super-Carrier";
		public override string FileName { get; } = "SuperCarrier";

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return damage.AffectedDistances.Contains(DistanceToShip);
		}
	}
}
