using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cells;

public abstract class CellParticle : MonoBehaviour
{
	[Header("Cell Particle")]
	protected Cell owner = null;

	// Spreading
	[SerializeField]
	protected Vector3 spreadPosition;

	// Targeting
	[SerializeField]
	protected KillerCell target = null;

	// Attributes
	[SerializeField]
	protected float lifeSpan = 0.0f;
	[SerializeField]
	protected float speed = 1.0f;
	[SerializeField]
	protected float distanceToReachSqr = 0.5f;

	// State 
	delegate void Action();
	private Action StateAction = null;

	void Update()
	{
		if (GlobalGameData.isGameplayPaused) return;
		OnUpdate();
	}

	public void OnUpdate()
	{
		StateAction();
	}

	public void Initialise(Vector3 _spreadPosition, KillerCell _target = null)
	{
		spreadPosition = _spreadPosition;
		target = _target;
		StateAction = Spread;
	}

	private void Idle()
	{
		lifeSpan -= Time.deltaTime;
		if (lifeSpan <= 0.0f)
		{
			OnDeathEffect();
		}
	}

	// State machine
	private void Spread()
	{
		Vector3 directionVector = spreadPosition - transform.position;
		if (Vector3.SqrMagnitude(directionVector) >= 1.0f)
		{
			transform.position += directionVector.normalized * Time.unscaledDeltaTime
				* speed * GlobalGameData.gameplaySpeed;
		}
		else if (target != null)
		{
			StateAction = FollowTarget;
		}
		else
		{
			StateAction = Idle;
		}
	}

	private void FollowTarget()
	{
		Vector3 directionVector = target.transform.position - transform.position;
		if (Vector3.SqrMagnitude(directionVector) <= distanceToReachSqr)
		{
			OnReachTarget();
		} 
		else
		{
			transform.position += directionVector.normalized * Time.unscaledDeltaTime
				* speed * GlobalGameData.gameplaySpeed;
		}
	}

	private void Dead() { }

	protected abstract void OnReachTarget();

	protected virtual void OnDeathEffect()
	{
		StateAction = Dead;
		Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (target != null) return;

		KillerCell cell = collider.GetComponent<KillerCell>();
		if (cell != null)
		{
			target = cell;
			StateAction = FollowTarget;
		}
	}
}
