﻿using System.Collections.Generic;
using System.Linq;

namespace BLL.Players
{
	public class PlayerAction
	{
		public PlayerActionSegment FirstActionSegment { get; private set; }
		public PlayerActionSegment SecondActionSegment { get; private set; }
		public PlayerActionSegment BonusActionSegment { get; private set; }
		private IEnumerable<PlayerActionSegment> AllSegments => new[] { FirstActionSegment, SecondActionSegment, BonusActionSegment };

		private PlayerActionSegment NextUnperformedSegment => AllSegments.FirstOrDefault(segment => segment.SegmentStatus != PlayerActionStatus.Performed);

		public PlayerActionType? NextActionToPerform => NextUnperformedSegment.SegmentType;

		public void MarkNextActionPerforming()
		{
			var seg = NextUnperformedSegment;
			if (seg != null)
				seg.SegmentStatus = PlayerActionStatus.Performing;
		}

		public void MarkNextActionPerformed()
		{
			var seg = NextUnperformedSegment;
			if (seg != null)
				seg.SegmentStatus = PlayerActionStatus.Performed;
		}

		internal PlayerAction(PlayerActionType? firstActionType, PlayerActionType? secondActionType, PlayerActionType? bonusActionType)
		{
			FirstActionSegment = new PlayerActionSegment { SegmentType = firstActionType, SegmentStatus = PlayerActionStatus.NotPerformed };
			SecondActionSegment = new PlayerActionSegment { SegmentType = secondActionType, SegmentStatus = PlayerActionStatus.NotPerformed };
			BonusActionSegment = new PlayerActionSegment { SegmentType = bonusActionType, SegmentStatus = PlayerActionStatus.NotPerformed };
		}

		public bool CanBeMadeHeroic()
		{
			return FirstActionSegment.CanBeMadeHeroic() || SecondActionSegment.CanBeMadeHeroic();
		}

		public void MakeHeroic()
		{
			if (FirstActionSegment.CanBeMadeHeroic())
				FirstActionSegment.MakeHeroic();
			else if (SecondActionSegment.CanBeMadeHeroic())
				SecondActionSegment.MakeHeroic();
		}

		public void MarkAllActionsPerformed()
		{
			foreach (var segment in AllSegments)
			{
				segment.SegmentStatus = PlayerActionStatus.Performed;
			}
		}

		public bool AllActionsPerformed()
		{
			return AllSegments.All(segment => segment.SegmentStatus == PlayerActionStatus.Performed);
		}
	}
}
