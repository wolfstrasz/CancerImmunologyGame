using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ImmunotherapyGame.Core;

namespace ImmunotherapyGame
{
	[CreateAssetMenu]
	public class CellType : ScriptableObject
	{
		[SerializeField]
		private string cellName;
		[SerializeField]
		private List<CellType> enemyCells;
		[SerializeField]
		private float initialMaxHealth;
		[SerializeField]
		private float initialMaxEnergy;
		[SerializeField]
		private float initialSpeed;

		[ReadOnly]
		private float maxHealth;
		[ReadOnly]
		private float maxEnergy;
		[ReadOnly]
		private float speed;

		public float Speed => speed;
		public float MaxHealth => maxHealth;
		public float MaxEnergy => maxEnergy;
		private List<CellType> EnemyCells => enemyCells;

		public void ResetData()
		{
			maxHealth = initialMaxHealth;
			maxEnergy = initialMaxEnergy;
			speed = initialSpeed;
		}

	}

}
