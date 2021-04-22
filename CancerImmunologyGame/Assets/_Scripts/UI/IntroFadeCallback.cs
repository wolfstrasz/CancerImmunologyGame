using UnityEngine;

namespace ImmunotherapyGame
{
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
}