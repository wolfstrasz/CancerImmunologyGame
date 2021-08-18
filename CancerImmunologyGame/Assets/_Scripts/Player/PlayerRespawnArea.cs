using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.Player
{
	public class PlayerRespawnArea : MonoBehaviour
	{
		public Vector3 Position => transform.position;
		private void OnEnable()
		{
			GlobalLevelData.RespawnAreas.Add(this);
		}

		private void OnDisable()
		{
			GlobalLevelData.RespawnAreas.Remove(this);
		}
	}
}