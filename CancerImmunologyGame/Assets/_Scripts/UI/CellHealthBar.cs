using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ImmunotherapyGame.Core;

namespace ImmunotherapyGame
{
	public class CellHealthBar : MonoBehaviour
	{
		[SerializeField] private Cell owner = null;
		[SerializeField] private Slider slider = null;
		[SerializeField] private bool autoHide = false;
		[SerializeField] private float autoHideScale = 1.0f;
		[SerializeField] private Image image = null;

		public void UpdateHealth()
		{
			slider.value = owner.CurrentHealth;
			image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);
		}

		public void UpdateMaxHealth()
		{
			if (owner != null)
			{
				slider.maxValue = owner.cellType.MaxHealth;
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
					image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
				}
			}
		}

		private void OnEnable()
		{
			if (owner != null)
			{
				if (owner.cellType.maxHealth != null)
				{
					owner.cellType.maxHealth.onValueChanged += UpdateMaxHealth;
					UpdateMaxHealth();
				}

				owner.onUpdateHealth += UpdateHealth;
				UpdateHealth();
			}
	
		}

		private void OnDisable()
		{
			if (owner != null)
			{
				if (owner.cellType.maxHealth != null)
				{
					owner.cellType.maxHealth.onValueChanged -= UpdateMaxHealth;
				}

				owner.onUpdateHealth -= UpdateHealth;
			}
		}
	}
}