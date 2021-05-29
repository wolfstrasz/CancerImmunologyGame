using UnityEngine;


namespace ImmunotherapyGame.Loader
{
	public class IntroText : MonoBehaviour
	{
		[SerializeField]
		private Intro intro;

		public void TextFinishedFade()
		{
			intro.ShowNextText();
		}
	}
}

