using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class ChemokinPath : MonoBehaviour
{
    PathCreator pathCreator;

    public GameObject chemokinePrefab;
    public float spacing = 3;
    const float minSpacing = .1f;

    // Start is called before the first frame update
    void Awake()
    {
        pathCreator = GetComponent<PathCreator>();

        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Generate()
    {
        if (pathCreator != null && chemokinePrefab != null)
        {

            VertexPath path = pathCreator.path;

            spacing = Mathf.Max(minSpacing, spacing);
            float dst = 0;

            while (dst < path.length)
            {
                Vector3 point = path.GetPointAtDistance(dst);
                Instantiate(chemokinePrefab, point, Quaternion.identity, gameObject.transform);
                dst += spacing;
            }
        }
    }
}
