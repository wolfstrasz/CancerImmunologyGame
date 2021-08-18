using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Core;
using ImmunotherapyGame.UI;
using ImmunotherapyGame.Audio;

namespace ImmunotherapyGame
{ 
	namespace GameManagement
	{
		public class MainMenuState : GameState
		{
			public MainMenuState(GameStateController owner) : base(owner) { }

			internal override void OnFixedUpdate()
			{

			}

			internal override void OnStateEnter()
			{
				Debug.Log("Entering: MainMenu State");

				BackgroundMusic.Instance.StopMusic();
			}

			internal override void OnStateExit()
			{

			}

			internal override void OnStateReEnter()
			{
				Debug.Log("Re-Entering: MainMenu State");

			}

			internal override void OnUpdate()
			{
				if (!MainMenu.Instance.Opened)
				{
					MainMenu.Instance.RequestOpen();
				}
			}
		}
	}

}