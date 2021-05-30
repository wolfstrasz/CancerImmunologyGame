using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ImmunotherapyGame.Core;

namespace ImmunotherapyGame
{
	[CreateAssetMenu]
	public class CellType : ScriptableObject
	{
		public string cellName;
		public List<CellType> enemyCells;
		[Expandable]
		public StatAttribute maxHealth;
		[Expandable]
		public StatAttribute maxEnergy;
		[Expandable]
		public StatAttribute speed;

		public float maxHealthValue => maxHealth.CurrentValue;
		public float maxEnergyValue => maxEnergy.CurrentValue;
		public float speedValue => speed.CurrentValue;
	}

}
