using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using UnityEngine.SceneManagement;

namespace Loader {

	public class Loader : Singleton<Loader>
	{
		void Awake()
		{
			StartCoroutine(LoadGameAndInitialise());
		}

		IEnumerator LoadGameAndInitialise()
		{
			GameManager.Instance.Initialise();
			// Load datas


			// call intro finish
			Intro.Instance.FadeLogos();
			yield return null;
		}

		internal void OnIntroFinished()
		{
			SceneManager.LoadScene("MainMenu");
		}
	}

}
