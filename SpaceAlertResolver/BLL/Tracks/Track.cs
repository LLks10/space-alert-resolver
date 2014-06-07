﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Threats;

namespace BLL.Tracks
{
	public abstract class Track<T> where T : Threat
	{
		public IDictionary<T, int> ThreatPositions { get; private set; }
		private readonly IDictionary<int, TrackBreakpoint> breakpoints;
		protected readonly IList<TrackSection> sections;

		protected Track(TrackConfiguration trackConfiguration)
		{
			ThreatPositions = new Dictionary<T, int>();
			breakpoints = trackConfiguration.TrackBreakpoints().ToDictionary(breakpoint => breakpoint.Position);
			sections = trackConfiguration.TrackSections();
		}

		public void AddThreat(T threat)
		{
			ThreatPositions[threat] = sections.Sum(section => section.Length);
		}

		public void RemoveThreats(IEnumerable<T> threats )
		{
			foreach (var threat in threats)
				ThreatPositions.Remove(threat);
		}

		public IList<T> ThreatsSurvived
		{
			get { return ThreatPositions.Keys.Where(threat => ThreatPositions[threat] <= 0).ToList(); }
		}

		public void MoveThreat(T threat)
		{
			//TODO: Check if this works correctly if speed is altered while looping (should only loop for original amount)
			threat.BeforeMove();
			for (var i = 0; i < threat.Speed; i++)
			{
				ThreatPositions[threat]--;
				var newLocationOnTrack = ThreatPositions[threat];
				if (breakpoints.ContainsKey(newLocationOnTrack))
				{
					var crossedBreakpoint = breakpoints[newLocationOnTrack];
					switch (crossedBreakpoint.Type)
					{
						case TrackBreakpointType.X:
							threat.PeformXAction();
							break;
						case TrackBreakpointType.Y:
							threat.PerformYAction();
							break;
						case TrackBreakpointType.Z:
							threat.PerformZAction();
							break;
					}
				}
			}
			threat.AfterMove();
		}

		public int DistanceToThreat(T threat)
		{
			var distance = ThreatPositions[threat];
			foreach (var section in sections.OrderBy(section => section.DistanceFromShip))
			{
				if (section.Length >= distance)
					return section.DistanceFromShip;
				distance -= section.Length;
			}
			throw new InvalidOperationException();
		}
	}
}
