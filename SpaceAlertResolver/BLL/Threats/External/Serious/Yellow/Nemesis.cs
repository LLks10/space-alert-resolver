﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Threats.External.Serious.Yellow
{
	public class Nemesis : SeriousYellowExternalThreat
	{
		private int healthAtStartOfTurn;

		public Nemesis()
			: base(1, 9, 3)
		{
			healthAtStartOfTurn = RemainingHealth;
		}

		public override void PerformXAction()
		{
			Attack(1);
			RemainingHealth--;
			CheckDefeated();
		}

		public override void PerformYAction()
		{
			Attack(2);
			RemainingHealth -= 2;
			CheckDefeated();
		}

		public override void PerformZAction()
		{
			throw new LoseException(this);
		}

		public override void PerformEndOfDamageResolution()
		{
			if (healthAtStartOfTurn > RemainingHealth)
				AttackAllZones(1);
			base.PerformEndOfDamageResolution();
		}

		public static string GetDisplayName()
		{
			return "Nemesis";
		}

		public override void PerformEndOfTurn()
		{
			base.PerformEndOfTurn();
			healthAtStartOfTurn = RemainingHealth;
		}
	}
}
