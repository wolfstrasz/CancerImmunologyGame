using UnityEngine;


namespace ImmunotherapyGame.Loader
{
	public class IntroText : MonoBehaviour
	{
		public void TextFinishedFade()
		{
			Intro.Instance.ShowNextText();
		}
	}
}

