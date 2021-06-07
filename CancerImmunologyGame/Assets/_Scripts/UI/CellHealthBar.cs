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
		private Cell owner = null;
		[SerializeField]
		private Slider slider = null;
		[SerializeField]
		private bool autoHide = false;
		[SerializeField]
		private float autoHideScale = 1.0f;
		[SerializeField]
		private Image image = null;

		public void UpdateHealth()
		{
			slider.value = owner.CurrentHealth;
			image.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		}

		public void UpdateMaxHealth()
		{
			if (owner != null)
				slider.maxValue = owner.cellType.MaxHealth;
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

			if (owner.cellType.maxHealth != null)
			{
				owner.cellType.maxHealth.onValueChanged += UpdateMaxHealth;
			}
		}

		private void OnDisable()
		{
			if (owner != null)
			{
				owner.onUpdateHealth -= UpdateHealth;
			}

			if (owner.cellType.maxHealth != null)
			{
				owner.cellType.maxHealth.onValueChanged -= UpdateMaxHealth;
			}
		}
	}
}