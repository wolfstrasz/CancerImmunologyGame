﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cellpedia Item")]
public class CellDescription : ScriptableObject
{
	public string cellname;
	[TextArea(5,10)] 
	public string description;
	public string animatorTrigger;
	public Sprite sprite;
	public float scale;
}