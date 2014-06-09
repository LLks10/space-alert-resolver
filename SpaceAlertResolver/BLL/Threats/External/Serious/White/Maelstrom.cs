﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.White
{
	public class Maelstrom : SeriousWhiteExternalThreat
	{
		public Maelstrom()
			: base(3, 8, 2)
		{
		}

		public override void PerformXAction()
		{
			SittingDuck.DrainShields(EnumFactory.All<ZoneLocation>());
		}

		public override void PerformYAction()
		{
			AttackOtherTwoZones(2);
		}

		public override void PerformZAction()
		{
			AttackOtherTwoZones(3);
		}

		public static string GetDisplayName()
		{
			return "Maelstrom";
		}

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			var hitByPulse = damages.Any(damage => damage.PlayerDamageType == PlayerDamageType.Pulse);
			if (hitByPulse)
			{
				var oldShields = shields;
				shields = 0;
				base.TakeDamage(damages);
				shields = oldShields;
			}
			else
				base.TakeDamage(damages);
		}
	}
}
