using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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


		public Cell owner
		{
			get
			{
				return owner;
			}
			set
			{
				owner = value;
				if (autoHide)
				{
					owner.onUpdateHealth = UpdateHealthbar;
				}
			}
		}

		public void UpdateHealthbar()
		{
			slider.value = owner.Health;
			if (slider.maxValue != owner.Health)
			{
				image.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			}
		}

		public float MaxHealth
		{
			get => slider.maxValue;
			set => slider.maxValue = value;
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

	}
}