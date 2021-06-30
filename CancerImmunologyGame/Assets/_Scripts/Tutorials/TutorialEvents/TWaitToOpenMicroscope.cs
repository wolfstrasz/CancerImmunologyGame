﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.CellpediaSystem;

namespace ImmunotherapyGame.Tutorials
{
	public class TWaitToOpenMicroscope : TutorialEvent
	{
		protected override void OnEndEvent()
		{
		}

		protected override void OnStartEvent()
		{
		}

		protected override bool OnUpdateEvent()
		{
			if (CellpediaSystem.Cellpedia.Instance.IsCellpediaOpened)
				return true;
			return false;
		}
	}
}