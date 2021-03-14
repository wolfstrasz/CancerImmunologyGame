using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorials
{
	public class TWaitHeartCinematic : TutorialEvent
	{
		protected override void OnEndEvent()
		{
		}

		protected override void OnStartEvent()
		{
		}

		protected override bool OnUpdateEvent()
		{
			return !SmoothCamera.Instance.InHeartOutro;
		}
	}
}