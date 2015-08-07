﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class BatteryPack : EnergyContainer
	{
		public BatteryPack() : base(1, 1)
		{
		}

		public override int Energy
		{
			get { return 1; }
			// ReSharper disable once ValueParameterNotUsed
			// Value is ignored to simulate infinite energy (since only doubles have an infinity constant and energy is integral).
		}
	}
}
