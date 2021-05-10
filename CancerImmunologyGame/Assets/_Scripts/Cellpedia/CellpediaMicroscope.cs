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

        internal void OnOpen(CellpediaObject objToInitialise)
		{
            //petridishes[0].Reset();
            //petridishes[1].Reset();
            petridishes[dishIndex].SetVisual(objToInitialise);
        }


        internal bool NextPetridish(CellpediaObject cellObject)
        {

            if (petridishes[0].isShifting || petridishes[1].isShifting) return false;

            dishIndex ^= 1;

            petridishes[dishIndex].SetVisual(cellObject);
            Cellpedia.Instance.notepad.SetVisual(cellObject);

            petridishes[0].ShiftLeft();
            petridishes[1].ShiftLeft();
     
            return true;
        }
    }
}
