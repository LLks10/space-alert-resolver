﻿using System.Collections.Generic;
using System.Linq;
using BLL.Threats;
using BLL.Tracks;

namespace PL.Models
{
	public class TrackSnapshotModel
	{
		public IEnumerable<TrackSectionModel> Sections { get; set; }
	
		public TrackSnapshotModel(Track track, IEnumerable<Threat> threatsOnTrack)
		{
			Sections = track.Sections.Select(section => new TrackSectionModel(section, threatsOnTrack)).ToList();
		}
	}
}
