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
		[Expandable]
		public StatAttribute maxHealth;
		[Expandable]
		public StatAttribute maxEnergy;
		[Expandable]
		public StatAttribute initialSpeed;

		public float MaxHealth => maxHealth.CurrentValue;
		public float MaxEnergy => maxEnergy.CurrentValue;
		public float InitialSpeed => initialSpeed.CurrentValue;
	}

}
