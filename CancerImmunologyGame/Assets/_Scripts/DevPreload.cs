﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevPreload : MonoBehaviour
{
    void Awake()
    {
        GameObject check = GameObject.Find("__app");
        if (check == null)
        {
            Debug.Log("Loading _preload scene");
            UnityEngine.SceneManagement.SceneManager.LoadScene("_preload"); 
        }
    }
}
