﻿using BLL.Common;

namespace BLL.Threats.External.Minor.Yellow
{
	public class MinorAsteroid : MinorYellowExternalThreat
	{
		private int breakpointsCrossed;

		public MinorAsteroid()
			: base(0, 7, 4)
		{
		}
		protected override void PerformXAction(int currentTurn)
		{
			breakpointsCrossed++;
		}

		protected override void PerformYAction(int currentTurn)
		{
			breakpointsCrossed++;
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(RemainingHealth);
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			Check.ArgumentIsNotNull(damage, "damage");
			return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
		}

		protected override void OnHealthReducedToZero()
		{
			base.OnHealthReducedToZero();
			AttackCurrentZone(1 * breakpointsCrossed);
		}

		public override string Id { get; } = "E2-07";
		public override string DisplayName { get; } = "Minor Asteroid";
		public override string FileName { get; } = "MinorAsteroid";
	}
}
