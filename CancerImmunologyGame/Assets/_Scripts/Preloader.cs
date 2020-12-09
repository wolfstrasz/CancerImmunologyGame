using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Preload {

	public class Preloader : Singleton<Preloader>
	{
		void Awake()
		{
			StartCoroutine(LoadGameAndInitialise());
		}

		IEnumerator LoadGameAndInitialise()
		{
			// Load datas


			// call intro finish
			Intro.Instance.FadeLogos();
			yield return null;
		}

		internal void OnIntroFinished()
		{

		}
	}

}
