﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.ShipComponents
{
	public interface IBravoComponent : IDamageableComponent
	{
		void PerformBAction(bool isHeroic);
	}
}
