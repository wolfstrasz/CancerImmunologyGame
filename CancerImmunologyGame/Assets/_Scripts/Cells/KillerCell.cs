using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cells;

public class KillerCell : Cell
{

	[SerializeField]
	private Rigidbody2D rb = null;
	[SerializeField]
	private KillerSense sense = null;

	[Header("Attributes")]
	[SerializeField]
	private float speed = 4.0f;
	[SerializeField]
	private float maxHealth = 100.0f;
	private float health = 0.0f;
	[SerializeField]
	private float maxExhaustion = 100.0f;
	private float exhaustion = 0.0f;

	public float Health { get => health; set => health = value; }
	public float Exhaustion { get => exhaustion; set => exhaustion = value; }
	   
	public void Initialise()
	{
		sense.CancerCellsInRange.Clear();
	}

	public List<CancerCell> GetCancerCellsInRange()
	{
		return sense.CancerCellsInRange;
	}

	public void ReceiveExhaustion(float value)
	{
		Debug.Log(gameObject.name + " received exhaustion of " + value);
		// Do stuff with exhaustion
	}

	public void ReceiveHealth(float value)
	{
		Debug.Log(gameObject.name + " received health of " + value);
		// Do stuff with exhaustion
	}

	internal float GetSlowDown()
	{
		if (GlobalGameData.isPowerUpOn) return 1.0f;
		return (maxExhaustion - exhaustion) / maxExhaustion;
	}

	// Movement
	public void Move(Vector2 movementVector)
	{
		Vector2 move = movementVector * speed * Time.fixedDeltaTime;

		rb.MovePosition(move * GetSlowDown() + rb.position);
	}

	// Attacking

}
