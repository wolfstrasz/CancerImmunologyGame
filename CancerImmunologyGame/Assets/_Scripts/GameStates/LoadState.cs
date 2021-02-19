using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Tutorials;
using Bloodflow;

namespace Core
{
	namespace GameManagement
	{
		public class LoadState : GameState
		{
			public LoadState(GameStateController owner) : base(owner) { }

			internal override void OnFixedUpdate()
			{
			}

			internal override void OnStateEnter()
			{
			}

			internal override void OnStateExit()
			{
			}

			internal override void OnUpdate()
			{
				if (GameManager.Instance.sceneLoaded)
				{
					GameManager.Instance.sceneLoaded = false;
					InitialiseLevel();
					owner.SetState(new PlayState(owner));
				}
			}

			private void InitialiseLevel()
			{
				BackgroundMusic.Instance.Initialise();
				SmoothCamera.Instance.Reset();
				UIManager.Instance.ClosePanels();
				GlobalGameData.ResetObjectPool();

				PlayerController.Instance.Initialise();
				TutorialManager.Instance.Initialise();
				BloodflowController.Instance.Initialise();
				CellpediaUI.Cellpedia.Instance.Initialise();
			}
		}
	}
}