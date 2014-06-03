﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.ShipComponents;
using BLL.Threats.External;
using BLL.Threats.Internal;

namespace BLL
{
	public class SittingDuck
	{
		public Zone BlueZone { get; private set; }
		public Zone WhiteZone { get; private set; }
		public Zone RedZone { get; private set; }
		public IDictionary<ZoneLocation, Zone> ZonesByLocation { get; private set; }
		public IEnumerable<Zone> Zones { get { return ZonesByLocation.Values; } }
		public InterceptorStation InterceptorStation { get; set; }
		public ComputerComponent Computer { get; private set; }
		public RocketsComponent RocketsComponent { get; private set; }
		public VisualConfirmationComponent VisualConfirmationComponent { get; private set; }
		public IList<ExternalThreat> CurrentExternalThreats { get; private set; }
		public IList<InternalThreat> CurrentInternalThreats { get; private set; }
		public IDictionary<ExternalThreat, ExternalThreatBuff> CurrentThreatBuffs { get; private set; }

		//TODO: Wire up all 3 stations if variable range interceptors are allowed
		public SittingDuck()
		{
			CurrentThreatBuffs = new Dictionary<ExternalThreat, ExternalThreatBuff>();
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
			var interceptorStation = new InterceptorStation();
			var upperRedStation = new StandardStation
			{
				Cannon = new SideHeavyLaserCannon(redReactor, ZoneLocation.Red),
				EnergyContainer = new SideShield(redReactor),
				ZoneLocation = ZoneLocation.Red
			};
			interceptorStation.InterceptorComponent = new InterceptorComponent(null, upperRedStation);
			upperRedStation.CComponent = new InterceptorComponent(interceptorStation, null);
			var upperWhiteStation = new StandardStation
			{
				Cannon = new CentralHeavyLaserCannon(whiteReactor, ZoneLocation.White),
				EnergyContainer = new CentralShield(whiteReactor),
				ZoneLocation = ZoneLocation.White,
				CComponent = computerComponent
			};
			var upperBlueStation = new StandardStation
			{
				Cannon = new SideHeavyLaserCannon(blueReactor, ZoneLocation.Blue),
				EnergyContainer = new SideShield(blueReactor),
				ZoneLocation = ZoneLocation.Blue,
				CComponent = new BattleBotsComponent()
			};
			var lowerRedStation = new StandardStation
			{
				Cannon = new SideLightLaserCannon(redBatteryPack, ZoneLocation.Red),
				EnergyContainer = redReactor,
				ZoneLocation = ZoneLocation.Red,
				CComponent = new BattleBotsComponent()
			};
			
			var lowerWhiteStation = new StandardStation
			{
				Cannon = new PulseCannon(whiteReactor),
				EnergyContainer = whiteReactor,
				ZoneLocation = ZoneLocation.White,
				CComponent = visualConfirmationComponent
			};
			var lowerBlueStation = new StandardStation
			{
				Cannon = new SideLightLaserCannon(blueBatteryPack, ZoneLocation.Blue),
				EnergyContainer = blueReactor,
				ZoneLocation = ZoneLocation.Blue,
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
			InterceptorStation = interceptorStation;
		}

		public void SetPlayers(IEnumerable<Player> players)
		{
			foreach (var player in players)
			{
				player.CurrentStation = WhiteZone.UpperStation;
				WhiteZone.UpperStation.Players.Add(player);
			}
		}

		public void DrainAllShields()
		{
			BlueZone.DrainShields();
			RedZone.DrainShields();
			WhiteZone.DrainShields();
		}
	}
}
