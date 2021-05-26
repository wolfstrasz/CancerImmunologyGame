using UnityEngine;
using ImmunotherapyGame.Tutorials;
using ImmunotherapyGame.UI.TopOverlay;

namespace ImmunotherapyGame
{
	namespace GameManagement
	{
		public class GameplayPauseState : GameState
		{
			public GameplayPauseState(GameStateController owner) : base(owner) { }

			internal override void OnFixedUpdate()
			{
			}

			internal override void OnStateEnter()
			{
				GlobalGameData.isGameplayPaused = true;
				TopOverlayUI.Instance.GamePaused = true;

			}

			internal override void OnStateExit()
			{
				GlobalGameData.isGameplayPaused = true;
			}

			internal override void OnStateReEnter()
			{
				TopOverlayUI.Instance.GamePaused = true;
			}

			internal override void OnUpdate()
			{
				TutorialManager.Instance.OnUpdate();
			}
		}
	}
}