using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cells.Cancers;

namespace Cells
{
    public class CAFSense : MonoBehaviour
    {

        [SerializeField]
        public List<CancerCell> cancerCells = new List<CancerCell>();


        void Update()
        {
            for (int i = 0; i < cancerCells.Count; ++i)
            {
                if (cancerCells[i] == null)
                {
                    cancerCells.RemoveAt(i);
                    --i;
                }
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            CancerCell cell = collider.gameObject.GetComponent<CancerCell>();
            if (cell != null)
            {
                cancerCells.Add(cell);
            }
        }


        void OnTriggerExit2D(Collider2D collider)
        {
            CancerCell cell = collider.gameObject.GetComponent<CancerCell>();
            if (cell != null)
            {
                if (cancerCells.Contains(cell))
                    cancerCells.Remove(cell);
            }
        }
    }
}
