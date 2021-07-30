using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ImmunotherapyGame.Core;

namespace ImmunotherapyGame
{
	[CreateAssetMenu (menuName = "MyAssets/Cell Type")]
	public class CellType : ScriptableObject
	{
		public string cellName;

		[Expandable] public StatAttribute maxHealth;
		[Expandable] public StatAttribute maxEnergy;
		[Expandable] public StatAttribute initialSpeed;
		[Expandable] public StatAttribute healthRegenPerSecond;
		[Expandable] public StatAttribute energyRegenPerSecond;

		public float HealthRegenPerSecond => healthRegenPerSecond.CurrentValue;
		public float EnergyRegenPerSecond => energyRegenPerSecond.CurrentValue;
		public float MaxHealth => maxHealth.CurrentValue;
		public float MaxEnergy => maxEnergy.CurrentValue;
		public float InitialSpeed => initialSpeed.CurrentValue;
	}

}
