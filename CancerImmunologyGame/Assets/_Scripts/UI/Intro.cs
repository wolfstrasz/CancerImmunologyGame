using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    public int text_index = 0;

    public List<Animator> textAnims;
    public List<GameObject> texts;

    public bool canUpdate = false;
    public GameObject continueSpaceBar;

    public bool skipIntro = false;


    public GameObject MainMenu = null;
    void Awake()
    {
        if (skipIntro)
        {
            gameObject.SetActive(false);
            return;
        }
        StartCoroutine(StartIntro());
    }


    void Update()
    {
        if (!canUpdate) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (text_index >= texts.Count - 1)
            {
                MainMenu.SetActive(true);
                gameObject.SetActive(false);
                canUpdate = false;
                return;
            }

            textAnims[text_index].SetTrigger("FadeOut");
            text_index++;
           
            texts[text_index].SetActive(true);
        }
    }

    IEnumerator StartIntro()
    {
        yield return new WaitForSeconds(2.0f);
        texts[0].SetActive(true);
        canUpdate = true;
        yield return new WaitForSeconds(1.0f);

        continueSpaceBar.SetActive(true);
    }

}
