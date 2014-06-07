﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Minor.White
{
	public class Destroyer : MinorWhiteExternalThreat
	{
		public Destroyer(int timeAppears, ZoneLocation currentZone, ISittingDuck sittingDuck)
			: base(2, 5, 2, timeAppears, currentZone, sittingDuck)
		{
		}

		public override void PeformXAction()
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