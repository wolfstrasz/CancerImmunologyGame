using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.Abilities
{
    public abstract class Ability : ScriptableObject
    {
        [Header("Ability Description")]
        public new string name;
        public AudioClip audioClip;

        [Header("Non-Composite Data")]
        [Expandable] [SerializeField] protected StatAttribute range;
        [Expandable] [SerializeField] protected StatAttribute energyCost;
        [Expandable] [SerializeField] protected StatAttribute cooldownTime;
        [Expandable] [SerializeField] protected List<CellType> typesToBeInRange;

        [Header("Effect data")]
        [SerializeField] protected EffectOnTargetCell effectOnTargetCell;

        // Attributes
        public float Range => (range != null ? range.CurrentValue : 0f);
        public float EnergyCost => (energyCost != null ? energyCost.CurrentValue : 0f);
        public float CooldownTime => (cooldownTime != null ? cooldownTime.CurrentValue : 0f);
        public List<CellType> TypesToBeInRange => ((typesToBeInRange == null || typesToBeInRange.Count == 0) ? null : typesToBeInRange);

        // Casting
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

        // Application
        public bool CanHitCellType(CellType cellType)
        {
            if (effectOnTargetCell != null)
			{
                return effectOnTargetCell.CanHitCellType(cellType);
            }
            return false;
        }

        public void ApplyAbilityEffect(Cell cell, float multiplier = 1f)
        {
            if (effectOnTargetCell != null)
            {
                effectOnTargetCell.ApplyEffect(cell, multiplier);
            }
        }

        public void UndoAbilityEffect(Cell cell, float multiplier = 1f)
        {
            if (effectOnTargetCell != null)
            {
                effectOnTargetCell.UndoEffect(cell, multiplier);
            }
        }


        [System.Serializable]
        protected class EffectOnTargetCell : ISerializationCallbackReceiver
        {
            [SerializeField] private List<CellType> applicableCellTypes = new List<CellType>();

            [SerializeField] private bool isDamaging = false;
            [SerializeField] [ReadOnly] float damagingMultiplier;

            [Expandable] public StatAttribute healthEffect;
            [Expandable] public StatAttribute energyEffect;
            [Expandable] public StatAttribute speedEffect;

            internal bool CanHitCellType(CellType cellType)
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

            internal void ApplyEffect(Cell targetCell, float multiplier = 1f)
            {
                if (healthEffect != null)
                {
                    targetCell.ApplyHealthAmount(healthEffect.CurrentValue * multiplier * damagingMultiplier);
                }

                if (energyEffect != null)
                {
                    targetCell.ApplyEnergyAmount(energyEffect.CurrentValue * multiplier * damagingMultiplier);
                }

                if (speedEffect != null)
                {
                    targetCell.ApplySpeedAmount(speedEffect.CurrentValue * multiplier * damagingMultiplier);
                }
            }

            internal void UndoEffect(Cell targetCell, float multiplier = 1f)
            {
                if (healthEffect != null)
                {
                    targetCell.ApplyHealthAmount(-healthEffect.CurrentValue * multiplier * damagingMultiplier);
                }

                if (energyEffect != null)
                {
                    targetCell.ApplyEnergyAmount(-energyEffect.CurrentValue * multiplier * damagingMultiplier);
                }

                if (speedEffect != null)
                {
                    targetCell.ApplySpeedAmount(-speedEffect.CurrentValue * multiplier * damagingMultiplier);
                }
            }

			public void OnBeforeSerialize()
			{
			}

			public void OnAfterDeserialize()
			{
                damagingMultiplier = isDamaging ? -1.0f : 1.0f;
			}

        }

    }
}
