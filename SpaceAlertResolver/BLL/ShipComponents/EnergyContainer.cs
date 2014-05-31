﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public abstract class EnergyContainer
	{
		protected readonly int capacity;
		protected int energy;
		public virtual int Energy { get { return energy; } set { energy = value > 0 ? value : 0; } }

		protected EnergyContainer(int capacity, int energy)
		{
			this.capacity = capacity;
			this.energy = energy;
		}

		public abstract void PerformBAction();
	}
}