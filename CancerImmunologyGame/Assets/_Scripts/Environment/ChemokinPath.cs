using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class ChemokinPath : MonoBehaviour
{
    PathCreator pathCreator = null;
    public GameObject chemokinePrefab = null;
	public float minSpacing = 1;
	public float maxSpacing = 3;
    public float spacing = 3;


	public void ActivateChemokines()
	{
		gameObject.SetActive(true);
	}
	// Start is called before the first frame update
	void Awake()
    {
        pathCreator = GetComponent<PathCreator>();
        Generate();
    }

    void Generate()
    {
        if (pathCreator != null && chemokinePrefab != null)
        {

            VertexPath path = pathCreator.path;

            float dst = 0;

            while (dst < path.length)
            {
                Vector3 point = path.GetPointAtDistance(dst);
				Vector3 normal = path.GetNormalAtDistance(dst) * Random.Range(-0.5f, 0.5f);
                Instantiate(chemokinePrefab, point + normal, Quaternion.identity, gameObject.transform);
                dst += Mathf.Lerp(maxSpacing, minSpacing, dst/path.length);
            }
        }
    }
}
