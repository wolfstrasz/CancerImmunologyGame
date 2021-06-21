using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

using ImmunotherapyGame.UI;
using ImmunotherapyGame.Abilities;

namespace ImmunotherapyGame.Player
{
    public class ImmunotherpySwitcherPanel : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData = null;
        [SerializeField] private InterfaceControlPanel immunotherapySwitchPanel = null;

        [Header("Button Generation")]
        [SerializeField] private GameObject buttonPrefab = null;
        [SerializeField] private GameObject buttonsLayout = null;

        [SerializeField] [ReadOnly] private List<ImmunotherapySelectionButton> abilityButtons = new List<ImmunotherapySelectionButton>();


        // Start is called before the first frame update
        void Start()
        {
            if (playerData != null)
			{
                GenerateButtons();
			}
        }

        // Input handling
        PlayerControls playerControls = null;
        InputAction openImmunotherapies = null;

        private void Awake()
		{
            playerControls = new PlayerControls();
            openImmunotherapies = playerControls.Systems.SwitchImmunotherapy;
        }

		private void OnEnable()
		{
            playerControls.Enable();
            openImmunotherapies.started += OpenView;
		}

		private void OnDisable()
		{
            openImmunotherapies.started -= OpenView;
            playerControls.Disable();
        }

        private void OpenView (InputAction.CallbackContext context)
		{
            if (gameObject.activeInHierarchy)
            {
                if (immunotherapySwitchPanel.gameObject.activeInHierarchy)
                {
                    immunotherapySwitchPanel.Close();
                }
                else
                {
                    immunotherapySwitchPanel.Open();
                }
            }
        }

        public void OpenView()
		{
            if (gameObject.activeInHierarchy)
            {
                immunotherapySwitchPanel.Open();
            }
        }


		private void GenerateButtons()
		{
            for (int i = playerData.allAbilities.Count - 1; i >= 0 ; --i)
			{
                ImmunotherapySelectionButton btn = Instantiate(buttonPrefab, buttonsLayout.transform).GetComponent<ImmunotherapySelectionButton>();

                btn.ApplyData(playerData.allAbilities[i], i, this);
                immunotherapySwitchPanel.nodesToListen.Add(btn);
                abilityButtons.Add(btn);
            }

            immunotherapySwitchPanel.initialControlNode = abilityButtons[0];
		}


        internal void OnButtonMove(int buttonID, AxisEventData eventData)
		{
            int nextID = buttonID;

            if (eventData.moveDir == MoveDirection.Up)
			{
                nextID++;
			} 
            else if (eventData.moveDir == MoveDirection.Down)
			{
                nextID--;
			}

            if (nextID >= abilityButtons.Count)
            {
                nextID = 0;
            }
            else if (nextID < 0)
            {
                nextID = abilityButtons.Count - 1;
            }

            EventSystem.current.SetSelectedGameObject(abilityButtons[nextID].gameObject);
		}

        internal void OnButtonSubmit(int buttonID, Ability immonotherapyAbility)
		{
            if (abilityButtons[buttonID].IsUnlocked)
			{
                playerData.CurrentAbility = immonotherapyAbility;
			}
		}
    }
}
