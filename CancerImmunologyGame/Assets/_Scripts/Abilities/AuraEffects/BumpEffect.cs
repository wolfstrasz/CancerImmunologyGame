using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ImmunotherapyGame.Abilities;

namespace ImmunotherapyGame
{
    public class BumpEffect : AuraEffect
    {

		[Header("Bump Attributes")]
		[SerializeField] float bumpPower = 1f;
        [SerializeField] protected Vector3 onBumpScaleIncrease = Vector3.one;
		[SerializeField] [ReadOnly] Vector3 ownerOriginalScale;
		[SerializeField] [ReadOnly] Vector3 auraOriginalScale;
		[SerializeField] [ReadOnly] bool isBumpedUp = false;

		protected override void OnFixedUpdate()
		{
			base.OnFixedUpdate();

			if (isBumpedUp)
			{
				owner.transform.localScale = ownerOriginalScale;
				transform.localScale = auraOriginalScale;
				isBumpedUp = false;
			}
		}

		protected override void ApplyAuraEffectOnCollision(Cell cell)
		{
			base.ApplyAuraEffectOnCollision(cell);

			owner.transform.localScale = ownerOriginalScale + onBumpScaleIncrease;
			transform.localScale = auraOriginalScale + onBumpScaleIncrease;


			if (!cell.isImmune )
			{
				// Calculate bump direction
				Vector3 bumpDirection = (cell.transform.position - owner.transform.position).normalized;
				
				cell.gameObject.transform.position += bumpPower * bumpDirection;
				isBumpedUp = true;
			}
		}

		public override void Apply(AuraEffectAbility _auraEffectAbility, GameObject _owner)
		{
			base.Apply(_auraEffectAbility, _owner);
			ownerOriginalScale = owner.transform.localScale;
			auraOriginalScale = transform.localScale;
		}
	}
}
