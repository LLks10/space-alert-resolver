﻿using System.Collections.Generic;
using System.Linq;
using BLL.ShipComponents;
using BLL.Threats.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PL.Models
{
	public class InternalThreatOnTrackModel : ThreatModel
	{
		public int TotalInaccessibility { get; set; }
		[JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
		public IEnumerable<StationLocation> CurrentStations { get; set; }

		public InternalThreatOnTrackModel(InternalThreat threat) : base(threat)
		{
			TotalInaccessibility = threat.TotalInaccessibility.GetValueOrDefault();
			CurrentStations = threat.CurrentStations.ToList();
		}

		[JsonConstructor]
		public InternalThreatOnTrackModel()
		{
		}
	}
}
