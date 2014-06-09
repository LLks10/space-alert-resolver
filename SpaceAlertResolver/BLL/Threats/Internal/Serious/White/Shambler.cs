﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Serious.White
{
	public class Shambler : SeriousWhiteInternalThreat
	{
		public Shambler()
			: base(2, 2, StationLocation.LowerWhite, PlayerAction.BattleBots)
		{
		}

		public override void PerformXAction()
		{
			if (IsAnyPlayerPresent())
				MoveBlue();
		}

		public override void PerformYAction()
		{
			if (IsAnyPlayerPresent())
				Damage(2);
			else
				Repair(1);
		}

		public override void PerformZAction()
		{
			Damage(4);
		}

		public static string GetDisplayName()
		{
			return "Shambler";
		}

		private bool IsAnyPlayerPresent()
		{
			return SittingDuck.GetPlayerCount(CurrentStation) != 0;
		}
	}
}
