using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Tutorials;
using Bloodflow;
using CellpediaUI;

namespace Core
{
	namespace GameManagement
	{
		public class PlayState : GameState
		{
			public PlayState(GameStateController owner) : base(owner) { }

			internal override void OnStateEnter()
			{
				GlobalGameData.ResetObjectPool(); // Maybe move to OnState enter
				BackgroundMusic.Instance.Initialise();
				UIManager.Instance.ClosePanels();
				PlayerController.Instance.Initialise();
				BloodflowController.Instance.Initialise();
				Cellpedia.Instance.Initialise();
				TutorialManager.Instance.Initialise();
			}

			internal override void OnStateExit()
			{
			}

			internal override void OnFixedUpdate()
			{
				PlayerController.Instance.OnFixedUpdate();
				BloodflowController.Instance.OnFixedUpdate();
				foreach (KillerCell kc in GlobalGameData.KillerCells)
				{
					kc.OnFixedUpdate();
				}
			}

			internal override void OnUpdate()
			{
				TutorialManager.Instance.OnUpdate();
				PlayerController.Instance.OnUpdate();
				for (int i = 0; i < GlobalGameData.KillerCells.Count; ++i)
				{
					GlobalGameData.KillerCells[i].OnUpdate();
				}

				for (int i = 0; i < GlobalGameData.Cancers.Count; ++i)
				{
					GlobalGameData.Cancers[i].OnUpdate();
				}

				if (GlobalGameData.Cancers.Count == 0)
				{
					UIManager.Instance.WinScreen();
					owner.SetState(new MainMenuState(owner));
					return;
				}
			}
		}
	}
}