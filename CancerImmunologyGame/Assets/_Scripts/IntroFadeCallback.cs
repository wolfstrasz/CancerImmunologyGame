using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loader
{
	public class IntroFadeCallback : MonoBehaviour
	{
		[SerializeField]
		private Animator animator = null;

		internal void StartFade()
		{
			animator.SetTrigger("Fade");
		}

		public void FadeFinished()
		{
			Intro.Instance.LogoFadeFinished();
		}
	}
}

