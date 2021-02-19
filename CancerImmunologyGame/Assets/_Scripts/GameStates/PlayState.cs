using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Bloodflow;

namespace Core
{
	namespace GameManagement
	{
		public class PlayState : GameState
		{
			public PlayState(GameStateController owner) : base(owner) { }

			internal override void OnStateEnter()
			{
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

#if !REMOVE_PLAYER_DEBUG
				if (Input.GetKeyDown(KeyCode.R))
				{
					PlayerUI.Instance.ActivateImmunotherapyPanel();
				}
				if (Input.GetKeyDown(KeyCode.Z))
				{
					CellpediaUI.Cellpedia.Instance.UnlockCellDescription(CellpediaCells.TKILLER);
				}
				if (Input.GetKeyDown(KeyCode.X))
				{
					CellpediaUI.Cellpedia.Instance.UnlockCellDescription(CellpediaCells.DENDRITIC);
				}
				if (Input.GetKeyDown(KeyCode.C))
				{
					CellpediaUI.Cellpedia.Instance.UnlockCellDescription(CellpediaCells.CANCER);
				}
				if (Input.GetKeyDown(KeyCode.V))
				{
					CellpediaUI.Cellpedia.Instance.UnlockCellDescription(CellpediaCells.THELPER);
				}
				if (Input.GetKeyDown(KeyCode.B))
				{
					CellpediaUI.Cellpedia.Instance.UnlockCellDescription(CellpediaCells.REGULATORY);
				}

#endif
			}
		}
	}
}