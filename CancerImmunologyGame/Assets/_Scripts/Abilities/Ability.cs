using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.Abilities
{
    [CreateAssetMenu(menuName = "Abilities/Ability")]
    public abstract class Ability : ScriptableObject
    {
        [Header ("Ability Description")]
        public new string name;
        public Sprite thumbnail;
        public AudioClip audioClip;
        [TextArea(2, 10)] public string description;

        [Header ("Non-Composite Data")]
        [Expandable] public StatAttribute range;
        [Expandable] public StatAttribute energyCost;
        [Expandable] public StatAttribute cooldownTime;
        [Expandable] public List<CellType> typesToBeInRange;

        public float Range => (range != null ? range.CurrentValue : 0f);
        public float EnergyCost => (energyCost != null ? energyCost.CurrentValue : 0f);
        public float CooldownTime => (cooldownTime != null ? cooldownTime.CurrentValue : 0f);
        public List<CellType> TypesToBeInRange => ((typesToBeInRange == null || typesToBeInRange.Count == 0) ? null : typesToBeInRange);

        [Header ("Effect data")]
        [SerializeField] public EffectOnTargetCell effectOnTargetCell;
        [SerializeField] private List<CellType> applicableCellTypes = new List<CellType>();


        public abstract bool CastAbility(GameObject abilityCaster, GameObject target);
        public virtual bool CastAbility(GameObject abilityCaster, List<GameObject> targets)
		{
            bool success = true;
            for (int i = 0; i < targets.Count; ++i)
			{
                success &= CastAbility(abilityCaster, targets[i]);
			}
            return success;
		}

        public bool CanHitCellType (CellType cellType)
		{
            for (int i = 0; i < applicableCellTypes.Count; ++i)
            {
                if (applicableCellTypes[i] == cellType)
                {
                    return true;
                }
            }
            return false;
        }

        public void ApplyAbilityEffect (Cell cell, float multiplier = 1f)
		{
            if (effectOnTargetCell != null)
            {
                effectOnTargetCell.ApplyEffect(cell, multiplier);
            }
        }

        public void UndoAbilityEffect (Cell cell, float multiplier = 1f)
		{
            if (effectOnTargetCell != null)
            {
                effectOnTargetCell.UndoEffect(cell, multiplier);
            }
        }

        [System.Serializable]
        public class EffectOnTargetCell
		{
            [Expandable] public StatAttribute healthEffect;
            [Expandable] public StatAttribute energyEffect;
            [Expandable] public StatAttribute speedEffect;

            internal void ApplyEffect(Cell targetCell, float multiplier = 1f)
			{
                if (healthEffect != null)
				{
                    targetCell.ApplyHealthAmount(healthEffect.CurrentValue * multiplier);
				}

                if (energyEffect != null)
				{
                    targetCell.ApplyEnergyAmount(energyEffect.CurrentValue * multiplier);
				}

                if (speedEffect != null)
				{
                    targetCell.ApplySpeedAmount(speedEffect.CurrentValue * multiplier);
				}
			}

            internal void UndoEffect (Cell targetCell, float multiplier = 1f)
			{
                if (healthEffect != null)
                {
                    targetCell.ApplyHealthAmount(-healthEffect.CurrentValue * multiplier);
                }

                if (energyEffect != null)
                {
                    targetCell.ApplyEnergyAmount(-energyEffect.CurrentValue * multiplier);
                }

                if (speedEffect != null)
                {
                    targetCell.ApplySpeedAmount(-speedEffect.CurrentValue * multiplier);
                }
            }
        }
    }
}
