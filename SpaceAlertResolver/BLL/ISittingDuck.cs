﻿using System;
using System.Collections.Generic;
using BLL.ShipComponents;
using BLL.Threats.Internal;

namespace BLL
{
	public interface ISittingDuck
	{
		ThreatDamageResult TakeAttack(ThreatDamage damage);

		void DrainShields(IEnumerable<ZoneLocation> zoneLocations);
		void DrainShields(IEnumerable<ZoneLocation> zoneLocations, int amount);
		void DrainReactors(IEnumerable<ZoneLocation> zoneLocations, int amount);
		int DrainShield(ZoneLocation zoneLocation);
		int DrainReactor(ZoneLocation zoneLocation);
		int DrainReactor(ZoneLocation zoneLocation, int amount);
		void DrainAllReactors(int amount);
		void DrainEnergy(StationLocation stationLocation, int amount);

		void TransferEnergyToShields(IEnumerable<ZoneLocation> zoneLocations);

		void AddZoneDebuff(IEnumerable<ZoneLocation> zoneLocations, ZoneDebuff debuff, InternalThreat source);
		void RemoveZoneDebuffForSource(IEnumerable<ZoneLocation> zoneLocations, InternalThreat source);

		int GetPlayerCount(StationLocation station);
		IEnumerable<Player> GetPlayersInStation(StationLocation station);

		void KnockOutPlayersWithBattleBots(IEnumerable<StationLocation> locations);
		void KnockOutPlayersWithoutBattleBots(IEnumerable<StationLocation> locations);
		void KnockOutPlayers(IEnumerable<StationLocation> locations);
		void KnockOutPlayers(IEnumerable<ZoneLocation> locations);

		void DisableLowerRedInactiveBattleBots();

		event EventHandler<RocketsRemovedEventArgs> RocketsModified;
		event EventHandler CentralLaserCannonFired;
		int RocketCount { get; }
		void RemoveRocket();
		int RemoveAllRockets();
		void ShiftPlayers(IEnumerable<ZoneLocation> zoneLocations, int turnToShift);
		void ShiftPlayers(IEnumerable<StationLocation> stationLocations, int turnToShift);
		void ShiftPlayersAndRepeatPreviousAction(IEnumerable<StationLocation> stationLocations, int turnToShift);

		void SubscribeToMovingIn(IEnumerable<StationLocation> stationLocations, EventHandler<PlayerMoveEventArgs> handler);
		void SubscribeToMovingOut(IEnumerable<StationLocation> stationLocations, EventHandler<PlayerMoveEventArgs> handler);
		void UnsubscribeFromMovingIn(IEnumerable<StationLocation> stationLocations, EventHandler<PlayerMoveEventArgs> handler);
		void UnsubscribeFromMovingOut(IEnumerable<StationLocation> stationLocations, EventHandler<PlayerMoveEventArgs> handler);

		void AddIrreparableMalfunctionToStations(IEnumerable<StationLocation> stationLocations, IrreparableMalfunction malfunction);
		bool DestroyFuelCapsule();
		int GetEnergyInReactor(ZoneLocation zoneLocation);
		void KnockOutCaptain();
		void SealRedDoors();
		void SealBlueDoors();
		void RepairAllSealedDoors();
		bool RedDoorIsSealed { get; }
		bool BlueDoorIsSealed { get; }
		int GetDamageToZone(ZoneLocation zoneLocation);
		void TeleportPlayers(IEnumerable<Player> playersToTeleport, StationLocation newStationLocation);
	}
}
