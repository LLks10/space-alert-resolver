﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL.Tracks;

namespace BLL.Threats.External
{
	public abstract class ExternalThreat : Threat
	{
		public ZoneLocation CurrentZone { get; private set; }
		protected int shields;
		private ExternalTrack Track { get; set; }

		public override bool IsSurvived { get { return !IsDestroyed && (!Track.ThreatPositions.ContainsKey(this) || Track.ThreatPositions[this] <= 0); } }

		public void SetTrack(ExternalTrack track)
		{
			Track = track;
		}

		private int DistanceToShip { get { return Track.DistanceToThreat(this); } }
		public int TrackPosition  { get { return Track.ThreatPositions[this]; }}

		protected ExternalThreat(ThreatType type, ThreatDifficulty difficulty, int shields, int health, int speed, int timeAppears, ZoneLocation currentZone, ISittingDuck sittingDuck) :
			base(type, difficulty, health, speed, timeAppears, sittingDuck)
		{
			this.shields = shields;
			CurrentZone = currentZone;
		}

		public virtual void TakeDamage(IList<PlayerDamage> damages)
		{
			TakeDamage(damages, null);
		}

		protected void TakeDamage(IEnumerable<PlayerDamage> damages, int? maxDamageTaken)
		{
			var bonusShields = sittingDuck.CurrentThreatBuffsBySource.Values.Count(buff => buff == ExternalThreatBuff.BonusShield);
			var damageDealt = damages.Sum(damage => damage.Amount) - (shields + bonusShields);
			if (damageDealt > 0)
				RemainingHealth -= maxDamageTaken.HasValue ? Math.Min(damageDealt, maxDamageTaken.Value) : damageDealt;
			CheckForDestroyed();
		}

		public virtual bool CanBeTargetedBy(PlayerDamage damage)
		{
			var isInRange = damage.Range >= DistanceToShip;
			var gunCanHitCurrentZone = damage.ZoneLocations.Contains(CurrentZone);
			return isInRange && gunCanHitCurrentZone;
		}

		protected void AttackSpecificZones(int amount, IList<ZoneLocation> zones, ThreatDamageType threatDamageType = ThreatDamageType.Standard)
		{
			Attack(amount, threatDamageType, zones);
		}

		protected void Attack(int amount, ThreatDamageType threatDamageType = ThreatDamageType.Standard)
		{
			Attack(amount, threatDamageType, new [] {CurrentZone});
		}

		protected void AttackAllZones(int amount, ThreatDamageType threatDamageType = ThreatDamageType.Standard)
		{
			Attack(amount, threatDamageType, EnumFactory.All<ZoneLocation>());
		}

		protected void AttackOtherTwoZones(int amount, ThreatDamageType threatDamageType = ThreatDamageType.Standard)
		{
			Attack(amount, threatDamageType, EnumFactory.All<ZoneLocation>().Except(new[] { CurrentZone }).ToList());
		}

		private void Attack(int amount, ThreatDamageType threatDamageType, IList<ZoneLocation> zoneLocations)
		{
			var bonusAttacks = sittingDuck.CurrentThreatBuffsBySource.Values.Count(buff => buff == ExternalThreatBuff.BonusAttack);
			var damage = new ThreatDamage(amount + bonusAttacks, threatDamageType, zoneLocations);
			var result = sittingDuck.TakeAttack(damage);
			if (result.ShipDestroyed)
				throw new LoseException(this);
		}

		public virtual void PerformEndOfComputeDamage()
		{
		}
	}
}
