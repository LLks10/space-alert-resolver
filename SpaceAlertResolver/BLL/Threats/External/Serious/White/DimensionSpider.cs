﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.White
{
	public class DimensionSpider : SeriousWhiteExternalThreat
	{
		public DimensionSpider()
			: base(0, 13, 1)
		{
		}

		public override void PerformXAction()
		{
			shields = 1;
		}

		public override void PerformYAction()
		{
			shields++;
		}

		public override void PerformZAction()
		{
			AttackAllZones(4);
		}

		public override void OnJumpingToHyperspace()
		{
			if (HasBeenPlaced)
				PerformZAction();
		}

		public static string GetDisplayName()
		{
			return "Dimension Spider";
		}

		public override bool CanBeTargetedBy(PlayerDamage damage)
		{
			return damage.PlayerDamageType != PlayerDamageType.Rocket && base.CanBeTargetedBy(damage);
		}
	}
}
