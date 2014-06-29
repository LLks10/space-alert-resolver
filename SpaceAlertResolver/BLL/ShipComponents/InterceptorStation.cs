﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public class InterceptorStation : Station
	{
		public InterceptorComponent InterceptorComponent { private get; set; }
		public PlayerInterceptorDamage PlayerInterceptorDamage { get; private set; }

		public InterceptorStation()
		{
			MoveIn += UseBattleBots;
		}

		private void PerformCAction(Player performingPlayer, int currentTurn)
		{
			InterceptorComponent.PerformCAction(performingPlayer, currentTurn);
		}

		private void UseBattleBots(Player performingPlayer, int currentTurn)
		{
			UseBattleBots(performingPlayer, false);
		}

		private void UseBattleBots(Player performingPlayer, bool isHeroic)
		{
			var firstThreat = GetFirstThreatOfType(PlayerActionType.BattleBots, performingPlayer);
			if (firstThreat == null)
				PlayerInterceptorDamage = new PlayerInterceptorDamage(isHeroic, performingPlayer, StationLocation.DistanceFromShip().GetValueOrDefault());
			else
				DamageThreat(1, firstThreat, performingPlayer, isHeroic);
		}

		public override void PerformMoveIn(Player performingPlayer, int currentTurn)
		{
			Players.Add(performingPlayer);
			performingPlayer.CurrentStation = this;
			OnMoveIn(performingPlayer, currentTurn);
		}

		public override void PerformPlayerAction(Player performingPlayer, int currentTurn)
		{
			switch (performingPlayer.Actions[currentTurn].ActionType)
			{
				case PlayerActionType.C:
					PerformCAction(performingPlayer, currentTurn);
					break;
				case PlayerActionType.BattleBots:
					UseBattleBots(performingPlayer, false);
					break;
				case PlayerActionType.HeroicBattleBots:
					UseBattleBots(performingPlayer, true);
					break;
				case PlayerActionType.AdvancedSpecialization:
					if (performingPlayer.AdvancedSpecialization == PlayerSpecialization.SquadLeader)
						UseBattleBots(performingPlayer, true);
					else
						PerformInvalidAction(performingPlayer, currentTurn);
					break;
				default:
					PerformInvalidAction(performingPlayer, currentTurn);
					break;
			}
		}

		private void PerformInvalidAction(Player player, int currentTurn)
		{
			InterceptorComponent.PerformNoAction(player, currentTurn);
			player.Shift(currentTurn);
		}

		public void PerformEndOfTurn()
		{
			PlayerInterceptorDamage = null;
		}
	}
}
