using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HelperCellParticle : MonoBehaviour
{
	[Header("Helper Cell Particle Attributes")]
	[SerializeField]
	private new Collider2D collider;
	[SerializeField]
	private SpriteRenderer render;

	[SerializeField]
	private float healthRegeneration = 0.35f;
	[SerializeField]
	private float energyRegeneration = 0.30f;


	// Generic particle requirements
	[SerializeField]
	private float speed = 1f;

	private Vector3 direction = Vector3.zero;
	private float lifetime = 0f;

	public void SetParticleData(Vector3 direction, float lifetime)
	{
		this.direction = direction;
		this.lifetime = lifetime;
	}

	void Update()
	{
		lifetime -= Time.deltaTime;
		if (lifetime < 0f)
		{
			DestroyParticle();
		}
	}

	void FixedUpdate()
	{
		transform.position = transform.position + direction * Time.fixedDeltaTime * speed;
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		KillerCell cell = collider.GetComponent<KillerCell>();
		if (cell != null)
		{
			cell.AddEnergy(energyRegeneration);
			cell.AddHealth(healthRegeneration);
			DestroyParticle();
		}
	}

	void DestroyParticle()
	{
		lifetime = 0f;
		collider.enabled = false;
		render.enabled = false;
		Destroy(gameObject, 0.5f);
	}
}

