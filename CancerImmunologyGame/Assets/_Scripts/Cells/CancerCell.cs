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
    private Animator animator;

    public float health = 100;

    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.allCancerCells.Add(this);
    }

    void Awake()
    {
        float randomAngle = Random.Range(0.0f, 360.0f);
		hypoxicArea.transform.rotation = Quaternion.Euler(0, 0, randomAngle);
    }


    IEnumerator DestroyCell()
    {
        bodyBlocker.enabled = false;
        UIManager.Instance.allCancerCells.Remove(this);
		hypoxicArea.gameObject.SetActive(false);


        animator.SetTrigger("Apoptosis");
		yield return new WaitForSeconds(wait_before_destroy); 
		Destroy(hypoxicArea);
        Destroy(gameObject);
    }

    public void HitCell()
    {

        if (!first_hit)
        {
            first_hit = true;
            UIManager.Instance.StartRegulatoryCellTutorial();
        }
        health -= 20f;
        if (health <= 0)
        {
            StartCoroutine(DestroyCell());
        }
    }
}
