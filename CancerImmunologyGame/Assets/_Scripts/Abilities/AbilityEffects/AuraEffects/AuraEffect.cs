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
		[SerializeField] [ReadOnly] protected Cell ownerCell = null;
		[SerializeField] [ReadOnly] protected GameObject owner = null;
		[SerializeField] [ReadOnly] protected AuraAbility auraEffectAbility = null;
		[SerializeField] [ReadOnly] protected float lifetime = 0f;
		[SerializeField] [ReadOnly] protected float auraRange = 0f;
		[SerializeField] [ReadOnly] protected bool isEffectOverTime;

		[Header("Targets")]
		[SerializeField] [ReadOnly] protected List<Cell> affectedCells = new List<Cell>();

		protected virtual void Awake() { coll.isTrigger = true; }

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
			if (cell && auraEffectAbility.CanHitCellType(cell.CellType) && cell != ownerCell)
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
			if (cell && auraEffectAbility.CanHitCellType(cell.CellType) && cell != ownerCell)
			{
				if (isEffectOverTime)
				{
					affectedCells.Remove(cell);
				}
			}
		}

		/// <summary>
		/// Applies the effect of the aura when on an entry collision between a cell and the aura
		/// </summary>
		/// <param name="cell"></param>
		protected virtual void ApplyAuraEffectOnCollision(Cell cell)
		{
			auraEffectAbility.ApplyAbilityEffect(cell);
		}

		/// <summary>
		/// Applies the effect of the aura on all cells currently in the aura range.
		/// The stat change is scaled by the fixedDeltaTime.
		/// </summary>
		protected virtual void ApplyAuraEffectOvertime()
		{
			for (int i = 0; i < affectedCells.Count; ++i)
			{
				if (affectedCells[i] != null)
					auraEffectAbility.ApplyAbilityEffect(affectedCells[i], Time.fixedDeltaTime);
			}
		}

		/// <summary>
		///  Applies the aura effect on the owner game object cell if such component exists.
		///  Links the aura entity with the aura ability that created it.
		/// </summary>
		/// <param name="_auraEffectAbility"></param>
		/// <param name="_owner"></param>
		public virtual void Apply(AuraAbility _auraEffectAbility, GameObject _owner)
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
