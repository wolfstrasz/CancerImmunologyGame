using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopup : MonoBehaviour
{
    //bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //if (!isActivated)
        //{
        //    PlayerController pc = collider.gameObject.GetComponent<PlayerController>();
        //    if (pc != null)
        //    {
        //        Debug.Log("PopUpCollision");
        //        isActivated = true;
        //        if (gameObject.GetComponent<SpriteRenderer>() != null)
        //            gameObject.GetComponent<SpriteRenderer>().sprite = null;
        //        UIManager.Instance.NextTutorial();
        //        //Destroy(gameObject);
        //    }
        //}
    }
}
