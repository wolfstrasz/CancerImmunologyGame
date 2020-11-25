using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodCell : MonoBehaviour
{

    public Transform endPos = null;
    public float rotationSpeed = 90.0f;
    // Update is called once per frame
    void Awake()
    {

        transform.Rotate(0, 0, Random.Range(0.0f, 360.0f));
    }
    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

        if (endPos != null)
        {
            if (Vector3.Distance(transform.position, endPos.position) <= 1.0f)
                Destroy(gameObject);
        }
    }
}
