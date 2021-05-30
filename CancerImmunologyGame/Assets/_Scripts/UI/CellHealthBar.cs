using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ImmunotherapyGame.Core;

namespace ImmunotherapyGame
{
	public class CellHealthBar : MonoBehaviour
	{

		[SerializeField]
		private Slider slider = null;
		[SerializeField]
		private bool autoHide = false;
		[SerializeField]
		private float autoHideScale = 1.0f;
		[SerializeField]
		private Image image = null;
		[SerializeField]
		private StatAttribute maxHealthAttribute = null;

		public Cell owner
		{
			get
			{
				return owner;
			}
			set
			{
				owner = value;
				owner.onUpdateHealth = UpdateHealth;
			}
		}

		public void UpdateHealth(float value)
		{
			slider.value = value;
			image.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		}

		public void UpdateMaxHealth()
		{
			slider.maxValue = maxHealthAttribute.CurrentValue;
		}

		public float Health
		{
			get => slider.value;
			set
			{
				slider.value = value;
			}

		}

		void Update()
		{
			if (autoHide)
			{
				float alpha = image.color.a;
				if (alpha > 0.0f)
				{
					alpha -= autoHideScale * Time.deltaTime;
					image.color = new Color(1.0f, 1.0f, 1.0f, alpha);
				}
			}
		}

		private void OnEnable()
		{
			if (owner != null)
			{
				owner.onUpdateHealth += UpdateHealth;
			}

			if (maxHealthAttribute != null)
			{
				maxHealthAttribute.onValueChanged += UpdateMaxHealth;
			}
		}

		private void OnDisable()
		{
			if (owner != null)
			{
				owner.onUpdateHealth -= UpdateHealth;
			}

			if (maxHealthAttribute != null)
			{
				maxHealthAttribute.onValueChanged -= UpdateMaxHealth;
			}
		}
	}
}