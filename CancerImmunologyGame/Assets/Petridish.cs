using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Petridish : MonoBehaviour
{
	private static Petridish nextPetridish = null;

	[Header("Links")]
	[SerializeField]
	private RectTransform trans = null;
	[SerializeField]
	private RectTransform parentTransform = null;

	[Header("Attributes")]
	[SerializeField]
	private bool isLeaving = false;
	[SerializeField]
	private static float timetopass = 2.0f;

	[Header("Debugging")]
	[SerializeField]
	private float xStartCoord = 0.0f;
	[SerializeField]
	private static float xShiftCoord = 0.0f;
	[SerializeField]
	private bool isShifting = false;
	[SerializeField]
	private float timepassed = 0.0f;

	// Start is called before the first frame update
	void Start()
    {
		xShiftCoord = parentTransform.rect.width;
		if (isLeaving)
		{
			xStartCoord = 0.0f;
		} else
		{
			xStartCoord = xShiftCoord;
			nextPetridish = this;
		}
		trans.localPosition = new Vector2(xStartCoord, 0.0f);
    }


    // Update is called once per frame
    void Update()
    {
		if (!isShifting) return;

        if (isShifting && timepassed < timetopass)
		{
			timepassed += Time.deltaTime;
			if (timepassed > timetopass)
				timepassed = timetopass;


			trans.localPosition = new Vector2(xStartCoord - (timepassed / timetopass) * xShiftCoord, 0.0f);
			return;
		}

		Reset();
	}

	public void Reset()
	{
		isShifting = false;
		timepassed = 0.0f;
	
		if (isLeaving)
		{
			isLeaving = false;
			trans.localPosition = new Vector2(xShiftCoord, 0.0f);
			nextPetridish = this;
		}
		else
		{
			isLeaving = true;
		}
	}

	public void ShiftLeft()
	{
		timepassed = 0.0f;
		isShifting = true;
	}

}
