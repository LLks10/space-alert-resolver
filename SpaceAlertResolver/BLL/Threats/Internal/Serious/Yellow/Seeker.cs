﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Serious.Yellow
{
	public class Seeker : SeriousYellowInternalThreat
	{
		public Seeker()
			: base(2, 2, StationLocation.UpperWhite, PlayerAction.BattleBots)
		{
		}

		protected override int GetPointsForDefeating()
		{
			return ThreatPoints.GetPointsForDefeatingSeeker();
		}

		protected override void PerformXAction(int currentTurn)
		{
			MoveToMostPlayers();
		}

		protected override void PerformYAction(int currentTurn)
		{
			MoveToMostPlayers();
		}

		protected override void PerformZAction(int currentTurn)
		{
			Damage(5);
			SittingDuck.KnockOutPlayers(CurrentStations);
		}

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation stationLocation)
		{
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
			if (IsDefeated)
				performingPlayer.IsKnockedOut = true;
		}

		public static string GetDisplayName()
		{
			return "Seeker";
		}

		private void MoveToMostPlayers()
		{
			//TODO: Move to most players
		}
	}
}
