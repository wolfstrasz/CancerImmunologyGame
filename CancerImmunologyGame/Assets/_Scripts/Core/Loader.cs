using System.Collections;
using UnityEngine.SceneManagement;
using ImmunotherapyGame.GameManagement;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame
{
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
}