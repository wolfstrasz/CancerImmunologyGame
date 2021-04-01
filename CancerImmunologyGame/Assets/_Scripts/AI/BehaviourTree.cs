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

		public BehaviourTree() {}

		public BehaviourTree(BTNode rootNode)
		{
			this.rootNode = rootNode;
			currentProcessingNode = null;
			ResetTree();
		}

		public virtual void ResetTree()
		{
			currentProcessingNode = null;
			rootNode.ResetTreeNode();
		}

		public void Evaluate()
		{
			Debug.Log("-------------------------");
			Debug.Log("Evaluating tree");
			if (rootNode.NodeState != NodeStates.RUNNING)
			{
				Debug.Log("Resseting tree");
				ResetTree();
				rootNode.Evaluate();
				return;
			}


			if (currentProcessingNode != null && currentProcessingNode.NodeState == NodeStates.RUNNING)
			{
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