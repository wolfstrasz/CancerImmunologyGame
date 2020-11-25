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


    public void Activate()
    {
        if (doAction == 0) return;
       // if (doAction == 1) UIManager.Instance.AllowTimeToMove();
        if (doAction == 2) MoveDendriticCell();
        if (doAction == 10) UIManager.Instance.Cinema(4.0f);
        //if (doAction == 11) UIManager.Instance.Cinema(2.0f);
        if (doAction == 12) UIManager.Instance.FocusOnDendriticCell();
        if (doAction == -5) UIManager.Instance.RemoveTutorialText();
        if (doAction == 14) UIManager.Instance.FocusOnPlayer();
        if (doAction == -6) UIManager.Instance.SpawnTutorialPopUpAtDendriticCell();
        if (doAction == 33) UIManager.Instance.SpawnTrailOfChemokines();
    }

    public void MoveDendriticCell()
    {
        Debug.Log("MOVE DENDRITIC CELL");
    }


}
