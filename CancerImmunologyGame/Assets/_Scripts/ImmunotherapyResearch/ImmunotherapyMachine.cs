using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.ImmunotherapyResearchSystem
{
    public class ImmunotherapyMachine : MonoBehaviour
    {

        [SerializeField] private AudioSource source;
        [SerializeField] private AudioClip clip;
        [SerializeField] private Animator animator;
        [SerializeField] private bool isIdle = true;

        internal bool PlayAnimation()
		{

            if (isIdle)
            {

                animator.SetTrigger("BuyUpgrade");
                isIdle = false;
                return true;
            }
            return false;
		}

        public void OnFinishBuyAnimation()
		{
            source.PlayOneShot(clip);
		}

        public void OnEnterIdleAnimation()
		{
            isIdle = true;
		}
    }
}
