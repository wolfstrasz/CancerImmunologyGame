using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cells;
using Cancers;
using Player;

namespace Tutorials
{
	public class TCameraControl : TutorialEvent
	{
		[SerializeField]
		ControlFocusAttributes focusControl;

		[SerializeField]
		ControlZoomAttributes zoomControl;

		[SerializeField]
		ControlBlindAttributes blindControl;


		protected override void OnEndEvent()
		{
			return;
		}

		protected override void OnStartEvent()
		{
			if (focusControl.shouldFocus)
			{
				SelectFocusTarget();
			}
			if (zoomControl.shouldZoom)
			{
				GameCamera2D.Instance.SetOrthographicZoom(zoomControl.zoomValue, zoomControl.instant);
			}
			if (blindControl.shouldBlind)
			{
				GameCamera2D.Instance.Blind();
			}
			if (blindControl.shouldUnBlind)
			{
				GameCamera2D.Instance.Unblind();
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


		private void SelectFocusTarget()
		{

			if (focusControl.objectType == FocusObjectType.NONE) return;

			if (focusControl.objectType == FocusObjectType.HEART)
			{
				GameCamera2D.Instance.SetFocusTarget(FindObjectOfType<TheHeart>().gameObject, zoomControl.instant);
			}

			if (focusControl.objectType == FocusObjectType.PLAYER)
			{
				GameCamera2D.Instance.SetFocusTarget(PlayerController.Instance.gameObject, zoomControl.instant);
			}

			if (focusControl.objectType == FocusObjectType.DENDRITIC_CELL)
			{
				FindClosesDendritic();
			}

			if (focusControl.objectType == FocusObjectType.HELPER_CELL)
			{
				FindClosestHelperCell();
			}

			if (focusControl.objectType == FocusObjectType.CANCER)
			{
				FindClosestCancer();
			}
		}

		// Should be templated
		private void FindClosesDendritic()
		{
			Vector3 playerPos = PlayerController.Instance.gameObject.transform.position;
			DendriticCell[] dendriticCells = FindObjectsOfType<DendriticCell>();

			float closestDist = Vector3.SqrMagnitude(dendriticCells[0].gameObject.transform.position - playerPos);
			DendriticCell closestCell = dendriticCells[0];

			foreach (DendriticCell cell in dendriticCells)
			{
				float dist = Vector3.SqrMagnitude(cell.gameObject.transform.position - playerPos);
				if (dist < closestDist)
				{
					closestDist = dist;
					closestCell = cell;
				}
			}

			GameCamera2D.Instance.SetFocusTarget(closestCell.gameObject, zoomControl.instant);
		}

		private void FindClosestCancer()
		{
			Vector3 playerPos = PlayerController.Instance.gameObject.transform.position;
			Cancer[] cancers = FindObjectsOfType<Cancer>();

			float closestDist = Vector3.SqrMagnitude(cancers[0].gameObject.transform.position - playerPos);
			Cancer closestCell = cancers[0];

			foreach (Cancer cell in cancers)
			{
				float dist = Vector3.SqrMagnitude(cell.gameObject.transform.position - playerPos);
				if (dist < closestDist)
				{
					closestDist = dist;
					closestCell = cell;
				}
			}

			GameCamera2D.Instance.SetFocusTarget(closestCell.gameObject, zoomControl.instant);
		}

		private void FindClosestHelperCell()
		{
			Vector3 playerPos = PlayerController.Instance.gameObject.transform.position;
			HelperTCell[] helperCells = GameObject.FindObjectsOfType<HelperTCell>();
			float closestDist = Vector3.SqrMagnitude(helperCells[0].gameObject.transform.position - playerPos);
			HelperTCell closestCell = helperCells[0];

			foreach (HelperTCell cell in helperCells)
			{
				float dist = Vector3.SqrMagnitude(cell.gameObject.transform.position - playerPos);
				if (dist < closestDist)
				{
					closestDist = dist;
					closestCell = cell;
				}
			}

			GameCamera2D.Instance.SetFocusTarget(closestCell.gameObject, zoomControl.instant);
		}

		[System.Serializable]
		private struct ControlFocusAttributes
		{
			public bool shouldFocus;
			public FocusObjectType objectType;
			public bool instant;
		}

		[System.Serializable]
		private struct ControlZoomAttributes
		{
			public bool shouldZoom;
			public float zoomValue;
			public bool instant;
		}

		[System.Serializable]
		private struct ControlBlindAttributes
		{
			public bool shouldBlind;
			public bool shouldUnBlind;
		}

		private enum FocusObjectType { NONE, PLAYER, DENDRITIC_CELL, HELPER_CELL, CANCER, KILLER_CELL, HEART}
	}
}