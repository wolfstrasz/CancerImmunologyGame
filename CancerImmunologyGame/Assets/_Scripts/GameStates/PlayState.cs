using ImmunotherapyGame.Player;
using ImmunotherapyGame.Tutorials;
using ImmunotherapyGame.AI;
using ImmunotherapyGame.Audio;
using ImmunotherapyGame.UI.TopOverlay;

namespace ImmunotherapyGame
{
	namespace GameManagement
	{
		public class PlayState : GameState
		{
			public PlayState(GameStateController owner) : base(owner) { }

			internal override void OnStateEnter()
			{
				TopOverlayUI.Instance.GamePaused = false;
				GlobalLevelData.UpdateLevelData();
				BackgroundMusic.Instance.PlayMusic();
				TutorialManager.Instance.LoadLevelTutorials();
			}

			internal override void OnFixedUpdate()
			{
				foreach (PlayerController pc in GlobalLevelData.PlayerControllers)
				{
					pc.OnFixedUpdate();
				}

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

			internal override void OnUpdate()
			{
				TutorialManager.Instance.OnUpdate();

				foreach (PlayerController pc in GlobalLevelData.PlayerControllers)
				{
					pc.OnUpdate();
				}

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

				for (int i = 0; i < GlobalLevelData.Cancers.Count; ++i)
				{
					GlobalLevelData.Cancers[i].OnUpdate();
				}

				foreach (AIController controller in GlobalLevelData.AIKillerCells)
				{
					controller.OnUpdate();
				}

				for (int i = 0; i < GlobalLevelData.AbilityCasters.Count; ++i)
				{
					GlobalLevelData.AbilityCasters[i].OnUpdate();
				}

			}

			internal override void OnStateReEnter()
			{
				TopOverlayUI.Instance.GamePaused = false;

			}
		}
	}
}