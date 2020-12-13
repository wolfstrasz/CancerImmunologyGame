using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancerCell : MonoBehaviour
{
	internal Cancer cancer = null;

	private bool inDivision = false;
	internal bool isDying = false;

	public static bool first_hit = false; // should switch to a tutorial pop-up event
	public static float wait_before_destroy = 3.0f; // Must remove hard-coding value
	[SerializeField]
    private CancerCellBody body = null;
	
	[SerializeField]
	public CircleCollider2D divisionBodyBlocker = null;

	[SerializeField]
    private GameObject hypoxicArea = null;
    [SerializeField]
    private Animator animator = null;
	[SerializeField]
	private SpriteRenderer render = null;

    public float health = 100;

	// Division info
	private float rotationAngle = 0.0f;

	internal void SetSortOrder (int sortOrder)
	{
		render.sortingOrder = sortOrder;
	}

    void Awake()
    {
		hypoxicArea.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f));
    }

    public bool HitCell()
    {
        health -= 20f;
        if (health <= 0)
        {
			isDying = true;

			cancer.RemoveCell(this);
			body.gameObject.SetActive(false);
			animator.SetTrigger("Apoptosis");
			return true;
        }
		return false;
    }

	public bool CellInDivision()
	{
		return inDivision;
	}


	// Control calls
	public void StartPrepareDivision(float _rotationAngle)
	{
		inDivision = true;
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
		inDivision = false;
		isDying = false;
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
