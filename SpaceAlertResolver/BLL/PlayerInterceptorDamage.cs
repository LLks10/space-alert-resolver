﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;

namespace BLL
{
	public class PlayerInterceptorDamage
	{
		private readonly bool isHeroic;
		private readonly Player performingPlayer;
		private readonly int distance;
		public PlayerInterceptorDamage(bool isHeroic, Player performingPlayer, int distance)
		{
			this.isHeroic = isHeroic;
			this.performingPlayer = performingPlayer;
			this.distance = distance;
		}

		public PlayerDamage SingleDamage
		{
			get
			{
				return new PlayerDamage(
					isHeroic ? 4 : 3,
					PlayerDamageType.InterceptorsSingle,
					new[] {distance},
					EnumFactory.All<ZoneLocation>(),
					performingPlayer);
			}
		}

		public PlayerDamage MultipleDamage
		{
			get
			{
				return new PlayerDamage(
					isHeroic ? 2 : 1,
					PlayerDamageType.InterceptorsMultiple,
					new[] {distance},
					EnumFactory.All<ZoneLocation>(),
					performingPlayer);
			}
		}
	}
}
