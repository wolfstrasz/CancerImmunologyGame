using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.CellpediaSystem
{
    public class CellpediaMicroscope : MonoBehaviour
    {
        [SerializeField]
        private List<Petridish> petridishes = new List<Petridish>();
        [SerializeField]
        private int dishIndex = 0;

        internal void Initialise()
		{
            petridishes[0].Reset();
            petridishes[1].Reset();
        }

        internal void OnClose()
		{
            petridishes[0].SkipAnimation();
            petridishes[1].SkipAnimation();
        }

        internal void OnOpen(CellpediaCellDescription descriptionToInitialise)
		{
            //petridishes[0].Reset();
            //petridishes[1].Reset();
            petridishes[dishIndex].SetVisual(descriptionToInitialise);
        }


        internal bool NextPetridish(CellpediaCellDescription cellDescription)
        {

            if (petridishes[0].isShifting || petridishes[1].isShifting) return false;

            dishIndex ^= 1;

            petridishes[dishIndex].SetVisual(cellDescription);
            Cellpedia.Instance.notepad.SetVisual(cellDescription);

            petridishes[0].ShiftLeft();
            petridishes[1].ShiftLeft();
     
            return true;
        }
    }
}
