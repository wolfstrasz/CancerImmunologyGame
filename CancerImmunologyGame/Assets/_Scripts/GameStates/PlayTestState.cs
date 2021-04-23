using UnityEngine;
using ImmunotherapyGame.Player;
using ImmunotherapyGame.CellpediaSystem;
using ImmunotherapyGame.Tutorials;
using ImmunotherapyGame.AI;
using ImmunotherapyGame.Audio;

namespace ImmunotherapyGame
{
	namespace GameManagement
	{
		public class PlayTestState : GameState
		{
			public PlayTestState(GameStateController owner) : base(owner) { }

			internal override void OnFixedUpdate()
			{
				if (GameObject.FindObjectOfType<PlayerController>() != null)
					PlayerController.Instance.OnFixedUpdate();

				foreach (KillerCell kc in GlobalGameData.KillerCells)
				{
					kc.OnFixedUpdate();
				}

				foreach (RegulatoryCell rc in GlobalGameData.RegulatoryCells)
				{
					if (rc.gameObject.activeSelf)
						rc.OnFixedUpdate();
				}

				foreach (HelperTCell hc in GlobalGameData.HelperTCells)
				{
					if (hc.gameObject.activeSelf)
						hc.OnFixedUpdate();
				}

			}

			internal override void OnStateEnter()
			{
				Debug.Log("Play test state");
				GlobalGameData.ResetObjectPool();
				if (GameObject.FindObjectOfType<TutorialManager>() != null)
					BackgroundMusic.Instance.Initialise();
				if (GameObject.FindObjectOfType<UIManager>() != null)
					UIManager.Instance.ClosePanels();
				if (GameObject.FindObjectOfType<PlayerController>() != null)
					PlayerController.Instance.Initialise();
				if (GameObject.FindObjectOfType<Cellpedia>() != null)
					Cellpedia.Instance.Initialise();
				if (GameObject.FindObjectOfType<TutorialManager>() != null)
					TutorialManager.Instance.Initialise();
			}

			internal override void OnStateExit()
			{
				Debug.Log("Exit play test state");
			}

			internal override void OnUpdate()
			{

				if (GameObject.FindObjectOfType<TutorialManager>() != null)
					TutorialManager.Instance.OnUpdate();
				if (GameObject.FindObjectOfType<PlayerController>() != null)
					PlayerController.Instance.OnUpdate();
				for (int i = 0; i < GlobalGameData.KillerCells.Count; ++i)
				{
					GlobalGameData.KillerCells[i].OnUpdate();
				}


				foreach (RegulatoryCell rc in GlobalGameData.RegulatoryCells)
				{
					if (rc.gameObject.activeSelf)
						rc.OnUpdate();
				}

				foreach (HelperTCell hc in GlobalGameData.HelperTCells)
				{
					if (hc.gameObject.activeSelf)
						hc.OnUpdate();
				}
				foreach (AIController controller in GlobalGameData.AIKillerCells)
				{
					controller.OnUpdate();
				}
				for (int i = 0; i < GlobalGameData.Cancers.Count; ++i)
				{
					GlobalGameData.Cancers[i].OnUpdate();
				}
			}
		}
	}
}