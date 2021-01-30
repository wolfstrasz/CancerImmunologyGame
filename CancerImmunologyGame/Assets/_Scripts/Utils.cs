using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{

	public static Vector3 GetVectorFromAngle (float angle)
	{
		float angleRad = angle * (Mathf.PI / 180f);
		return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
	}
	
}

public enum CellpediaCells { NONE, TKILLER, THELPER, DENDRITIC, REGULATORY, CANCER}
public enum PlayerUIPanels { MICROSCOPE, PLAYER_INFO, IMMUNOTHERAPY }
