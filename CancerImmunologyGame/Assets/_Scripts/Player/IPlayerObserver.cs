using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.Player
{
	public interface IPlayerObserver
	{
		void OnPlayerDeath();
	}

}