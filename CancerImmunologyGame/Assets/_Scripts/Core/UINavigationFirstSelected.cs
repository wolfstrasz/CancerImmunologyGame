using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.Core
{
	public class UINavigationFirstSelected : MonoBehaviour
	{
		private void OnEnable()
		{
			InputHandlerUtils.RequestUIControl(this);
		}

		private void OnDisable()
		{
			InputHandlerUtils.FreeUIControl(this);
		}
	}
}
