using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Abilities;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.Player
{
    [CreateAssetMenu (menuName = "Data/Player Data")]
    public class PlayerData : ScriptableObject
    {
        [SerializeField] [ReadOnly] private Cell currentCell;
        [SerializeField] [ReadOnly] private Ability currentAbility;
        [SerializeField] [ReadOnly] private ImmunotherapyCaster currentCaster;
        public List<Ability> allAbilities = new List<Ability>();


        public delegate void OnCurrentCellChanged();
        public OnCurrentCellChanged onCurrentCellChanged;

        public delegate void OnCurrentAbilityChanged();
        public OnCurrentAbilityChanged onCurrentAbilityChanged;

        public delegate void OnCurrentCasterChanged();
        public OnCurrentCasterChanged onCurrentCasterChanged;


        public Cell CurrentCell
		{
            get => currentCell;
            set
			{
                currentCell = value;
                if (onCurrentCellChanged != null)
				{
                    onCurrentCellChanged();
				}
			}
		}

        public Ability CurrentAbility
		{
            get => currentAbility;
            set
			{
                currentAbility = value;
                if (onCurrentAbilityChanged != null)
				{
                    onCurrentAbilityChanged();
				}
			}
		}

        public ImmunotherapyCaster CurrentCaster
		{
            get => currentCaster;
            set
			{
                currentCaster = value;
                if (onCurrentCasterChanged != null)
                    onCurrentCasterChanged();
			}
		}

		protected void OnEnable()
		{
            for (int i = 0; i < allAbilities.Count; ++i)
			{
                if (allAbilities[i].isUnlocked)
				{
                    CurrentAbility = allAbilities[i];
                    break;
				}
			}
		

            // If no ability is unlocked
            Debug.LogWarning("No initial ability is unlocked in player data abilities!");
		}
	}
}
