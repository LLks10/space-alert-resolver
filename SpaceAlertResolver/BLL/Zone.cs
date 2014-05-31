﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL
{
	public class Zone
	{
		//TODO: Make UpperStation and LowerStation classes and have them have a reactor and shield instead of two EnergyContainers
		public Station UpperStation { get; set; }
		public Station LowerStation { get; set; }
		public int TotalDamage { get; set; }
		public ZoneLocation ZoneLocation { get; set; }
		public IList<Player> Players { get { return UpperStation.Players.Concat(LowerStation.Players).ToList(); } }

		public ExternalPlayerDamageResult TakeAttack(int damage)
		{
			var oldShields = UpperStation.EnergyContainer.Energy;
			UpperStation.EnergyContainer.Energy -= damage;
			var newShields = UpperStation.EnergyContainer.Energy;
			var damageShielded = oldShields - newShields;
			var damageDone = damage - damageShielded;
			//TODO: Apply damageDone tokens
			TotalDamage += damageDone;
			if (TotalDamage >= 7)
				//TODO: Lose
				throw new NotImplementedException();
			return new ExternalPlayerDamageResult
			{
				DamageDone = damageDone,
				DamageShielded = damageShielded
			};
		}

		public void TakeDamage(int damage)
		{
			//TODO: Apply damageDone tokens
			TotalDamage += damage;
			if (TotalDamage >= 7)
				//TODO: Lose
				throw new NotImplementedException();
		}

		public void DrainShields()
		{
			UpperStation.EnergyContainer.Energy = 0;
		}
	}
}
