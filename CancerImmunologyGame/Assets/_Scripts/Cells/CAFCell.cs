using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Cells.Cancers
{
    public class CAFCell : EvilCell
    {
		[Header("Debug (Read Only)")]
		[SerializeField]
		internal Cancer cancerOwner = null;

		public override bool isImmune => isDying;

		void Awake()
		{
			healthBar.MaxHealth = maxHealth;
			healthBar.Health = health;
		}

		protected override void OnDeath()
		{
			animator.SetTrigger("Death");
		}
	}
}
