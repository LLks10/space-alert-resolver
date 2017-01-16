﻿using System.Collections.Generic;
using System.Linq;

namespace BLL.Threats.External.Serious.White
{
	public class CryoshieldFrigate : SeriousWhiteExternalThreat
	{
		private bool cryoshieldUp = true;

		public CryoshieldFrigate()
			: base(1, 7, 2)
		{
		}

		protected override void PerformXAction(int currentTurn)
		{
			AttackCurrentZone(2);
		}

		protected override void PerformYAction(int currentTurn)
		{
			AttackCurrentZone(3);
		}

		protected override void PerformZAction(int currentTurn)
		{
			AttackCurrentZone(4);
		}

		public override string Id { get; } = "SE1-05";
		public override string DisplayName { get; } = "Cryoshield Frigate";
		public override string FileName { get; } = "CryoshieldFrigate";

		public override void TakeDamage(IList<PlayerDamage> damages)
		{
			if (cryoshieldUp && damages.Any())
				cryoshieldUp = false;
			else
				base.TakeDamage(damages);
		}
	}
}
