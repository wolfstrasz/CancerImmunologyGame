using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Player
{
	[CreateAssetMenu(menuName = "PlayerAttackRangeSettings")]
	public class PlayerAttackRangeSettings : ScriptableObject
	{
		[Range(0.0f, 1.0f)]
		public float fovCutoff = 0.0f;
		[Range(1.0f, 2.0f)]
		public float width = 1.3f;
		[Range(0.0f, 1.0f)]
		public float widthCentre = 0.0f;
		[Range(-0.5f, 0.5f)]
		public float heightCutoff = 0.0f;
		[Range(0.0f, 2.0f)]
		public float scaleFactor = 1.0f;
		[Range(-1.0f, 1.0f)]
		public float offsetFromCell = 0.0f;
	}
}