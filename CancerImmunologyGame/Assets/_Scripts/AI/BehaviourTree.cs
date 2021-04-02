using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviourTreeBase
{
	[System.Serializable]
	public class BehaviourTree
	{
		[Header("BehaviourTree")]
		public bool instant = false;
		[SerializeReference]
		public BTNode rootNode = null;
		[SerializeReference]
		public BTNode currentProcessingNode = null;

		public float reevaluateTime = 2.0f;
		protected float timeToWaitBeforeReevaluation = 0.0f;

		public BehaviourTree() {}

		public BehaviourTree(BTNode rootNode)
		{
			this.rootNode = rootNode;
			currentProcessingNode = null;
			ResetTree();
		}

		public virtual void ResetTree()
		{
			rootNode.ResetTreeNode();
			currentProcessingNode = null;
		}

		public void Evaluate()
		{
			if (rootNode.NodeState != NodeStates.RUNNING)
			{
				ResetTree();
				rootNode.Evaluate();
				return;
			}


			if (currentProcessingNode != null && currentProcessingNode.NodeState == NodeStates.RUNNING)
			{
				timeToWaitBeforeReevaluation += Time.deltaTime;
				if (timeToWaitBeforeReevaluation > reevaluateTime && currentProcessingNode.allowTreeToReevaluate)
				{

					timeToWaitBeforeReevaluation = 0.0f;
					ResetTree();
					rootNode.Evaluate();
					return;
				}

				currentProcessingNode.Evaluate();
		
				if (instant)
				{
					rootNode.Evaluate();
				}
			} 
			else
			{
				rootNode.Evaluate();
			}

		}

	}
}