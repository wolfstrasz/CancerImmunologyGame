using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICellController
{

	void OnEnemiesInRange();
	void OnEnemiesOutOfRange();
	void OnCellDeath();
}
