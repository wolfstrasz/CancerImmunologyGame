using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cells;
using Cancers;
using Player;

namespace Tutorials
{
	public class TCameraFocus : TutorialEvent
	{
		[SerializeField]
		private FocusObjectType closestObjectTypeToFocusOn = FocusObjectType.NONE;

		protected override void OnEndEvent()
		{
			return;
		}

		protected override void OnStartEvent()
		{
			if (closestObjectTypeToFocusOn == FocusObjectType.NONE) return;

			if (closestObjectTypeToFocusOn == FocusObjectType.HEART)
			{
				SmoothCamera.Instance.SetNewFocus(FindObjectOfType<TheHeart>().gameObject);
			}

			if (closestObjectTypeToFocusOn == FocusObjectType.HEART_CINEMATIC)
			{
				SmoothCamera.Instance.StartHeartOutro();
			}

			if (closestObjectTypeToFocusOn == FocusObjectType.INTRO)
			{
				SmoothCamera.Instance.StartIntro();
			}

			if (closestObjectTypeToFocusOn == FocusObjectType.PLAYER)
			{
				SmoothCamera.Instance.SetNewFocus(PlayerController.Instance.gameObject);
			}

			if (closestObjectTypeToFocusOn == FocusObjectType.DENDRITIC_CELL)
			{
				FindClosesDendritic();
			}

			if (closestObjectTypeToFocusOn == FocusObjectType.HELPER_CELL)
			{
				FindClosestHelperCell();
			}

			if (closestObjectTypeToFocusOn == FocusObjectType.CANCER)
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

			SmoothCamera.Instance.SetNewFocus(closestCell.gameObject);
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

			SmoothCamera.Instance.SetNewFocus(closestCell.gameObject);
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

			SmoothCamera.Instance.SetNewFocus(closestCell.gameObject);
		}

		protected override bool OnUpdateEvent()
		{
			if (SmoothCamera.Instance.isCameraFocused)
			{
				return true;
			}
			return false;
		}

		private enum FocusObjectType { NONE, PLAYER, DENDRITIC_CELL, HELPER_CELL, CANCER, KILLER_CELL, INTRO, HEART, HEART_CINEMATIC}
	}
}