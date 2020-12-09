using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialObject", menuName = "Tutorials")]
public class TutorialObject : ScriptableObject
{
    public string txt = "Display text";
    public bool shouldDisplayText = true;
    public int doAction = 0;
    public bool skipButton = true;


}
