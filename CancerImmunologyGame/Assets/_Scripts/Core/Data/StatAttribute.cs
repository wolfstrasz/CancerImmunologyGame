using UnityEngine;
using System.Collections.Generic;

namespace ImmunotherapyGame.Core
{
    [CreateAssetMenu (menuName = "MyAssets/Stat Attribute")]
    public class StatAttribute : ScriptableObject, ISerializationCallbackReceiver
    {
        public string attributeName;

		[SerializeField] private float initialValue;
        [SerializeField] [ReadOnly] private float currentValue;

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
	}
}
