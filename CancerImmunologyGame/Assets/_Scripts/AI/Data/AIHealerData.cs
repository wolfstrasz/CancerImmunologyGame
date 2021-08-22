using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.AI
{
	[System.Serializable]
    public class AIHealerData
    {

		[Header("Healer Data")]
		[SerializeField] [ReadOnly] public HelperTCell bookedHelperTCell;
		[SerializeField] [ReadOnly] public GameObject bookingSpot;
		public float acceptableDistance;
	}
}
