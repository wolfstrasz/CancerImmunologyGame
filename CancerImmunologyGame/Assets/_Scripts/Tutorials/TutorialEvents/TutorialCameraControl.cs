using UnityEngine;

namespace ImmunotherapyGame.Tutorials
{
	public class TutorialCameraControl : TutorialEvent
	{
		[Header("Focus")]
		[SerializeField] private bool shouldFocus = false;
		[SerializeField] private bool instantFocus = false;
		[SerializeField] private GameObject focusTarget = null;

		[Header("Zoom")]
		[SerializeField] private bool shouldZoom = false;
		[SerializeField] private float zoomValue = 6f;
		[SerializeField] private bool instantZoom = false;

		[Header("Blinding")]
		[SerializeField] private CameraBlinding blinding = CameraBlinding.KEEP;
		[SerializeField] private bool gradual = false;
		[SerializeField] [Range(0.01f, 5f)] private float blindTime = 0.01f;

		protected override void OnStartEvent()
		{
			if (shouldFocus)
			{
				GameCamera2D.Instance.SetFocusTarget(focusTarget, instantFocus);
			}

			if (shouldZoom)
			{
				GameCamera2D.Instance.SetOrthographicZoom(zoomValue, instantZoom);
			}

			switch (blinding)
			{
				case CameraBlinding.BLIND:
					if (!gradual) GameCamera2D.Instance.Blind();
					else GameCamera2D.Instance.GradualBlind(blindTime);
					break;
				case CameraBlinding.UNBLIND:
					if (!gradual) GameCamera2D.Instance.Unblind();
					else GameCamera2D.Instance.GradualUnblind(blindTime);
					break;
				default: break;
			}
		}

		protected override bool OnUpdateEvent()
		{
			if (gradual)
			{
				return true;
			}
			return GameCamera2D.Instance.IsCameraFocusedAndFinishedZooming;
		}

		private enum CameraBlinding {KEEP, BLIND, UNBLIND}

	}
}