using UnityEngine;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame
{
	public class GameCamera2D : Singleton<GameCamera2D>
	{

		[Header("Links")]
		[SerializeField] private new Camera camera = null;
		[SerializeField] private GameObject blind = null;
		[SerializeField] private SpriteRenderer blindImage = null;

		[Header("Attributes")]
		[SerializeField] private GameCameraControlState state = GameCameraControlState.IDLE;

		[SerializeField, Range(0.5f, 1.0f)] private float focusExponential = 0.943f;
		[SerializeField] private const float focusAcceptedTreshold = 0.2f;
		[SerializeField] private const float focusSkipTreshold = 0.01f;

		[SerializeField, Range(0.5f, 1.0f)] private float zoomExponential = 0.943f;
		[SerializeField] private const float zoomAcceptedTreshold = 0.2f;
		[SerializeField] private const float zoomSkipTreshold = 0.01f;

		[Header("Camera Movement Prediction")]
		[SerializeField] private float maxForwardingDistance = 3.5f;
		[SerializeField] private float minDistanceForPrediction = 0.001f;
		[SerializeField] private float minPredictionSkipTreshhold = 0.01f;
		[SerializeField, Range(0.5f, 1.0f)] private float predictionShiftExponential = 0.943f;
		[SerializeField, Range(0.5f, 1.0f)] private float predictionReturnExponential = 0.943f;
		private Vector3 prevPosition = Vector3.zero;
		private Vector3 shiftOffset = Vector3.zero;


		[Header("Debug (Read only)")]
		[SerializeField] [ReadOnly] private GameObject focusTarget = null;
		[SerializeField] [ReadOnly] private Vector3 focusPosition = Vector3.zero;
		[SerializeField] [ReadOnly] private bool isFocused = false;
		[SerializeField] [ReadOnly] private float orthographicZoom = 6.0f;
		[SerializeField] [ReadOnly] private bool isZooming = false;
		[SerializeField] [ReadOnly] private bool isBlinding = false;
		[SerializeField] [ReadOnly] private bool isUnblinding = false;
		[SerializeField] [ReadOnly] private float blindTimeLeft = 0f;
		[SerializeField] [ReadOnly] private float blindTime = 0f;

		public bool IsCameraFocusedAndFinishedZooming => (isFocused && !isZooming);


		// Target's position will change in Update, so camera should move focus here
		void FixedUpdate()
		{

			if (state == GameCameraControlState.FOLLOW && focusTarget != null)
			{
				UpdateFocusPoint();
#if BLOODFLOW_ROTATION
				transform.rotation = focusTarget.transform.rotation;
#else
#endif
			} 
			else
			{
				shiftOffset = Vector3.zero;
			}

			UpdateOrthographicZoom();
			UpdateGradualBlind();
		}

		// Focus objects can change in Update, so camera moves here
		void LateUpdate()
		{
			UpdatePositionByFocusPoint();
			//debugDot.transform.position = shiftOffset + transform.position + new Vector3(0.0f, 0.0f, 1.0f);

		}

		private void UpdateFocusPoint()
		{
			// Focusing
			Vector3 targetPosition = focusTarget.transform.position;
			float distance = Vector3.Distance(targetPosition, focusPosition);

			isFocused = distance < focusAcceptedTreshold ? true : false;

			focusPosition = distance <= focusSkipTreshold ? targetPosition
				: Vector3.Lerp(targetPosition, focusPosition, (float)System.Math.Pow((1.0f - focusExponential), Time.deltaTime));

			// Movement Prediction
			Vector3 targetMovementDirection = targetPosition - prevPosition;
			float movementDistance = targetMovementDirection.magnitude;

			if (movementDistance > minDistanceForPrediction)
			{
				Vector3 shiftDirection = targetMovementDirection.normalized;
				Vector3 maxShiftVector =  shiftDirection * maxForwardingDistance;
				shiftOffset = Vector3.Lerp(shiftOffset, maxShiftVector, 1.0f - (float)System.Math.Pow((1.0f - predictionShiftExponential), Time.deltaTime));
				prevPosition = focusTarget.transform.position;

			}
			else
			{
				shiftOffset = Vector3.Lerp(shiftOffset, Vector3.zero,  1.0f - (float) System.Math.Pow((1.0f - predictionReturnExponential), Time.deltaTime));
				if (shiftOffset.magnitude < minPredictionSkipTreshhold)
				{
					shiftOffset = Vector3.zero;
				}
			}
}

		private void UpdateOrthographicZoom()
		{
			float gap = Mathf.Abs(orthographicZoom - camera.orthographicSize);
			isZooming = gap < zoomAcceptedTreshold ? false : true;
			camera.orthographicSize = gap <= zoomSkipTreshold ? orthographicZoom
				: Mathf.Lerp(orthographicZoom, camera.orthographicSize, (float)System.Math.Pow((1.0f - zoomExponential), Time.deltaTime));
		}

		private void UpdatePositionByFocusPoint()
		{
			Vector3 lookDirection = transform.forward;
			Vector3 lookPosition = focusPosition - lookDirection * camera.orthographicSize;
			transform.position = lookPosition + shiftOffset;
		}

		public void SetFocusTarget(GameObject objectToFocusOn, bool shouldInstantlyFocus = false)
		{
			focusTarget = objectToFocusOn;
			isFocused = false;
			state = GameCameraControlState.FOLLOW;
			prevPosition = focusTarget.transform.position;
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
			isZooming = true;

			if (shouldInstantlyZoom)
			{
				camera.orthographicSize = orthographicZoom;
			}
		}

		public void Blind()
		{
			blind.SetActive(true);
			isBlinding = false;
			isUnblinding = false;
			blindTimeLeft = 0f;
			blindImage.color = new Color(0,0,0,1);

		}

		public void Unblind()
		{
			blind.SetActive(false);
			isBlinding = false;
			isUnblinding = false;
			blindTimeLeft = 0f;
		}

		public void GradualBlind(float t)
		{
			blind.SetActive(true);
			blindTimeLeft = t;
			blindTime = t;
			isBlinding = true;
			isUnblinding = false;
			blindImage.color = new Color(0, 0, 0, 0);

		}

		public void GradualUnblind(float t)
		{
			blindTimeLeft = t;
			blindTime = t;
			isBlinding = false;
			isUnblinding = true;
		}

		private void UpdateGradualBlind()
		{
		
			if (isBlinding)
			{
				blindTimeLeft -= Time.fixedDeltaTime;

				if (blindTimeLeft <= 0)
				{
					blindTimeLeft = 0f;
					isBlinding = false;
				}

				Color c = new Color(0, 0, 0, ((blindTime - blindTimeLeft) / blindTime));
				blindImage.color = c;

			}
			else if (isUnblinding)
			{
				blindTimeLeft -= Time.fixedDeltaTime;

				if (blindTimeLeft <= 0)
				{
					blindTimeLeft = 0f;
					isBlinding = false;
					blind.SetActive(false);

				}

				Color c = new Color(0, 0, 0,  (blindTimeLeft / blindTime));
				blindImage.color = c;

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

		public enum GameCameraControlState { IDLE, FOLLOW, FREEROAM }

	}



}