using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;


namespace ImmunotherapyGame.Chemokines
{
	public class ChemokinPath : MonoBehaviour
	{
		private PathCreator pathCreator = null;

		[SerializeField]
		private GameObject chemokinePrefab = null;
		[SerializeField]
		private float minSpacing = 1;
		[SerializeField]
		private float maxSpacing = 3;

		[SerializeField]
		private GameObject chemokinePathEnd = null;

		[Header("Debug (Read Only)")]
		[SerializeField]
		private List<Chemokine> chemokines = new List<Chemokine>();

		public void ActivateChemokines()
		{
			gameObject.SetActive(true);
		}

		internal void OnPlayerReachedEndOfPath()
		{

			for (int i = 0; i < chemokines.Count; i++)
			{
				chemokines[i].Remove();
			}
			chemokines = new List<Chemokine>();

		}


		// Start is called before the first frame update
		void Awake()
		{
			pathCreator = GetComponent<PathCreator>();
			Generate();
		}

		private void Generate()
		{
			if (pathCreator != null && chemokinePrefab != null)
			{

				VertexPath path = pathCreator.path;

				float dst = 0;

				while (dst < path.length - minSpacing)
				{
					Vector3 point = path.GetPointAtDistance(dst);
					Vector3 normal = path.GetNormalAtDistance(dst) * Random.Range(-0.5f, 0.5f);
					chemokines.Add( Instantiate(chemokinePrefab, point + normal, Quaternion.identity, gameObject.transform).GetComponent<Chemokine>());
					dst += Mathf.Lerp(maxSpacing, minSpacing, dst / path.length);
				}

				chemokinePathEnd.SetActive(true);
				chemokinePathEnd.transform.position = path.GetPointAtDistance(path.length, EndOfPathInstruction.Stop);
			}



		}
	}
}
