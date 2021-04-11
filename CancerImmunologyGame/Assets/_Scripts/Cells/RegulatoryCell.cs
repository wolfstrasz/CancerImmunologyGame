using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class RegulatoryCell : MonoBehaviour
{
    public float shiftSpeed = 0.5f;
    public float energyDmg = -10.0f;
    public PathCreator path = null;
    public VertexPath pathToFollow = null;
    public CircleCollider2D coll = null;

    public float distanceTravelled = 0.0f;
    public float maxLengthDist = 0.0f;
    public float speed = 1.0f;

    ////////////////////////////////
    public bool isShooting = false;
    public float shootDelay = 3.0f;
	////////////////////////////////

	[SerializeField]
	private const float bumpcooldown = 2.0f;
	private float cooldown = 2.0f;

    [SerializeField]
    SpriteRenderer render = null;

    public GameObject particleToShoot = null;


    void Awake()
	{
        if (path != null)
        {
            pathToFollow = path.path;
            maxLengthDist = pathToFollow.length;
        } else
		{
            Debug.LogError("Unassigned path to follow for regulatory cell!");
		}
    }

    public void OnUpdate()
    {
		if (cooldown > 0.0f)
			cooldown -= Time.deltaTime;
    }

    public void OnFixedUpdate()
	{
        Move();
    }


    void Move()
    {
        distanceTravelled += Time.fixedDeltaTime * speed;
        if (distanceTravelled > maxLengthDist) distanceTravelled -= maxLengthDist;
        Vector3 newPos = pathToFollow.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Loop);
        transform.position = newPos;

        // Check for flipping
        Vector3 direction = newPos - transform.position;
        render.flipX = direction.x < 0.0f;
    }

	// SHOOTING

    public void StartShooting()
    {
        isShooting = true;
        StartCoroutine(Shooting());
    }

    public void StopShooting()
    {
        isShooting = false;
        StopCoroutine(Shooting());
    }

    IEnumerator Shooting()
    {
        while (isShooting)
        {
            yield return new WaitForSeconds(5.0f);
            if (GlobalGameData.isGameplayPaused) continue;

                float rotation = Random.Range(0, 360);
                Vector3 spreadVector = new Vector3(2.5f, 0.0f, 0.0f);
                spreadVector = Quaternion.Euler(0.0f, 0.0f, rotation) * spreadVector;

                GameObject particle = Instantiate(particleToShoot, transform.position, Quaternion.identity);
                RegulatoryParticle rp = particle.GetComponent<RegulatoryParticle>();
                rp.Initialise(transform.position + spreadVector);
        }
    }

	// BUMPING

    private void OnTriggerEnter2D(Collider2D collider)
    {
        KillerCell collidedKillerCell = collider.GetComponent<KillerCell>();
        if ( collidedKillerCell != null)
        {
			if (cooldown <= 0.0f)
				StartCoroutine(BumpPlayer(collidedKillerCell));
        }
    }


    public float ScaleIncrease;
    public float RadiusIncrease;

    public float prevRadius;
    public float prevScale;

    IEnumerator BumpPlayer(KillerCell target)
    {
        Debug.Log("STARTED BUMP");

        //StopMoving();
        prevRadius = coll.radius;
        prevScale = transform.localScale.x;

        float scaleToIncrease = ScaleIncrease - transform.localScale.x;
        float radiusToIncrease = RadiusIncrease - coll.radius;

        coll.radius = 0.1f;
        coll.isTrigger = false;

        while(transform.localScale.x < ScaleIncrease || coll.radius < RadiusIncrease)
        {
            transform.localScale +=  new Vector3(scaleToIncrease * Time.deltaTime * shiftSpeed, scaleToIncrease * Time.deltaTime * shiftSpeed, 0.0f);
            coll.radius += radiusToIncrease * Time.deltaTime * shiftSpeed;
        }

		target.ExhaustCell(Mathf.Abs(energyDmg));
		//PlayerUI.Instance.AddExhaustion(exhaust_dmg);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(StopBump());
    }

    IEnumerator StopBump()
    {
        Debug.Log("Stopping Bump");
        //StartMoving();
        float scaleToDecrease = transform.localScale.x - prevScale;
        float radiusToDecrease = coll.radius - prevRadius;

        while ( transform.localScale.x > prevScale || coll.radius > prevRadius)
        {
            transform.localScale -= new Vector3(scaleToDecrease * Time.deltaTime * shiftSpeed, scaleToDecrease * Time.deltaTime * shiftSpeed, 0.0f);
            coll.radius -= radiusToDecrease * Time.deltaTime * shiftSpeed;

        }

        transform.localScale = new Vector3(prevScale, prevScale, 1.0f);
        coll.radius = prevRadius;
        coll.isTrigger = true;
		cooldown = bumpcooldown;
        yield return new WaitForSeconds(0.1f);
    }
}
