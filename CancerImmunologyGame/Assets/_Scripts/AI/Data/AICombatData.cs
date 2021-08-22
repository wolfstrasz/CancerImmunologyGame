using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Cancers;

namespace ImmunotherapyGame.AI
{
    [System.Serializable]
    public class AICombatData 
    {
        [Header("Cancer Data")]
        public TargetFocus focusType = TargetFocus.CLOSEST;
        public List<Cancer> cancersToFight;
        public float acceptableDistance;
        [SerializeField] [ReadOnly] public Cancer currentCancerTarget;


        [System.Serializable]
        public enum TargetFocus { CLOSEST, MOST_SEVERE }
    }
}
