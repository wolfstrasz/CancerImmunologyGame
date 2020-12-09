using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Preload
{
	public class IntroFadeCallback : MonoBehaviour
	{
		[SerializeField]
		private Animator animator;
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

