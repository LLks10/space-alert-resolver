﻿using System.Collections.Generic;
using BLL.Common;

namespace BLL.Threats.External.Serious.Yellow
{
	public class MotherSwarm : SeriousYellowExternalThreat
	{
		public MotherSwarm()
			: base(1, 6, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackCurrentZone(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackCurrentZone(2);
			AttackOtherTwoZones(1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(4);
			AttackOtherTwoZones(3);
		}

		public override string Id { get; } = "SE2-103";
		public override string DisplayName { get; } = "Mother Swarm";
		public override string FileName { get; } = "MotherSwarm";

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			TakeDamage(damages, 2);
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			Check.ArgumentIsNotNull(damage, "damage");
			return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
		}
	}
}
