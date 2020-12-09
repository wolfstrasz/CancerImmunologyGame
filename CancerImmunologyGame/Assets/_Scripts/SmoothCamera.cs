using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(Camera))]
public class SmoothCamera : Singleton<SmoothCamera>
{
	public bool free_roam = true;

    [SerializeField, Range(1.0f, 20.0f)]
    float zoomDistance = 8.0f;

    [SerializeField]
    public GameObject focusTarget;
    private Vector3 focusPosition;
    private Vector3 targetPosition;

    [SerializeField, Range(0.5f, 1.0f)] 
    private float focusCenteringSpeed = 0.943f;

    public bool isCameraFocused = false;

	void Awake()
	{
		focusPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0.0f);
	}

	public void SetNewFocus(GameObject focusObject)
	{
		focusTarget = focusObject;
		isCameraFocused = false;
		free_roam = false;
	}

    // Target's position will change in Update, so camera should move focus here
    void FixedUpdate()
    {
		if (free_roam) return;

        UpdateFocusPoint();
    }

    // Focus objects can change in Update, so camera moves here
    void LateUpdate()
    {
		if (free_roam) return;

		UpdatePosition();
    }

    private void UpdateFocusPoint()
    {
        targetPosition = focusTarget.transform.position;
        float distance = Vector3.Distance(targetPosition, focusPosition);
        if (distance < 0.2f)
        {
            isCameraFocused = true;
        } else
        {
            isCameraFocused = false;
        }

        if (distance > 0.01f)
        {
            // Time.deltaTime switch to unscaledDeltaTime to cover when game is paused
            focusPosition = Vector3.Lerp(targetPosition, focusPosition, (float)System.Math.Pow((1.0f - focusCenteringSpeed), Time.unscaledDeltaTime));
        } else
		{
			focusPosition = targetPosition;
		}
    }

	private void UpdatePosition()
	{
		Vector3 lookDirection = transform.forward;
		Vector3 lookPosition = focusPosition - lookDirection * zoomDistance;
		transform.position = lookPosition;
	}
}



