﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CellpediaUI;
namespace Tutorials
{
	public class TWaitToOpenMicroscope : TutorialEvent
	{
		protected override void OnEndEvent()
		{
		}

		protected override void OnStartEvent()
		{
		}

		protected override bool OnUpdate()
		{
			if (Cellpedia.Instance.IsCellpediaOpened)
				return true;
			return false;
		}
	}
}