using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancerCell : MonoBehaviour
{

    public static bool first_hit = false; // should switch to a tutorial pop-up event
	public static float wait_before_destroy = 3.0f; // Must remove hard-coding value
	[SerializeField]
    private CircleCollider2D bodyBlocker = null;
    [SerializeField]
    private GameObject hypoxicArea = null;
    [SerializeField]
    private Animator animator = null;

    public float health = 100;

    void Awake()
    {
        float randomAngle = Random.Range(0.0f, 360.0f);
		hypoxicArea.transform.rotation = Quaternion.Euler(0, 0, randomAngle);
    }


    IEnumerator DestroyCell()
    {
        bodyBlocker.enabled = false;
		hypoxicArea.gameObject.SetActive(false);

        animator.SetTrigger("Apoptosis");
		yield return new WaitForSeconds(wait_before_destroy); 
		Destroy(hypoxicArea);
        Destroy(gameObject);
    }

    public void HitCell()
    {
        health -= 20f;
        if (health <= 0)
        {
            StartCoroutine(DestroyCell());
        }
    }
}
