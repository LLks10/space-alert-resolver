﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Threats.External;
using BLL.Threats.Internal;

namespace BLL
{
	public class SittingDuck : ISittingDuck
	{
		public Zone BlueZone { get; private set; }
		public Zone WhiteZone { get; private set; }
		public Zone RedZone { get; private set; }
		public IDictionary<ZoneLocation, Zone> ZonesByLocation { get; private set; }
		public IEnumerable<Zone> Zones { get { return ZonesByLocation.Values; } }
		public IDictionary<StationLocation, Station> StationByLocation { get; private set; }
		public InterceptorStation InterceptorStation { get; set; }
		public ComputerComponent Computer { get; private set; }
		public RocketsComponent RocketsComponent { get; private set; }
		public VisualConfirmationComponent VisualConfirmationComponent { get; private set; }
		public IList<ExternalThreat> CurrentExternalThreats { get; private set; }
		public IList<InternalThreat> CurrentInternalThreats { get; private set; }
		private IDictionary<StationLocation, BattleBotsComponent> BattleBotsComponentsByLocation { get; set; }

		public IDictionary<ExternalThreat, ExternalThreatBuff> CurrentThreatBuffsBySource { get; private set; }

		//TODO: Wire up all 3 stations if variable range interceptors are allowed
		public SittingDuck()
		{
			CurrentThreatBuffsBySource = new Dictionary<ExternalThreat, ExternalThreatBuff>();
			CurrentInternalThreats = new List<InternalThreat>();
			CurrentExternalThreats = new List<ExternalThreat>();
			var whiteReactor = new CentralReactor();
			var redReactor = new SideReactor(whiteReactor);
			var blueReactor = new SideReactor(whiteReactor);
			var redBatteryPack = new BatteryPack();
			var blueBatteryPack = new BatteryPack();
			var computerComponent = new ComputerComponent();
			var visualConfirmationComponent = new VisualConfirmationComponent();
			var rocketsComponent = new RocketsComponent();
			Computer = computerComponent;
			VisualConfirmationComponent = visualConfirmationComponent;
			RocketsComponent = rocketsComponent;
			var interceptorStation = new InterceptorStation
			{
				StationLocation = StationLocation.Interceptor
			};
			var upperRedStation = new StandardStation
			{
				Cannon = new SideHeavyLaserCannon(redReactor, ZoneLocation.Red),
				EnergyContainer = new SideShield(redReactor),
				StationLocation = StationLocation.UpperRed
			};
			interceptorStation.InterceptorComponent = new InterceptorComponent(null, upperRedStation);
			upperRedStation.CComponent = new InterceptorComponent(interceptorStation, null);
			var upperWhiteStation = new StandardStation
			{
				Cannon = new CentralHeavyLaserCannon(whiteReactor, ZoneLocation.White),
				EnergyContainer = new CentralShield(whiteReactor),
				StationLocation = StationLocation.UpperWhite,
				CComponent = computerComponent
			};
			var upperBlueBattleBots = new BattleBotsComponent();
			var upperBlueStation = new StandardStation
			{
				Cannon = new SideHeavyLaserCannon(blueReactor, ZoneLocation.Blue),
				EnergyContainer = new SideShield(blueReactor),
				StationLocation = StationLocation.UpperBlue,
				CComponent = upperBlueBattleBots
			};
			var lowerRedBattleBots = new BattleBotsComponent();
			var lowerRedStation = new StandardStation
			{
				Cannon = new SideLightLaserCannon(redBatteryPack, ZoneLocation.Red),
				EnergyContainer = redReactor,
				StationLocation = StationLocation.LowerRed,
				CComponent = lowerRedBattleBots
			};
			
			var lowerWhiteStation = new StandardStation
			{
				Cannon = new PulseCannon(whiteReactor),
				EnergyContainer = whiteReactor,
				StationLocation = StationLocation.LowerWhite,
				CComponent = visualConfirmationComponent
			};
			var lowerBlueStation = new StandardStation
			{
				Cannon = new SideLightLaserCannon(blueBatteryPack, ZoneLocation.Blue),
				EnergyContainer = blueReactor,
				StationLocation = StationLocation.LowerBlue,
				CComponent = rocketsComponent
			};
			upperRedStation.BluewardStation = upperWhiteStation;
			upperRedStation.OppositeDeckStation = lowerRedStation;
			upperWhiteStation.RedwardStation = upperRedStation;
			upperWhiteStation.BluewardStation = upperBlueStation;
			upperWhiteStation.OppositeDeckStation = lowerWhiteStation;
			upperBlueStation.RedwardStation = upperWhiteStation;
			upperBlueStation.OppositeDeckStation = lowerBlueStation;
			lowerRedStation.BluewardStation = lowerWhiteStation;
			lowerRedStation.OppositeDeckStation = upperRedStation;
			lowerWhiteStation.RedwardStation = lowerRedStation;
			lowerWhiteStation.BluewardStation = lowerBlueStation;
			lowerWhiteStation.OppositeDeckStation = upperWhiteStation;
			lowerBlueStation.RedwardStation = lowerWhiteStation;
			lowerBlueStation.OppositeDeckStation = upperBlueStation;

			RedZone = new Zone { LowerStation = lowerRedStation, UpperStation = upperRedStation, ZoneLocation = ZoneLocation.Red, Gravolift = new Gravolift() };
			WhiteZone = new Zone { LowerStation = lowerWhiteStation, UpperStation = upperWhiteStation, ZoneLocation = ZoneLocation.White, Gravolift = new Gravolift() };
			BlueZone = new Zone { LowerStation = lowerBlueStation, UpperStation = upperBlueStation, ZoneLocation = ZoneLocation.Blue, Gravolift = new Gravolift() };
			ZonesByLocation = new[] {RedZone, WhiteZone, BlueZone}.ToDictionary(zone => zone.ZoneLocation);
			StationByLocation = Zones
				.SelectMany(zone => new[] {zone.LowerStation, zone.UpperStation})
				.Concat(new Station[] {interceptorStation})
				.ToDictionary(station => station.StationLocation);
			InterceptorStation = interceptorStation;
			BattleBotsComponentsByLocation = new Dictionary<StationLocation, BattleBotsComponent>
			{
				{StationLocation.UpperBlue, upperBlueBattleBots},
				{StationLocation.LowerRed, lowerRedBattleBots}
			};
		}

		public void SetPlayers(IEnumerable<Player> players)
		{
			foreach (var player in players)
			{
				player.CurrentStation = WhiteZone.UpperStation;
				WhiteZone.UpperStation.Players.Add(player);
			}
		}

		public int DrainShields(IEnumerable<ZoneLocation> zoneLocations)
		{
			return zoneLocations.Select(zoneLocation => ZonesByLocation[zoneLocation]).Sum(zone => zone.DrainShield());
		}

		public int DrainShields(IEnumerable<ZoneLocation> zoneLocations, int amount)
		{
			return zoneLocations.Select(zoneLocation => ZonesByLocation[zoneLocation]).Sum(zone => zone.DrainShield(amount));
		}

		public int DrainAllShields(int amount)
		{
			return DrainShields(EnumFactory.All<ZoneLocation>(), amount);
		}

		public int DrainAllShields()
		{
			return DrainShields(EnumFactory.All<ZoneLocation>());
		}

		public int DrainReactors(IEnumerable<ZoneLocation> zoneLocations)
		{
			return zoneLocations.Select(zoneLocation => ZonesByLocation[zoneLocation]).Sum(zone => zone.DrainReactor());
		}

		public int DrainReactors(IEnumerable<ZoneLocation> zoneLocations, int amount)
		{
			return zoneLocations.Select(zoneLocation => ZonesByLocation[zoneLocation]).Sum(zone => zone.DrainReactor(amount));
		}

		public int DrainAllReactors(int amount)
		{
			return DrainReactors(EnumFactory.All<ZoneLocation>(), amount);
		}

		public int DrainAllReactors()
		{
			return DrainReactors(EnumFactory.All<ZoneLocation>());
		}

		public ThreatDamageResult TakeAttack(ThreatDamage damage)
		{
			var result = new ThreatDamageResult();
			foreach (var zone in damage.ZoneLocations.Select(zoneLocation => ZonesByLocation[zoneLocation]))
			{
				bool isDestroyed;
				switch (damage.ThreatDamageType)
				{
					case ThreatDamageType.Internal:
					case ThreatDamageType.IgnoresShields:
						isDestroyed = zone.TakeDamage(damage.Amount);
						result.ShipDestroyed = result.ShipDestroyed || isDestroyed;
						break;
					case ThreatDamageType.ReducedByTwoAgainstInterceptors:
						var amount = damage.Amount;
						if (InterceptorStation.Players.Any())
							amount -= 2;
						isDestroyed = zone.TakeAttack(amount, ThreatDamageType.Standard);
						result.ShipDestroyed = result.ShipDestroyed || isDestroyed;
						break;
					default:
						isDestroyed = zone.TakeAttack(damage.Amount, damage.ThreatDamageType);
						result.ShipDestroyed = result.ShipDestroyed || isDestroyed;
						break;
				}
			}
			return result;
		}

		public int GetPlayerCount(StationLocation station)
		{
			return StationByLocation[station].Players.Count;
		}

		public int GetPoisonedPlayerCount(IEnumerable<StationLocation> locations)
		{
			return locations
				.Select(location => StationByLocation[location])
				.SelectMany(station => station.Players)
				.Count(player => player.IsPoisoned);
		}

		public void KnockOutPlayersWithBattleBots()
		{
			var playersWithBattleBots = Zones.SelectMany(zone => zone.Players).Where(player => player.BattleBots != null);
			KnockOut(playersWithBattleBots);
		}

		public void KnockOutPlayersWithBattleBots(IEnumerable<StationLocation> locations)
		{
			var playersWithBattleBots = locations
				.Select(location => StationByLocation[location])
				.SelectMany(zone => zone.Players)
				.Where(player => player.BattleBots != null);
			KnockOut(playersWithBattleBots);
		}

		public void KnockOutPlayersWithoutBattleBots()
		{
			var playersWithBattleBots = Zones.SelectMany(zone => zone.Players).Where(player => player.BattleBots == null);
			KnockOut(playersWithBattleBots);
		}

		public void KnockOutPlayersWithoutBattleBots(IEnumerable<StationLocation> locations)
		{
			var playersWithBattleBots = locations
				.Select(location => StationByLocation[location])
				.SelectMany(zone => zone.Players)
				.Where(player => player.BattleBots == null);
			KnockOut(playersWithBattleBots);
		}

		public void KnockOutPoisonedPlayers(IEnumerable<StationLocation> locations)
		{
			var poisonedPlayers = locations
				.Select(location => StationByLocation[location])
				.SelectMany(zone => zone.Players)
				.Where(player => player.IsPoisoned);
			KnockOut(poisonedPlayers);
		}

		public void KnockOutPlayers(IEnumerable<StationLocation> locations)
		{
			var players = locations.SelectMany(location => StationByLocation[location].Players);
			KnockOut(players);
		}

		public void TransferEnergyToShields(IEnumerable<ZoneLocation> zoneLocations)
		{
			foreach (var zone in zoneLocations.Select(zoneLocation => ZonesByLocation[zoneLocation]))
				TransferEnergyToShield(zone.UpperStation.EnergyContainer, zone.LowerStation.EnergyContainer);
		}

		private static void TransferEnergyToShield(EnergyContainer shield, EnergyContainer reactor)
		{
			var roomForShields = shield.Capacity - shield.Energy;
			var energyTransferredToShields = Math.Min(roomForShields, reactor.Energy);
			shield.Energy += energyTransferredToShields;
			reactor.Energy -= energyTransferredToShields;
		}

		private static void KnockOut(IEnumerable<Player> players)
		{
			foreach (var player in players)
				player.IsKnockedOut = true;
		}

		public void AddDebuff(IEnumerable<ZoneLocation> zoneLocations, ZoneDebuff debuff, InternalThreat source)
		{
			foreach (var zone in zoneLocations.Select(zoneLocation => ZonesByLocation[zoneLocation]))
				zone.DebuffsBySource[source] = debuff;
		}

		public void RemoveDebuffForSource(IEnumerable<ZoneLocation> zoneLocations, InternalThreat source)
		{
			foreach (var zone in zoneLocations.Select(zoneLocation => ZonesByLocation[zoneLocation]))
				zone.DebuffsBySource.Remove(source);
		}

		public void DisableInactiveBattlebots(IEnumerable<StationLocation> stationLocations)
		{
			foreach (var stationLocation in stationLocations.Where(stationLocation => BattleBotsComponentsByLocation.ContainsKey(stationLocation)))
				BattleBotsComponentsByLocation[stationLocation].DisableInactiveBattleBots();
		}
	}
}
