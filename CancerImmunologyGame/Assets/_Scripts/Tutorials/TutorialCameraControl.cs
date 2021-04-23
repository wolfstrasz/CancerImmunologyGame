using UnityEngine;
using ImmunotherapyGame.Cancers;
using ImmunotherapyGame.Player;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.Tutorials
{
	public class TutorialCameraControl : TutorialEvent
	{
		[Header("Focus")]
		[SerializeField]
		private bool shouldFocus = false;
		[SerializeField]
		private bool instantFocus = false;
		[SerializeField]
		private GameObject focusTarget = null;

		[Header("Zoom")]
		[SerializeField]
		private bool shouldZoom = false;
		[SerializeField]
		private float zoomValue = 6f;
		[SerializeField]
		private bool instantZoom = false;

		[Header("Blinding")]
		[SerializeField]
		private CameraBlinding blinding = CameraBlinding.KEEP;

		protected override void OnEndEvent()
		{
			return;
		}

		protected override void OnStartEvent()
		{
			if (shouldFocus)
			{
				Vector3 location = PlayerController.Instance.gameObject.transform.position;
				GameCamera2D.Instance.SetFocusTarget(focusTarget, instantFocus);
			}

			if (shouldZoom)
			{
				GameCamera2D.Instance.SetOrthographicZoom(zoomValue, instantZoom);
			}

			switch (blinding)
			{
				case CameraBlinding.BLIND: GameCamera2D.Instance.Blind(); break;
				case CameraBlinding.UNBLIND: GameCamera2D.Instance.Unblind(); break;
				default: break;
			}
		}

		protected override bool OnUpdateEvent()
		{
			if (GameCamera2D.Instance.IsCameraFocusedAndFinishedZooming)
			{
				return true;
			}
			return false;
		}

		private enum CameraBlinding {KEEP, BLIND, UNBLIND}

	}
}