using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ImmunotherapyGame.Abilities
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class AuraEffect : AbilityEffect
	{

        [Header("Linking")]
        [SerializeField] protected Animator animator = null;
        [SerializeField] protected SpriteRenderer render = null;
        [SerializeField] protected CircleCollider2D coll = null;


		[Header("Attributes")]
		[SerializeField] [ReadOnly] protected bool isEffectOverTime;
		[SerializeField] [ReadOnly] protected Cell ownerCell = null;
		[SerializeField] [ReadOnly] protected GameObject owner = null;
		[SerializeField] [ReadOnly] protected AuraEffectAbility auraEffectAbility = null;
		[SerializeField] [ReadOnly] protected float lifetime = 0f;
		[SerializeField] [ReadOnly] protected float auraRange = 0f;

		[Header("Targets")]
		[SerializeField] [ReadOnly] protected List<Cell> affectedCells = new List<Cell>();

		protected virtual void Awake() { coll.isTrigger = true; }

		/* ABILITY EFFECT */

		internal override void OnFixedUpdate()
		{
			if (owner == null || !owner.activeInHierarchy)
			{
				OnLifeEnded();
				return;
			}

			transform.position = owner.transform.position;

			if (isEffectOverTime)
			{
				ApplyAuraEffectOvertime();
			}

			if (lifetime > 0f)
			{
				lifetime -= Time.fixedDeltaTime;
				if (lifetime <= 0f)
				{
					OnLifeEnded();
				}
			}
		}

		/* AURA EFFECT */
		protected virtual void OnTriggerEnter2D(Collider2D coll)
		{
			Cell cell = coll.GetComponent<Cell>();
			if (cell && auraEffectAbility.CanHitCellType(cell.cellType) && cell != ownerCell)
			{

				if (!affectedCells.Contains(cell)){
					affectedCells.Add(cell);
				}

				if (!isEffectOverTime) // Apply single time on first collision
				{
					ApplyAuraEffectOnCollision(cell);
				}
			}
		}

		protected virtual void OnTriggerExit2D(Collider2D coll)
		{

			Cell cell = coll.GetComponent<Cell>();
			if (cell && auraEffectAbility.CanHitCellType(cell.cellType) && cell != ownerCell)
			{
				if (isEffectOverTime)
				{
					affectedCells.Remove(cell);
				}
			}
		}

		protected virtual void ApplyAuraEffectOnCollision(Cell cell)
		{
			auraEffectAbility.ApplyAbilityEffect(cell);
		}

		protected virtual void ApplyAuraEffectOvertime()
		{
			for (int i = 0; i < affectedCells.Count; ++i)
			{
				if (affectedCells[i] != null)
					auraEffectAbility.ApplyAbilityEffect(affectedCells[i], Time.fixedDeltaTime);
			}
		}

		public virtual void Apply(AuraEffectAbility _auraEffectAbility, GameObject _owner)
		{
			affectedCells.Clear();
			owner = _owner;
			ownerCell = _owner.GetComponent<Cell>();
			auraEffectAbility = _auraEffectAbility;

			lifetime = auraEffectAbility.Lifetime;
			isEffectOverTime = auraEffectAbility.IsAuraEffectOverTime;
			coll.radius = auraEffectAbility.AuraRange;
			if (ownerCell == null)
			{
				Debug.LogWarning("Status effect (" + gameObject.name + ") applied to " + _owner.name + " does not operate on a cell.");
			}

		}

	}
}
