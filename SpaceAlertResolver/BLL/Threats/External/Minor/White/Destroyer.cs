﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.White
{
	public class Destroyer : MinorWhiteExternalThreat
	{
		public Destroyer()
			: base(2, 5, 2)
		{
		}

		public override void PerformXAction()
		{
			Attack(1, ThreatDamageType.DoubleDamageThroughShields);
		}

		public override void PerformYAction()
		{
			Attack(2, ThreatDamageType.DoubleDamageThroughShields);
		}

		public override void PerformZAction()
		{
			Attack(2, ThreatDamageType.DoubleDamageThroughShields);
		}

		public static string GetDisplayName()
		{
			return "Destroyer";
		}
	}
}
