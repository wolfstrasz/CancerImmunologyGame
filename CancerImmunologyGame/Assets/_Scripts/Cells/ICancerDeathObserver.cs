using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ImmunotherapyGame.Cancers 
{ 
	public interface ICancerDeathObserver
	{
		void OnCancerDeath(Cancer cancer);
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