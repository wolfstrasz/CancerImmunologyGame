using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ImmunotherapyGame.Abilities;

namespace ImmunotherapyGame
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class AuraEffect : MonoBehaviour
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

		protected virtual void OnEnable() { }

		protected virtual void OnDisable() { }

		protected virtual void FixedUpdate()
		{
			OnFixedUpdate();
		}

		protected virtual void OnLifeEnded()
		{
			Destroy(gameObject);
		}

		protected virtual void OnFixedUpdate()
		{
			transform.position = owner.transform.position;
			if (isEffectOverTime)
			{

				for (int i = 0; i < affectedCells.Count; ++i)
				{ 
					if (affectedCells[i] != null)
						auraEffectAbility.ApplyAbilityEffect(ownerCell, Time.fixedDeltaTime);
				}
			}

			if (lifetime > 0)
			{
				lifetime -= Time.fixedDeltaTime;
				if (lifetime <= 0)
				{
					OnLifeEnded();
				}
			}
		}


		protected virtual void ApplyAuraEffect(Cell cell)
		{
			auraEffectAbility.ApplyAbilityEffect(cell);
		}

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
					ApplyAuraEffect(cell);
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
