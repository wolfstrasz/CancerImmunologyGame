using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
	public interface IPlayerObserver
	{
		void OnPlayerDeath();
	}

}