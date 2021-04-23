using UnityEngine;
using UnityEngine.UI;

namespace ImmunotherapyGame.CellpediaSystem
{
	public class CellpediaPopupEffect : MonoBehaviour
	{
		[Header("Attributes")]
		[SerializeField]
		private Image image = null;
		[SerializeField]
		private float timeToReachTarget = 5.0f;

		[Header("Debug (Read Only)")]
		[SerializeField]
		private Vector3 targetPosition = Vector3.zero;
		[SerializeField]
		private Vector3 startScale = Vector3.zero;
		[SerializeField]
		private Vector3 moveVector = Vector3.zero;


		internal void SetData(Vector3 _targetPosition, CellDescription cd)
		{
			targetPosition = _targetPosition;
			moveVector = targetPosition - transform.position;

			image.sprite = cd.sprite;
			transform.localScale = cd.scaleVector;
			startScale = cd.scaleVector;

		}

		void Update()
		{

			float timeFactor = Time.deltaTime / timeToReachTarget;
			Vector3 factoredMove =  moveVector * timeFactor;

			if (factoredMove.sqrMagnitude >= (targetPosition - transform.position).sqrMagnitude)
			{
				// Reaches target
				Destroy(gameObject);
				return;
			}

			transform.localScale -= startScale * timeFactor;
			transform.position += factoredMove;

		}
	}
}