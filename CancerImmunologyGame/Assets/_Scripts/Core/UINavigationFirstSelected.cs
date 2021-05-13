using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.UI
{
	public class UINavigationFirstSelected : MonoBehaviour
	{
		private void OnEnable()
		{
			UIUtils.RequestUINavigationControl(this);
		}

		private void OnDisable()
		{
			UIUtils.FreeUINavigationControl(this);
		}
	}
}
