using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cells;
using Cancers;
using ImmunotherapyGame.Player;

namespace ImmunotherapyGame.Tutorials
{
	public class TCameraControl : TutorialEvent
	{
		[Header("Focus")]
		[SerializeField]
		private bool shouldFocus = false;
		[SerializeField]
		private FocusObjectType closestObjectType = FocusObjectType.NONE;
		[SerializeField]
		private bool instantFocus = false;

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
				switch (closestObjectType)
				{
					case FocusObjectType.PLAYER:
						GameCamera2D.Instance.SetFocusTarget(PlayerController.Instance.gameObject, instantFocus);
						break;
					case FocusObjectType.DENDRITIC_CELL:
						GameCamera2D.Instance.SetFocusTarget(Utils.FindClosestGameObjectOfType<DendriticCell> (location), instantFocus);
						break;
					case FocusObjectType.HEART:
						GameCamera2D.Instance.SetFocusTarget(Utils.FindClosestGameObjectOfType<TheHeart>(location), instantFocus);
						break;
					case FocusObjectType.HELPER_CELL:
						GameCamera2D.Instance.SetFocusTarget(Utils.FindClosestGameObjectOfType<HelperTCell>(location), instantFocus);
						break;
					case FocusObjectType.CANCER:
						GameCamera2D.Instance.SetFocusTarget(Utils.FindClosestGameObjectOfType<Cancer>(location), instantFocus);
						break;
					default: break;
				}
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
		private enum FocusObjectType { NONE, PLAYER, DENDRITIC_CELL, HELPER_CELL, CANCER, HEART }

	}
}