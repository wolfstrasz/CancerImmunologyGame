﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSingleton : Singleton<TestSingleton>
{
    // Start is called before the first frame update
    void Start()
    {
		Debug.Log("HELO");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
