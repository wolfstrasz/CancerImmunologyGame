using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviourTreeBase
{
	public abstract class BehaviourTree : MonoBehaviour
	{
		[Header("BehaviourTree")]
		public float reevaluateTime = 2.0f;
		protected float timeToWaitBeforeReevaluation = 0.0f;

		[SerializeReference]
		public BTNode rootNode = null;
		[SerializeReference]
		public BTNode currentProcessingNode = null;
		public bool instant = false;

		protected virtual void ResetTree()
		{
			currentProcessingNode = null;
			rootNode.ResetTreeNode();
		}

		void Update()
		{
			Evaluate();
		}

		public void Evaluate()
		{
			Debug.Log("-------------------------");
			Debug.Log("Evaluating tree");
			if (rootNode.NodeState != NodeState.RUNNING)
			{
				Debug.Log("Resseting tree");
				ResetTree();
				rootNode.Evaluate();
				return;
			}


			if (currentProcessingNode != null && currentProcessingNode.NodeState == NodeState.RUNNING)
			{
				timeToWaitBeforeReevaluation += Time.deltaTime;
				if (timeToWaitBeforeReevaluation > reevaluateTime && currentProcessingNode.allowTreeToReevaluate)
				{
					Debug.Log("RE - Evaluating tree");

					timeToWaitBeforeReevaluation = 0.0f;
					ResetTree();
					rootNode.Evaluate();
					return;
				}
	
				Debug.Log("Processing from middle");
				currentProcessingNode.Evaluate();
		

				if (instant)
				{
					rootNode.Evaluate();
				}
			} 
			else
			{
				Debug.Log("Processing without being instant");
				rootNode.Evaluate();
			}
			Debug.Log("Finished Tree Evaluation");
			Debug.Log("-------------------------");

		}

	}
}