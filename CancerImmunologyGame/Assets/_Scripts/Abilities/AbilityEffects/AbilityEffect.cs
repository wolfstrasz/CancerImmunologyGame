using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.Abilities
{
    public abstract class AbilityEffect : MonoBehaviour
    {
        [SerializeField][ReadOnly] internal AbilityEffectType type = null;

        protected virtual void OnEnable() { }

        protected virtual void OnDisable() { }

        internal abstract void OnFixedUpdate();

        /// <summary>
        /// Notifies the Ability Effect Manager that the particle has finished 
        /// its functionality and it is ready to be reused.
        /// </summary>
        internal virtual void OnLifeEnded()
		{
            AbilityEffectManager.Instance.OnEffectLifeEnd(this);
		}
    }
}
