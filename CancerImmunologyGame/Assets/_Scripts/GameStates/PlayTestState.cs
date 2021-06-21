using UnityEngine;
using ImmunotherapyGame.Player;
using ImmunotherapyGame.CellpediaSystem;
using ImmunotherapyGame.Tutorials;
using ImmunotherapyGame.AI;
using ImmunotherapyGame.Audio;
using ImmunotherapyGame.UI.TopOverlay;

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

				foreach (KillerCell kc in GlobalLevelData.KillerCells)
				{
					kc.OnFixedUpdate();
				}

				foreach (RegulatoryCell rc in GlobalLevelData.RegulatoryCells)
				{
					if (rc.gameObject.activeSelf)
						rc.OnFixedUpdate();
				}

				foreach (HelperTCell hc in GlobalLevelData.HelperTCells)
				{
					if (hc.gameObject.activeSelf)
						hc.OnFixedUpdate();
				}

			}

			internal override void OnStateEnter()
			{
				TopOverlayUI.Instance.GamePaused = false;

				Debug.Log("Play test state");
				GlobalLevelData.UpdateLevelData();
				if (GameObject.FindObjectOfType<TutorialManager>() != null)
					BackgroundMusic.Instance.Initialise();
				if (GameObject.FindObjectOfType<PlayerController>() != null)
					PlayerController.Instance.Initialise();
				if (GameObject.FindObjectOfType<TutorialManager>() != null)
					TutorialManager.Instance.LoadLevelTutorials();
			}

			internal override void OnStateExit()
			{
				Debug.Log("Exit play test state");
			}

			internal override void OnStateReEnter()
			{
				TopOverlayUI.Instance.GamePaused = false;
			}

			internal override void OnUpdate()
			{

				if (GameObject.FindObjectOfType<TutorialManager>() != null)
					TutorialManager.Instance.OnUpdate();
				if (GameObject.FindObjectOfType<PlayerController>() != null)
					PlayerController.Instance.OnUpdate();
				for (int i = 0; i < GlobalLevelData.KillerCells.Count; ++i)
				{
					GlobalLevelData.KillerCells[i].OnUpdate();
				}


				foreach (RegulatoryCell rc in GlobalLevelData.RegulatoryCells)
				{
					if (rc.gameObject.activeSelf)
						rc.OnUpdate();
				}

				foreach (HelperTCell hc in GlobalLevelData.HelperTCells)
				{
					if (hc.gameObject.activeSelf)
						hc.OnUpdate();
				}
				foreach (AIController controller in GlobalLevelData.AIKillerCells)
				{
					controller.OnUpdate();
				}

				for (int i = 0; i < GlobalLevelData.Cancers.Count; ++i)
				{
					GlobalLevelData.Cancers[i].OnUpdate();
				}

				for (int i = 0; i < GlobalLevelData.AbilityCasters.Count; ++i)
				{
					GlobalLevelData.AbilityCasters[i].OnUpdate();
				}

			}
		}
	}
}