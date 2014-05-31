﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class SideHeavyLaserCannon : HeavyLaserCannon
	{
		public SideHeavyLaserCannon(Reactor source, ZoneLocation currentZone) : base(source, 4, currentZone)
		{
		}
	}
}