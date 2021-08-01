using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.CellpediaSystem;

namespace ImmunotherapyGame.Tutorials
{
	public class TWaitToOpenMicroscope : TutorialEvent
	{
		protected override bool OnUpdateEvent()
		{
			return CellpediaSystem.Cellpedia.Instance.IsCellpediaOpened;
		}
	}
}
