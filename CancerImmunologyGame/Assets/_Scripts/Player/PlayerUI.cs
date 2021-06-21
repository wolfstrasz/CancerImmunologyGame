using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ImmunotherapyGame.Core;


namespace ImmunotherapyGame.Player
{
	public class PlayerUI : MonoBehaviour
	{

		[Header("UI links")]
		[SerializeField]
		private PlayerData playerData = null;

		[SerializeField]
		private PlayerCellInfoPanel playerCellInfoPanel = null;
		[SerializeField]
		private ImmunotherapyInfoPanel immunotherapyInfoPanel = null;
		[SerializeField]
		private ImmunotherpySwitcherPanel immunotherpySwitcherPanel = null;

		public bool PlayerCellInfoPanelActive
		{
			get
			{
				return playerCellInfoPanel.gameObject.activeInHierarchy;
			}
			set
			{
				playerCellInfoPanel.gameObject.SetActive(value);
			}
		}

		public bool ImmunotherapyInfoPanel
		{
			get
			{
				return immunotherapyInfoPanel.gameObject.activeInHierarchy;
			}
			set
			{
				immunotherapyInfoPanel.gameObject.SetActive(value);
				immunotherpySwitcherPanel.gameObject.SetActive(value);
			}
		}
	}

	public enum PlayerUIPanels { CELL_INFO, ABILITY_INFO, EVERYTHING }

}
