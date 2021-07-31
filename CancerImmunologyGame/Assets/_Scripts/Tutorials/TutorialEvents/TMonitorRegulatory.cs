﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.Tutorials
{
	public class TMonitorRegulatory : TutorialEvent
	{
		[Header("Debug")]
		[SerializeField]
		private List<RegulatoryCell> regCells = new List<RegulatoryCell>();

		protected override void OnEndEvent()
		{
		}

		protected override void OnStartEvent()
		{
			regCells.AddRange(FindObjectsOfType<RegulatoryCell>());
		}

		protected override bool OnUpdateEvent()
		{
			foreach (RegulatoryCell cell in regCells)
			{
				if (GameCamera2D.Instance.IsInCameraViewBounds(cell.gameObject.transform.position, true))
				{
					return true;
				}
			}

			return false;
		}

	}
}