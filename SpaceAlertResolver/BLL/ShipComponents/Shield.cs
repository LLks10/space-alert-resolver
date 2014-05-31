﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class Shield : EnergyContainer
	{
		private EnergyContainer Source { get; set; }

		protected Shield(Reactor source, int capacity, int energy) : base(capacity, energy)
		{
			Source = source;
		}

		public override void PerformBAction()
		{
			var energyToPull = capacity - Energy;
			Source.Energy -= energyToPull;
			Energy += energyToPull;
		}
	}
}