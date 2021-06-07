using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.Abilities
{
	[RequireComponent(typeof(CircleCollider2D))]
    public class AbilityCaster : MonoBehaviour
    {
		[Header("Linking")]
		[SerializeField] protected CircleCollider2D coll = null;
		[SerializeField] protected AudioSource audioSource = null;
		[SerializeField] protected Ability ability = null;
		[Header("Debug (ReadOnly)")]
		[SerializeField] [ReadOnly] protected List<CellType> cellTypesToTarget = null;
		[SerializeField] [ReadOnly] protected List<GameObject> targetsInRange = null;

		protected float currentCooldown;

		public delegate void OnCooldownEnded();
		public OnCooldownEnded onCooldownEnded;

		public bool IsOnCooldown => currentCooldown > 0f;
		public bool HasTargetsInRange => (targetsInRange != null ? targetsInRange.Count > 0 : true);
		public bool CanCastAbility(float resourceValue)
			=> !IsOnCooldown && HasTargetsInRange && resourceValue > ability.EnergyCost;
		public List<GameObject> TargetsInRange => (targetsInRange != null ? targetsInRange : new List<GameObject>());

		protected void Awake()
		{
			if (audioSource)
				audioSource.clip = ability.audioClip;
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
			}
		}

		protected virtual void OnEnable()
		{
			coll.radius = ability.Range;
			GlobalLevelData.AbilityCasters.Add(this);
		}

		protected virtual void OnDisable()
		{
			GlobalLevelData.AbilityCasters.Remove(this);
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

		public virtual void OnUpdate()
		{
			if (currentCooldown > 0f)
			{
				currentCooldown -= Time.deltaTime;

				if (currentCooldown <= 0f)
				{
					currentCooldown = 0f;

					if (onCooldownEnded != null)
					{
						onCooldownEnded();
					}
				}
			}
		}

	

		/// <summary>
		/// Casts the ablity on the targets and returns the cost of the ability.
		/// Always use CanCastAbility before CastAbility as you might cast it without cost
		/// </summary>
		/// <param name="targets"></param>
		/// <returns></returns>
		public virtual float CastAbility (List<GameObject> targets)
		{
			Debug.Log(this.name + ": casted ability on all targets");

			ability.CastAbility(this.gameObject, targets);
			currentCooldown = ability.CooldownTime;
			if (audioSource)
			{
				audioSource.Stop();
				audioSource.Play();
			}

			return ability.EnergyCost;
		}

		public virtual float CastAbilityOnAllTargetsInRange()
		{
			Debug.Log(this.name + ": casted ability on all targets in range");

			return CastAbility(targetsInRange);
		}

		public virtual float CastAbility(GameObject target)
		{
			Debug.Log(this.name + ": casted ability on " + target.name);
			ability.CastAbility(this.gameObject, target);
			currentCooldown = ability.CooldownTime;
			if (audioSource)
			{
				audioSource.Stop();
				audioSource.Play();
			}

			return ability.EnergyCost;
		}

	}
}
