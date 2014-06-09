﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.White
{
	public class CryoshieldFighter : MinorWhiteExternalThreat
	{
		private bool cryoshieldUp = true;

		public CryoshieldFighter()
			: base(1, 4, 3)
		{
		}

		public override void PerformXAction()
		{
			Attack(1);
		}

		public override void PerformYAction()
		{
			Attack(2);
		}

		public override void PerformZAction()
		{
			Attack(2);
		}

		public static string GetDisplayName()
		{
			return "Cryoshield Fighter";
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
