using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancerCell : MonoBehaviour
{
	internal Cancer cancer = null;

    public static bool first_hit = false; // should switch to a tutorial pop-up event
	public static float wait_before_destroy = 3.0f; // Must remove hard-coding value
	[SerializeField]
    private CircleCollider2D bodyBlocker = null;
	[SerializeField]
	public CircleCollider2D divisionBodyBlocker = null;

	[SerializeField]
    private GameObject hypoxicArea = null;
    [SerializeField]
    private Animator animator;
	[SerializeField]
	private SpriteRenderer render;

    public float health = 100;

	internal void SetSortOrder (int sortOrder)
	{
		render.sortingOrder = sortOrder;
	}

    void Awake()
    {
		hypoxicArea.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f));
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

	private float rotationAngle;
	private float _divideToLocation;


	// Control calls
	public void StartPrepareDivision(Vector3 _divideToLocation, float _rotationAngle)
	{
		divisionBodyBlocker.gameObject.SetActive(true);
		animator.SetTrigger("PrepareToDivide");
		rotationAngle = _rotationAngle;
	}


	public void StartDivision()
	{

		animator.SetTrigger("Divide");
	}

	public void StartReturnFromDivision()
	{
		animator.SetTrigger("ReturnFromDivision");
	}


	// Animation callbacks
	public void FinishedDivisionPreparation()
	{
		cancer.OnFinishDivisionPreparation();
	}

	public void FinishedDivision()
	{
		cancer.OnFinishDivision();
	}

	public void CellSpawned()
	{
		if (!hypoxicArea.activeSelf)
		{
			float randomAngle = Random.Range(0.0f, 360.0f);
			hypoxicArea.transform.rotation = Quaternion.Euler(0, 0, randomAngle);
			hypoxicArea.SetActive(true);

			//   UIManager.Instance.allCancerCells.Add(this);
		}
	}

	public void CellDied()
	{
		Destroy(hypoxicArea);
		Destroy(gameObject);
	}

	public void RotateForDivision()
	{
		transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationAngle);
	}

	public void RotateForReturn()
	{
		transform.rotation = Quaternion.identity;
	}
}
