using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cancers;

public class KillerParticle : MonoBehaviour
{

	[SerializeField]
	private SpriteRenderer render = null;
	[SerializeField]
	private float damage = 2.0f;
	[SerializeField]
	private float speed = 1.0f;
	[SerializeField]
	private Vector3 direction = Vector3.zero;
	[SerializeField]
	private Vector3 startPos = Vector3.zero;
	[SerializeField]
	private float distance = 1.0f;
	[SerializeField]
	private float distanceSqr = 1.0f;

	void Update()
	{
		if (GlobalGameData.isGameplayPaused) return;
		OnUpdate();
	}


	private void OnUpdate()
	{
		if (Vector3.SqrMagnitude(transform.position - startPos) > distanceSqr)
		{
			Destroy(gameObject);
			return;
		}

		transform.position = transform.position + direction * speed * Time.deltaTime;

	}

	public void Shoot(Vector3 _direction, float _distance, Color color)
	{
		render.color = color;
		startPos = transform.position;
		direction = _direction;
		distance = _distance;
		distanceSqr = _distance * distance;
		speed += Random.Range(-0.2f, 0.2f);
		gameObject.SetActive(true);
	}


	void OnTriggerEnter2D(Collider2D collider)
	{
		CancerCell cc = collider.gameObject.GetComponent<CancerCell>();
		if (cc)
		{
			if (cc.HitCell(damage))
			{
				Destroy(gameObject);
			}
		}
	}

}
