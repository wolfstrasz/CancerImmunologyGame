using System;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame
{
	public interface IControllerMovementOverride
	{
		public void ApplyOverride(ref Vector2 momementVector, ref Quaternion rotation, ref Vector3 position);
	}

	public interface IControllerMovementOverridable
	{
		void SubscribeMovementOverride(IControllerMovementOverride controllerOverride);
		void UnsubscribMovementOverride(IControllerMovementOverride controllerOverride);
	}

}
