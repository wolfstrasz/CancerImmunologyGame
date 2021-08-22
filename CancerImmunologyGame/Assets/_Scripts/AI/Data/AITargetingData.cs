using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.AI
{
	[System.Serializable]
    public class AITargetingData
    {
        [Header("Target Data")]
        [SerializeField] [ReadOnly] public GameObject currentTarget;
        [SerializeField] [ReadOnly] public float acceptableDistanceFromCurrentTarget;
    }
}
