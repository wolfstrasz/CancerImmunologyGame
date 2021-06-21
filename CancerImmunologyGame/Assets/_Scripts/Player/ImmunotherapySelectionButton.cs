using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using ImmunotherapyGame.UI;
using ImmunotherapyGame.Abilities;

namespace ImmunotherapyGame.Player
{
	[RequireComponent(typeof(Selectable))]
    public class ImmunotherapySelectionButton : UIMenuNode, IMoveHandler, ISubmitHandler
    {
		[SerializeField] private Image thumbnail = null;
		[SerializeField] private GameObject lockedUI = null;

		[SerializeField] [ReadOnly] private int itemID = 0;
		[SerializeField] [ReadOnly] private ImmunotherpySwitcherPanel owner = null;
	    [SerializeField] [ReadOnly] Ability immunotherapyAbility = null;


		internal bool IsUnlocked => !lockedUI.activeInHierarchy;

		protected override void OnEnable()
		{
			base.OnEnable();
			if (immunotherapyAbility != null)
			{
				lockedUI.SetActive(!immunotherapyAbility.IsUnlocked);
				immunotherapyAbility.onUnlockedStateChanged += OnImmunotherapyAbilityUnlocked;
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			if (immunotherapyAbility != null)
			{
				lockedUI.SetActive(!immunotherapyAbility.IsUnlocked);
				immunotherapyAbility.onUnlockedStateChanged -= OnImmunotherapyAbilityUnlocked;
			}
		}

		private void OnImmunotherapyAbilityUnlocked()
		{
			lockedUI.SetActive(!immunotherapyAbility.IsUnlocked);
		}

		internal void ApplyData(Ability _immonotherapyAbility, int _itemID, ImmunotherpySwitcherPanel _owner ) 
		{
            immunotherapyAbility = _immonotherapyAbility;
			itemID = _itemID;
			owner = _owner;

            thumbnail.sprite = immunotherapyAbility.thumbnail;
            lockedUI.SetActive(!immunotherapyAbility.IsUnlocked);
		}

        public void OnMove(AxisEventData eventData)
        {
            owner.OnButtonMove(itemID, eventData);
		}

		public void OnSubmit(BaseEventData eventData)
		{
			owner.OnButtonSubmit(itemID, immunotherapyAbility);
		}
	}
}
