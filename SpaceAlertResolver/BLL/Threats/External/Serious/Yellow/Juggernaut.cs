﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.Yellow
{
	public class Juggernaut : SeriousYellowExternalThreat
	{
		//TODO: Rockets always target it, even at distance 3
		public Juggernaut(int timeAppears, ZoneLocation currentZone, ISittingDuck sittingDuck)
			: base(3, 10, 1, timeAppears, currentZone, sittingDuck)
		{
		}

		public static string GetDisplayName()
		{
			return "Juggernaut";
		}

		public override void PeformXAction()
		{
			Speed += 2;
			Attack(2);
		}

		public override void PerformYAction()
		{
			Speed += 2;
			Attack(3);
		}

		public override void PerformZAction()
		{
			Attack(7);
		}

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			base.TakeDamage(damages);
			if (damages.Any(damage => damage.PlayerDamageType == PlayerDamageType.Rocket))
				shields++;
		}
	}
}