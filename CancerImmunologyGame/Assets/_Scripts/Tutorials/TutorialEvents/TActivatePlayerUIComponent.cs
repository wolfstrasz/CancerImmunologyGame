using UnityEngine;
using ImmunotherapyGame.Player;

namespace ImmunotherapyGame.Tutorials
{
	public class TActivatePlayerUIComponent : TutorialEvent
	{
		//[Header("Attributes")]
		//[SerializeField]
		//PlayerUIPanels panel = PlayerUIPanels.IMMUNOTHERAPY;

		protected override void OnEndEvent()
		{
		}

		protected override void OnStartEvent()
		{
			//if (panel == PlayerUIPanels.IMMUNOTHERAPY)
			//{
			//	PlayerUI.Instance.ActivateImmunotherapyPanel();
			//}
			//else if (panel == PlayerUIPanels.PLAYER_INFO_HEALTHBAR)
			//{
			//	PlayerUI.Instance.ActivatePlayerInfoPanel();
			//} 
			//else if (panel == PlayerUIPanels.PLAYER_INFO_ENERGYBAR)
			//{
			//	PlayerUI.Instance.ActivatePlayerInfoPanelEnergyBar();
			//}
			//else if (panel == PlayerUIPanels.EVERYTHING)
			//{
			//	PlayerUI.Instance.ActivateImmunotherapyPanel();
			//	PlayerUI.Instance.ActivatePlayerInfoPanel();
			//	PlayerUI.Instance.ActivatePlayerInfoPanelEnergyBar();

			//}
		}

		protected override bool OnUpdateEvent()
		{
			return true;
		}


	}
}
