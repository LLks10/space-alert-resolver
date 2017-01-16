﻿using BLL.ShipComponents;

namespace BLL.Threats.Internal.Serious.White
{
	public class CrossedWires : SeriousWhiteInternalThreat
	{
		public CrossedWires()
			: base(4, 3, StationLocation.UpperWhite, PlayerActionType.Bravo)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			SittingDuck.TransferEnergyToShields(new [] {CurrentZone});
			EnergyLeaksOut(CurrentZone);
		}

		protected override void PerformYAction(int currentTurn)
		{
			EnergyLeaksOut(CurrentZone);
		}

		protected override void PerformZAction(int currentTurn)
		{
			foreach (var zoneLocation in EnumFactory.All<ZoneLocation>())
				EnergyLeaksOut(zoneLocation);
		}

		public override string Id { get; } = "SI1-05";
		public override string DisplayName { get; } = "Crossed Wires";
		public override string FileName { get; } = "CrossedWires";

		private void EnergyLeaksOut(ZoneLocation zoneLocation)
		{
			var energyDrained = SittingDuck.DrainShield(zoneLocation);
			Damage(energyDrained);
		}
	}
}
