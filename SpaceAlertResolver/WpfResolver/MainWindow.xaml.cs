﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using BLL;
using BLL.Threats;
using BLL.Threats.External;
using BLL.Threats.Internal;
using BLL.Tracks;

namespace WpfResolver
{
	//TODO: Maintain list of threats/tracks/players in subsequent calls to the sets (reload on form)

	public partial class MainWindow
	{

		private readonly SittingDuck sittingDuck = new SittingDuck();

		public MainWindow()
		{
			InitializeComponent();
		}

		private void ResolveGame(object sender, RoutedEventArgs routedEventArgs)
		{
			var players = new List<Player>();
			var externalThreats = new List<ExternalThreat>();
			var externalTracks = new List<ExternalTrack>();
			var internalThreats = new List<InternalThreat>();
			var internalTrack = new InternalTrack(TrackConfiguration.Track1);

			sittingDuck.SetPlayers(players);
			var game = new Game(sittingDuck, externalThreats, externalTracks, internalThreats, internalTrack, players);

			var currentTurn = 0;
			try
			{
				for (currentTurn = 0; currentTurn < Game.NumberOfTurns; currentTurn++)
					game.PerformTurn();
			}
			catch (LoseException loseException)
			{
				StatusTextBlock.Text += string.Format("Killed on turn {0} by: {1}", currentTurn, loseException.Threat);
			}

			StatusTextBlock.Text += string.Format("Damage Taken:\r\nBlue: {0}\r\nRed: {1}\r\nWhite: {2}",
				sittingDuck.BlueZone.TotalDamage,
				sittingDuck.RedZone.TotalDamage,
				sittingDuck.WhiteZone.TotalDamage);
			StatusTextBlock.Text += string.Format("Threats killed: {0}. Threats survived: {1}",
				game.defeatedThreats.Count,
				game.survivedThreats.Count);
			StatusTextBlock.Text += string.Format("Total points: {0}", game.TotalPoints);
			foreach (var zone in sittingDuck.Zones)
			{
				foreach (var token in zone.AllDamageTokensTaken)
					StatusTextBlock.Text += string.Format("{0} damage token taken in zone {1}. Still damaged: {2}", token, zone.ZoneLocation, zone.CurrentDamageTokens.Contains(token));
			}
		}
	}
}
