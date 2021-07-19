using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Abilities;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame.Player
{
    [CreateAssetMenu (menuName = "Data/Player Data")]
    public class PlayerData : ScriptableObject
    {
        [SerializeField] [ReadOnly] private Cell currentCell;
        public delegate void OnCurrentCellChanged();
        public OnCurrentCellChanged onCurrentCellChanged;


        public Cell CurrentCell
		{
            get => currentCell;
            set
			{
                currentCell = value;
                if (onCurrentCellChanged != null)
				{
                    onCurrentCellChanged();
				}
			}
		}
	}
}
