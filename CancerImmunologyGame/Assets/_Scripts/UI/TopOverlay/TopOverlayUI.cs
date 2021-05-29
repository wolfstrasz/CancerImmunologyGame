using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.UI.TopOverlay
{
    public class TopOverlayUI : Singleton<TopOverlayUI>
    {
		[SerializeField]
		private GameObject gamePausedText = null;
		public bool GamePaused 
		{ 
			get { return gamePausedText.activeInHierarchy; } 
			set { gamePausedText.SetActive(value); } 
		}

        public void Initialise()
		{
			SceneManager.activeSceneChanged += OnActiveSceneChanged;
			gameObject.SetActive(SceneManager.GetActiveScene().buildIndex >= 2);
		}

		public void OnActiveSceneChanged(Scene currentScene, Scene nextScene)
		{
			gameObject.SetActive(nextScene.buildIndex >= 2);
		}
	}
}
