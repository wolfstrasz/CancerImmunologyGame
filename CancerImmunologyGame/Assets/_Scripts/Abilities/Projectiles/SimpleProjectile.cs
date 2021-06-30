using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame
{  
	public class SimpleProjectile : Projectile
	{

		[Header("Simple particle")]
		[SerializeField]
		private List<Sprite> sprites = new List<Sprite>();


		protected override void Awake()
		{
			base.Awake();
			if (sprites.Count > 0)
			{
				int randomInt = Random.Range(0, sprites.Count);
				render.sprite = sprites[randomInt];
			}

		}

		protected override void OnOutOfRange()
		{
			OnEndOfEffect();
		}

		protected override void OnCollisionWithTarget(Cell cell)
		{
			projectileAbility.ApplyAbilityEffect(cell);
			OnEndOfEffect();
		}

		protected override void OnEndOfEffect()
		{
			coll.enabled = false;
			render.enabled = false;
			Destroy(gameObject);
		}

	}
}