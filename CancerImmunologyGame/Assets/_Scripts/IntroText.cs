using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Preload
{
	public class IntroText : MonoBehaviour
	{
		public void TextFinishedFade()
		{
			Intro.Instance.ShowNextText();
		}
	}
}

