using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayButtonRecolour : MonoBehaviour
{
    public Image buttonImage = null;
    public Image ImageToGetColourFrom = null;
    public TMP_Text text;

    void Update()
    {
        buttonImage.color = ImageToGetColourFrom.color;
        Color color = Color.white - ImageToGetColourFrom.color;
        color.a = 255;
        text.color = color;
    }
}
