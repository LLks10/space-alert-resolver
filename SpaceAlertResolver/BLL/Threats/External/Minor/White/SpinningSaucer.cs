﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.White
{
	public class SpinningSaucer : MinorWhiteExternalThreat
	{
		private bool hitByRocket;

		public SpinningSaucer()
			: base(4, 3, 3)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			Attack(1);
		}

		protected override void PerformYAction(int currentTurn)
		{
			Attack(1);
		}

		protected override void PerformZAction(int currentTurn)
		{
			if (!hitByRocket)
				Attack(5);
		}

		public static string GetDisplayName()
		{
			return "Spinning Saucer";
		}

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			if (damages.Any(damage => damage.PlayerDamageType == PlayerDamageType.Rocket))
				hitByRocket = true;
			base.TakeDamage(damages);
		}
	}
}
