using UnityEngine;
using System.Collections;
using Player;
using System;

public class SmoothCamera : Singleton<SmoothCamera>
{
	[SerializeField]
	private new Camera camera = null;
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
	[SerializeField]
	private Vector3 startPosition = new Vector3(0.0f, 0.0f, 0.0f);

	[SerializeField]
	private Animator cameraAnimator = null;

	[SerializeField]
	private bool DebugForceSkip = false;
	[SerializeField]
	private bool inHeartOutro = false;
	public bool InHeartOutro { get => inHeartOutro; }

	public void Reset()
	{
		focusPosition = new Vector3(startPosition.x, startPosition.y, 0.0f);
	}

	//void Awake()
	//{
	//	gameObject.transform.position = startPosition;
	//	focusPosition = new Vector3(startPosition.x, startPosition.y, 0.0f);
	//}

	void Awake()
	{
		if (DebugForceSkip)
		{
			cameraAnimator.SetTrigger("Idle");
			free_roam = false;
		}
	}

	public void SetNewFocus(GameObject focusObject, bool instant = false)
	{
		focusTarget = focusObject;
		isCameraFocused = false;
		free_roam = false;

		if (instant)
		{
			focusPosition = focusTarget.transform.position;
			Vector3 lookDirection = transform.forward;
			Vector3 lookPosition = focusPosition - lookDirection * zoomDistance;
			transform.position = lookPosition;
		}
	}


	// Target's position will change in Update, so camera should move focus here
	void FixedUpdate()
	{
		if (!free_roam)
		{
			UpdateFocusPoint();
#if BLOODFLOW_ROTATION
			transform.rotation = focusTarget.transform.rotation;
#else
#endif

		}
	}

	// Focus objects can change in Update, so camera moves here
	void LateUpdate()
	{
		if (!free_roam)
			UpdatePosition();
	}

	private void UpdateFocusPoint()
	{
		targetPosition = focusTarget.transform.position;
		float distance = Vector3.Distance(targetPosition, focusPosition);
		if (distance < 0.2f)
		{
			isCameraFocused = true;
		}
		else
		{
			isCameraFocused = false;
		}

		if (distance > 0.01f)
		{
			// Time.deltaTime switch to unscaledDeltaTime to cover when game is paused
			focusPosition = Vector3.Lerp(targetPosition, focusPosition, (float)System.Math.Pow((1.0f - focusCenteringSpeed), Time.deltaTime));
		}
		else
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


	public bool IsInCameraViewBounds(Vector3 position, bool useHalfBoundsForX = false)
	{
		Vector2 lowerBound;
		Vector3 upperBound;

		if (useHalfBoundsForX)
		{
			lowerBound = camera.ViewportToWorldPoint(new Vector3(0.25f, 0, 0));
			upperBound = camera.ViewportToWorldPoint(new Vector3(0.75f, 1, 0));
		}
		else
		{
			lowerBound = camera.ViewportToWorldPoint(new Vector3(0, 0, 0));
			upperBound = camera.ViewportToWorldPoint(new Vector3(1, 1, 0));
		}

		if (position.x > lowerBound.x
			&& position.x < upperBound.x
			&& position.y > lowerBound.y
			&& position.y < upperBound.y)
			return true;

		return false;
	}


	// Tutorial call
	public void StartIntro()
	{
		cameraAnimator.SetTrigger("Intro");
	}

	// Animation callbacks
	public void StartHeartOutro()
	{
		cameraAnimator.SetTrigger("HeartOutro");
		inHeartOutro = true;
	}

	public void HeartOutroEnd()
	{
		// Tell TUTOTIRIAL THAT IT FINISHED
		cameraAnimator.SetTrigger("Idle");
		transform.position = new Vector3(transform.position.x, transform.position.y, -6.0f);
		inHeartOutro = false;
	}

	public void ReturnToIdle()
	{
		camera.orthographicSize = 6.0f;
		transform.position = new Vector3(transform.position.x, transform.position.y, -6.0f);
		cameraAnimator.SetTrigger("Idle");
		free_roam = false;
		focusPosition = focusTarget.transform.position;
	}

}



