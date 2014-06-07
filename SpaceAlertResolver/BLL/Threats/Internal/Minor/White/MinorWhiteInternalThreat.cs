﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.Internal.Minor.White
{
	public abstract class MinorWhiteInternalThreat : MinorInternalThreat
	{
		protected MinorWhiteInternalThreat(int health, int speed, int timeAppears, StationLocation currentStation, PlayerAction actionType, ISittingDuck sittingDuck, int? accessibility = null) :
			base(ThreatDifficulty.White, health, speed, timeAppears, currentStation, actionType, sittingDuck, accessibility)
		{
		}

		protected MinorWhiteInternalThreat(int health, int speed, int timeAppears, List<StationLocation> currentStations, PlayerAction actionType, ISittingDuck sittingDuck, int? accessibility = null) :
			base(ThreatDifficulty.White, health, speed, timeAppears, currentStations, actionType, sittingDuck, accessibility)
		{
		}
	}
}