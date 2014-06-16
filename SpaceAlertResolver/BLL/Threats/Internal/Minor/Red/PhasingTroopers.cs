﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Tracks;

namespace BLL.Threats.Internal.Minor.Red
{
	public class PhasingTroopers : MinorRedInternalThreat
	{
		private bool isPhased;
		private bool wasPhasedAtStartOfTurn;

		public PhasingTroopers()
			: base(2, 2, StationLocation.UpperWhite, PlayerAction.BattleBots)
		{
		}

		public static string GetDisplayName()
		{
			return "Phasing Troopers";
		}

		protected override void PlaceOnTrack(Track track, int trackPosition)
		{
			base.PlaceOnTrack(track, trackPosition);
			BeforeMove += PerformBeforeMove;
			AfterMove += PerformAfterMove;
		}

		protected override void PerformXAction(int currentTurn)
		{
			if (!wasPhasedAtStartOfTurn)
				ChangeDecks();
		}

		protected override void PerformYAction(int currentTurn)
		{
			MoveRed();
		}

		protected override void PerformZAction(int currentTurn)
		{
			Damage(wasPhasedAtStartOfTurn ? 3 : 4);
		}

		private void PerformBeforeMove()
		{
			isPhased = false;
		}

		private void PerformAfterMove()
		{
			isPhased = !wasPhasedAtStartOfTurn;
		}

		protected override void PerformEndOfTurn()
		{
			if (isPhased)
				wasPhasedAtStartOfTurn = isPhased;
			base.PerformEndOfTurn();
		}

		public override bool IsDamageable
		{
			get { return base.IsDamageable && !isPhased; }
		}

		public override void TakeDamage(int damage, Player performingPlayer, bool isHeroic, StationLocation stationLocation)
		{
			base.TakeDamage(damage, performingPlayer, isHeroic, stationLocation);
			if (!isHeroic)
				performingPlayer.BattleBots.IsDisabled = true;
		}

		protected override void OnThreatTerminated()
		{
			BeforeMove += PerformBeforeMove;
			AfterMove += PerformAfterMove;
			base.OnThreatTerminated();
		}
	}
}