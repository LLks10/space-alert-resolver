﻿using System.Collections.Generic;
using System.Linq;
using BLL.Tracks;

namespace PL.Models
{
	public class TrackSnapshotModel
	{
		public IEnumerable<TrackSectionSnapshotModel> Sections { get; set; }
	
		public TrackSnapshotModel(Track track)
		{
			Sections = track.Sections.Select(section => new TrackSectionSnapshotModel(section)).ToList();
		}
	}
}