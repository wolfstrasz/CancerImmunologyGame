using UnityEngine;

namespace ImmunotherapyGame.Core
{
    [CreateAssetMenu]
    public class StatAttribute : ScriptableObject, ISerializationCallbackReceiver
    {
        public string attributeName;
        public float initialValue;
        private float currentValue;

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
