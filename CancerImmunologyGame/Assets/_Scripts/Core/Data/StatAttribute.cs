using UnityEngine;
using System.Collections.Generic;

namespace ImmunotherapyGame.Core
{
    [CreateAssetMenu]
    public class StatAttribute : ScriptableObject, ISerializationCallbackReceiver
    {
        public string attributeName;

		[SerializeField] private float initialValue;
        [SerializeField] [ReadOnly] private float currentValue;
		[SerializeField] [ReadOnly] private List<StatAttribute> modifiers = new List<StatAttribute>();

		public float CurrentValue
		{
			get => currentValue;
			set
			{
				currentValue = value;
				if (onValueChanged != null)
					onValueChanged();
			}
		}

		public delegate void OnValueChanged();
        public OnValueChanged onValueChanged;


		public void OnAfterDeserialize()
		{
            CurrentValue = initialValue;
		}

		public void OnBeforeSerialize() { }

		public void AddModifier(StatAttribute modifier)
		{
			modifiers.Add(modifier);
			currentValue += modifier.currentValue;
			if (onValueChanged != null)
				onValueChanged();
		}

		public void RemoveModifier(StatAttribute modifier)
		{
			int lastIndex = modifiers.Count - 1;
			for (int i = lastIndex; i >= 0; --i)
			{
				if (modifiers[i] == modifier)
				{
					modifiers[i] = modifiers[lastIndex];
					currentValue -= modifier.currentValue;
					modifiers.RemoveAt(lastIndex);
					if (onValueChanged != null)
						onValueChanged();
					break;
				}
			}
		}
	}
}
