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
        public delegate void OnCooldownEnded();
        public OnCooldownEnded onCooldownEnded;
        public bool IsOnCooldown => currentCooldown > 0f;
        public float CooldownTimePassed => (ability == null ? 0f : ability.CooldownTime - currentCooldown);

        // Casting
        public delegate void OnAbilityCasted();
        public OnAbilityCasted onAbilityCasted;

        public virtual bool CanCastAbility(float resourceValue)
            => !IsOnCooldown && resourceValue > ability.EnergyCost;

        protected virtual void OnEnable()
		{
            if (audioSource && ability != null)
            {
                audioSource.clip = ability.audioClip;
            }
            GlobalLevelData.AbilityCasters.Add(this);
        }


        protected virtual void OnDisable()
        {
            GlobalLevelData.AbilityCasters.Remove(this);
        }

        public virtual void OnUpdate()
        {
            if (currentCooldown > 0f)
            {
                currentCooldown -= Time.deltaTime;

                if (currentCooldown <= 0f)
                {
                    currentCooldown = 0f;

                    if (onCooldownEnded != null)
                    {
                        onCooldownEnded();
                    }
                }
            }
        }

        public abstract float CastAbility();

        public virtual float CastAbility(GameObject target)
        {
            Debug.Log(this.name + ": casted ability on " + target.name);
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
            Debug.Log(this.name + ": casted ability on all targets");

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
