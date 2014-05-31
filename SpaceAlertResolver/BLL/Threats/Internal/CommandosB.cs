﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal
{
	public class CommandosB : Commandos
	{
		public CommandosB(int timeAppears, SittingDuck sittingDuck)
			: base(timeAppears, sittingDuck.BlueZone.UpperStation, sittingDuck)
		{
		}

		public override void PerformYAction()
		{
			if (IsDamaged)
				MoveRed();
			else
				sittingDuck.TakeDamage(2, CurrentStation.ZoneLocation);
		}
	}
}