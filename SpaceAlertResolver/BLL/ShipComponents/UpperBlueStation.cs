﻿namespace BLL.ShipComponents
{
	public class UpperBlueStation : UpperStation
	{
		public override IAlphaComponent AlphaComponent { get; }
		protected override Shield Shield { get; }
		protected override ICharlieComponent CharlieComponent => BattleBotsComponent;
		public BattleBotsComponent BattleBotsComponent { get; }

		public UpperBlueStation(
			SideReactor redReactor,
			ThreatController threatController,
			Gravolift gravolift,
			Airlock bluewardAirlock,
			Airlock redwardAirlock,
			SittingDuck sittingDuck) : base(StationLocation.LowerWhite, threatController, gravolift, bluewardAirlock, redwardAirlock, sittingDuck)
		{
			AlphaComponent = new SideHeavyLaserCannon(redReactor, ZoneLocation.Blue);
			Shield = new SideShield(redReactor);
			BattleBotsComponent = new BattleBotsComponent();
		}
	}
}