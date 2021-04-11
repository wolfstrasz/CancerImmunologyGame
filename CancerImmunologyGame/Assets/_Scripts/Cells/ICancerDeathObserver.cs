using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Cells.Cancers 
{ 
	public interface ICancerDeathObserver
	{
		void OnCancerDeath();
	}

	public interface ICancerDivisionObserver
	{
		void OnDivisionStart(Cancer dividingCancer);
		void OnDivisionEnd(Cancer dividingCancer);
	}

	public interface ICancerFullObserver :
		ICancerDeathObserver,
		ICancerDivisionObserver
	{

	}
}