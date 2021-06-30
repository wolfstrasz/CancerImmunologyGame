using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.Abilities
{
	[RequireComponent(typeof(CircleCollider2D))]
    public class RangedAbilityCaster : AbilityCaster
    {
		[Header("RangedAbilityCaster")]
		[SerializeField] protected CircleCollider2D coll = null;
		[SerializeField] [ReadOnly] protected List<GameObject> targetsInRange = null;

		// Targeting
		public bool HasTargetsInRange => (targetsInRange != null ? targetsInRange.Count > 0 : true);
		public List<GameObject> TargetsInRange => (targetsInRange != null ? targetsInRange : new List<GameObject>());

		public override bool CanCastAbility(float resourceValue)
		{
			return base.CanCastAbility(resourceValue) && HasTargetsInRange;
		}

		protected virtual void Awake()
		{
			// Find initial cells in range
			coll.radius = ability.Range;
			cellTypesToTarget = ability.TypesToBeInRange;
			if (cellTypesToTarget != null)
			{
				Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, coll.radius, LayerMask.GetMask("CellBody"));
				for (int i = 0; i < colliders.Length; ++i)
				{
					Cell colliderCell = colliders[i].GetComponent<Cell>();
					if (colliderCell != null)
					{
						if (cellTypesToTarget.Contains(colliderCell.cellType) && !targetsInRange.Contains(colliderCell.gameObject))
						{
							targetsInRange.Add(colliderCell.gameObject);
						}
					}
				}
			} else
			{
				coll.enabled = false;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			coll.radius = ability.Range;
		}


		protected virtual void OnTriggerEnter2D(Collider2D collider)
		{
			Debug.Log("Ability caster 2D triggered by " + collider.gameObject);
			Cell cell = collider.gameObject.GetComponent<Cell>();
			if (cell != null)
			{
				if (cellTypesToTarget.Contains(cell.cellType))
				{
					if (!targetsInRange.Contains(cell.gameObject))
						targetsInRange.Add(cell.gameObject);
				}
			}
		}

		protected virtual void OnTriggerExit2D(Collider2D collider)
		{
			Cell cell = collider.gameObject.GetComponent<Cell>();
			if (cell != null)
			{
				if (cellTypesToTarget.Contains(cell.cellType))
				{
					if (targetsInRange.Contains (cell.gameObject))
						targetsInRange.Remove(cell.gameObject);
				}
			}
		}


		public override float CastAbility()
		{
			Debug.Log(this.name + ": casted ability on all targets in range");

			if (targetsInRange != null && targetsInRange.Count > 0)
			{
				return CastAbility(targetsInRange);
			}
			else
			{
				Debug.Log("Did not manage to cast ability on all targets in range");
				return 0f;
			}
		}


	}
}
