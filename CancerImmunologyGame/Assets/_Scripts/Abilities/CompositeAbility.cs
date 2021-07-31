using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImmunotherapyGame.Abilities
{
	[CreateAssetMenu(menuName = "MyAssets/Abilities/Composite Ability")]
	public class CompositeAbility : Ability, ISerializationCallbackReceiver
	{
		[Header("Ability Components")]
		[SerializeField] protected List<Ability> abilities = new List<Ability>();
		private bool alreadyCalled = false;

		public override bool CastAbility(GameObject abilityCaster, GameObject target)
		{
			Debug.Log("Casting ability: " + this);
			if (alreadyCalled)
			{
				Debug.LogError("Composite Ability loop found at : " + GetType() + ". Backtrack Log:");
				return false;
			}

			for (int i = 0; i < abilities.Count; ++i)
			{
				Debug.Log("Casting Composite part: " + abilities[i].name);
				if (!abilities[i].CastAbility(abilityCaster, target))
				{
					Debug.LogError("Backtrack Log : " + this.GetType());
					return false;
				}
			}

			alreadyCalled = false;
			return true;
		}

		public void OnAfterDeserialize()
		{
			alreadyCalled = false;

			var compositeType = this.GetType();
			Debug.Log("Composite type + " + compositeType + " has items: ");
			for ( int i = 0; i < abilities.Count; ++i)
			{
				Debug.Log("Item (" + (i + 1) + ") of type: " + abilities[i].GetType());
				if (abilities[i].GetType().Equals(this.GetType()))
				{
					Debug.LogWarning("Composite Ability has a Composite Ability as a child. Possibility of unending loops!");
				}
			}
		}

		public void OnBeforeSerialize()
		{
		}
	}
}
