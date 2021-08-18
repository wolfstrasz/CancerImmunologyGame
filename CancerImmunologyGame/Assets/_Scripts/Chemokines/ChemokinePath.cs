using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;


namespace ImmunotherapyGame.Chemokines
{
	public class ChemokinePath : MonoBehaviour
	{
		private PathCreator pathCreator = null;

		[SerializeField] private GameObject chemokinePrefab = null;
		[SerializeField] private float minSpacing = 1;
		[SerializeField] private float maxSpacing = 3;
		[SerializeField] private GameObject chemokinePathEnd = null;

		[Header("Debug (Read Only)")]
		[SerializeField] [ReadOnly] private List<Chemokine> chemokines = new List<Chemokine>();

		private void Awake()
		{
			pathCreator = GetComponent<PathCreator>();
		}

		private void OnEnable()
		{
			Generate();
		}

		private void OnDisable()
		{
			chemokines.Clear();
		}

		private void Generate()
		{
			if (pathCreator != null && chemokinePrefab != null)
			{
				float dst = 0;
				VertexPath path = pathCreator.path;
				while (dst < path.length - minSpacing)
				{
					Vector3 point = path.GetPointAtDistance(dst);
					Vector3 normal = path.GetNormalAtDistance(dst) * Random.Range(-0.5f, 0.5f);
					chemokines.Add(Instantiate(chemokinePrefab, point + normal, Quaternion.identity, gameObject.transform).GetComponent<Chemokine>());
					dst += Mathf.Lerp(maxSpacing, minSpacing, dst / path.length);
				}

				// Update path end
				chemokinePathEnd.SetActive(true);
				chemokinePathEnd.transform.position = path.GetPointAtDistance(path.length, EndOfPathInstruction.Stop);
			}
		}

		internal void HideChemokines()
		{
			for (int i = 0; i < chemokines.Count; ++i)
			{
				chemokines[i].Hide();
			}
		}
	}
}
