using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class RegulatoryParticle : MonoBehaviour
{
    public RegulatoryCell rc = null;
    public float exhaust_dmg = 10.0f;
    public float speed = 1.0f;
    public float Dist_To_Hit = 0.5f;

    // Spreading
    public Vector3 spreadPosition;
    public bool isSpreading = false;

    // Targeting
    public bool isFollowingTarget = false;
    public GameObject target = null;

    // Lifespan
    public float lifeSpan = 10.0f;

    void Update()
    {
        if (GlobalGameData.isPaused) return;

        if (isSpreading)
        {
            Spread();
        }
        else if (isFollowingTarget)
        {
            FollowTarget();
        }
        else
        {
            ReduceLife();
        }
    }

    private void ReduceLife()
    {
        lifeSpan -= Time.deltaTime;

        if (lifeSpan <= 0.0f)
        {
            Die();
        }
    }

    private void Die()
    {
        rc.particles.Remove(this);
        Destroy(gameObject);
    }
    private void Spread()
    {
        Vector3 directionVector = spreadPosition - transform.position;
        if (Vector3.SqrMagnitude(directionVector) >= 1.0f)
        {
            transform.position += directionVector.normalized * Time.deltaTime * speed;
        }
        else
        {
            isSpreading = false;
        }
    }

    private void FollowTarget()
    {
        Vector3 directionVector =  target.transform.position - transform.position;
        if (Vector3.SqrMagnitude(directionVector) <= Dist_To_Hit)
        {
            OnPlayerReached();

        } else
        {
            transform.position += directionVector.normalized * Time.deltaTime * speed;
        }
    }

    public void StartSpread(Vector3 _positionToReach)
    {
        spreadPosition = _positionToReach;
        isSpreading = true;
    }

    private void OnPlayerReached()
    {
        GlobalGameData.AddExhaustion(exhaust_dmg);
        Die();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Targets player
        if(collider.GetComponent<PlayerController>() != null)
        {
            target = collider.gameObject;
            isFollowingTarget = true;
            isSpreading = false;
        }
    }

}
