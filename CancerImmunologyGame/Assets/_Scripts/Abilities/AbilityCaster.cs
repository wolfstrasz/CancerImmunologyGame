using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.Abilities
{
    public abstract class AbilityCaster : MonoBehaviour
    {
        [Header("Linking")]
        [SerializeField] protected AudioSource audioSource = null;
        [SerializeField] protected Ability ability = null;

        [Header("Debug (ReadOnly)")]
        [SerializeField] [ReadOnly] protected List<CellType> cellTypesToTarget = null;
        [SerializeField] [ReadOnly] protected float currentCooldown;

        // Cooldown
        public bool IsOnCooldown => currentCooldown > 0f;

        // Casting
        public virtual bool CanCastAbility(float resourceValue)
        { 
            return !IsOnCooldown && resourceValue >= ability.EnergyCost;
        }

        protected virtual void OnEnable()
		{
            if (audioSource && ability != null)
            {
                audioSource.clip = ability.audioClip;

                if (ability.CooldownTime > 0f)
				{
                    GlobalLevelData.AbilityCasters.Add(this);

                }
            }

        }

        protected virtual void OnDisable()
        {
            if (ability != null)
			{
                if (ability.CooldownTime > 0f)
                {
                    GlobalLevelData.AbilityCasters.Remove(this);
                }
            }
        }

        public virtual void OnUpdate()
        {
            if (currentCooldown > 0f)
            {
                currentCooldown -= Time.deltaTime;

                if (currentCooldown <= 0f)
                {
                    currentCooldown = 0f;
                }
            }
        }

        public abstract float CastAbility();

        public virtual float CastAbility(GameObject target)
        {
            ability.CastAbility(this.gameObject, target);
            currentCooldown = ability.CooldownTime;
            if (audioSource)
            {
                audioSource.Stop();
                audioSource.Play();
            }

            return ability.EnergyCost;
        }

        public virtual float CastAbility(List<GameObject> targets)
        {
            ability.CastAbility(this.gameObject, targets);
            currentCooldown = ability.CooldownTime;
            if (audioSource)
            {
                audioSource.Stop();
                audioSource.Play();
            }

            return ability.EnergyCost;
        }

    

      
    }
}
