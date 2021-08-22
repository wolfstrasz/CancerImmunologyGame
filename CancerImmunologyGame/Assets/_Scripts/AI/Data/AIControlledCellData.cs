using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.AI
{
	[System.Serializable]
	public class AIControlledCellData
	{
		[Header("Controler Data")]
		[SerializeField] [ReadOnly] public Cell controlledCell;
		[SerializeField] [ReadOnly] public float speed;
		[SerializeField] [ReadOnly] public Vector2 movementDirection;

		public GameObject controlledObject => controlledCell.gameObject;
	}
}
