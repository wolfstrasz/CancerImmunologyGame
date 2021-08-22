using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.AI
{
    [System.Serializable]
    public class AIHomeData
    {
        [Header("Home Data")]
        public GameObject home;
        public float acceptableDistance;
    }
}
