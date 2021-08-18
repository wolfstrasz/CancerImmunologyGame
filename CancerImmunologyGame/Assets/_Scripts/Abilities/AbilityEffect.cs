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

        internal virtual void OnLifeEnded()
		{
            AbilityEffectManager.Instance.OnEffectLifeEnd(this);
		}
    }
}
