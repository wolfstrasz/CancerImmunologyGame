using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cells;
namespace ImmunotherapyGame.Tutorials
{
	public class TSpawnObject : TutorialEvent
	{
		[SerializeField]
		private GameObject prefabToSpawn = null;
		[SerializeField]
		private Vector3 spawnPosition = Vector3.zero;

		protected override void OnEndEvent()
		{
		}

		protected override void OnStartEvent()
		{
			GameObject.Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
		}

		protected override bool OnUpdateEvent()
		{
			return true;
		}

		
	}
}