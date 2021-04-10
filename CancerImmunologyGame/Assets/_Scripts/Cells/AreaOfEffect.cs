using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class AreaOfEffect : MonoBehaviour
{

	private void Update()
	{
		OnEffectStatus();
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		KillerCell pc = collider.gameObject.GetComponent<KillerCell>();
		if (pc != null)
		{
			OnActivation();
		}
	}

	private void OnTriggerExit2D(Collider2D collider)
	{
		KillerCell pc = collider.gameObject.GetComponent<KillerCell>();
		if (pc != null)
		{
			OnDeactivation();
		}
	}

	protected abstract void OnActivation();

	protected abstract void OnDeactivation();

	protected abstract void OnEffectStatus();

}
