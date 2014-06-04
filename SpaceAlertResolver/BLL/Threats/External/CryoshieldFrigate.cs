﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External
{
	public class CryoshieldFrigate : SeriousWhiteExternalThreat
	{
		private bool cryoshieldUp = true;

		public CryoshieldFrigate(int timeAppears, ZoneLocation currentZone, ISittingDuck sittingDuck)
			: base(1, 7, 3, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
		{
			Attack(2);
		}

		public override void PerformYAction()
		{
			Attack(3);
		}

		public override void PerformZAction()
		{
			Attack(4);
		}

		public static string GetDisplayName()
		{
			return "Cryoshield Frigate";
		}

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			if (cryoshieldUp && damages.Any())
				cryoshieldUp = false;
			else
				base.TakeDamage(damages);
		}
	}
}
