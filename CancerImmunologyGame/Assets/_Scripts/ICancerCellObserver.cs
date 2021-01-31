using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cancers;

public interface ICancerCellObserver
{
	void NotifyOfDeath(CancerCell cc);
}
