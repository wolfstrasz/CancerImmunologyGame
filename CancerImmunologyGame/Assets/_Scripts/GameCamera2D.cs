using UnityEngine;
using System.Collections;
using Player;
using System;

public class GameCamera2D : Singleton<GameCamera2D>
{

	[Header("Links")]
	[SerializeField]
	private new Camera camera = null;

	[Header("Attributes")]
	[SerializeField]
	private GameCameraControlState state = GameCameraControlState.IDLE;

	[SerializeField, Range(0.5f, 1.0f)]
	private float focusSpeed = 0.943f;
	private const float focusAcceptedTreshold = 0.2f;
	private const float focusSkipTreshold = 0.01f;

	[SerializeField, Range(0.5f, 1.0f)]
	private float zoomSpeed = 0.943f;
	private const float zoomAcceptedTreshold = 0.2f;
	private const float zoomSkipTreshold = 0.01f;

	[SerializeField]
	private float freeroamSpeedScale = 4.0f;

	[Header("Debug (Read only)")]
	[SerializeField]
	private GameObject focusTarget = null;
	[SerializeField]
	private Vector3 focusPosition = Vector3.zero;
	[SerializeField]
	private bool isCameraFocused = false;
	[SerializeField]
	private float orthographicZoom = 6.0f;
	[SerializeField]
	private bool isCameraZoomed = false;

	public bool IsCameraZoomed => isCameraZoomed;
	public bool IsCameraFocused => isCameraFocused;


	void Update()
	{
		if (state == GameCameraControlState.FREEROAM)
		{
			if (Input.GetKey(KeyCode.W))
			{
				//transform.position += new Vector3(0f, 1f, 0f) * freeroamSpeedScale;
				focusPosition += new Vector3(0f, 1f, 0f) * freeroamSpeedScale;
			}
			else if (Input.GetKey(KeyCode.S))
			{
				//transform.position += new Vector3(0f, -1f, 0f) * freeroamSpeedScale;
				focusPosition += new Vector3(0f, -1f, 0f) * freeroamSpeedScale;
			}

			if (Input.GetKey(KeyCode.D))
			{
				//transform.position += new Vector3(1f, 0f, 0f) * freeroamSpeedScale;
				focusPosition += new Vector3(1f, 0f, 0f) * freeroamSpeedScale;
			}
			else if (Input.GetKey(KeyCode.A))
			{
				//transform.position += new Vector3(-1f, 0f, 0f) * freeroamSpeedScale;
				focusPosition += new Vector3(-1f, 0f, 0f) * freeroamSpeedScale;
			}
			if (Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.Plus))
			{
				orthographicZoom -= 1.0f;
			}
			else if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.Minus))
			{
				orthographicZoom += 1.0f;
			}
		}
	}

	// Target's position will change in Update, so camera should move focus here
	void FixedUpdate()
	{
		if (state == GameCameraControlState.FOLLOW)
		{
			UpdateFocusPoint();
#if BLOODFLOW_ROTATION
			transform.rotation = focusTarget.transform.rotation;
#else
#endif
		} 

		UpdateOrthographicZoom();

	}

	// Focus objects can change in Update, so camera moves here
	void LateUpdate()
	{
		UpdatePositionByFocusPoint();
	}

	private void UpdateFocusPoint()
	{
		Vector3 targetPosition = focusTarget.transform.position;
		float distance = Vector3.Distance(targetPosition, focusPosition);

		isCameraFocused = distance < focusAcceptedTreshold ? true : false;

		focusPosition = distance <= focusSkipTreshold ? targetPosition 
			: Vector3.Lerp(targetPosition, focusPosition, (float)System.Math.Pow((1.0f - focusSpeed), Time.deltaTime));
	}

	private void UpdateOrthographicZoom()
	{
		float gap = Mathf.Abs( orthographicZoom - camera.orthographicSize) ;
		isCameraZoomed = gap < zoomAcceptedTreshold ? true : false;
		camera.orthographicSize = gap <= zoomSkipTreshold ? orthographicZoom
			: Mathf.Lerp(orthographicZoom, camera.orthographicSize, (float) System.Math.Pow((1.0f - zoomSpeed), Time.deltaTime));
	}

	private void UpdatePositionByFocusPoint()
	{
		Vector3 lookDirection = transform.forward;
		Vector3 lookPosition = focusPosition - lookDirection * camera.orthographicSize;
		transform.position = lookPosition;
	}

	public void SetFocusTarget(GameObject objectToFocusOn, bool shouldInstantlyFocus = false)
	{
		focusTarget = objectToFocusOn;
		isCameraFocused = false;

		if (shouldInstantlyFocus)
		{
			focusPosition = focusTarget.transform.position;
			Vector3 lookDirection = transform.forward;
			Vector3 lookPosition = focusPosition - lookDirection;
			transform.position = lookPosition;
		}
	}

	public void SetOrthographicZoom(float orthographicZoom, bool shouldInstantlyZoom = false)
	{
		this.orthographicZoom = orthographicZoom;
		isCameraZoomed = false;

		if (shouldInstantlyZoom)
		{
			camera.orthographicSize = orthographicZoom;
		}
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

	[System.Serializable]
	public enum GameCameraControlState { IDLE, FOLLOW, FREEROAM}
}



