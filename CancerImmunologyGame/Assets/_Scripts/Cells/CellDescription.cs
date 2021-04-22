using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame
{
	[CreateAssetMenu(menuName = "Cellpedia Item")]
	public class CellDescription : ScriptableObject
	{
		public string cellname;
		[TextArea(5, 10)]
		public string description;
		public string animatorTrigger;
		public Sprite sprite;
		public float scale;

		public Sprite note1;
		public Sprite note2;
		public Sprite note3;

		public Vector3 scaleVector => new Vector3(scale, scale, 1.0f);

	}
}