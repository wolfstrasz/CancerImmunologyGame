using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialEvent : MonoBehaviour
{
	public TutorialStage owner = null;

	[SerializeField]
	protected bool removeControl;
	[SerializeField]
	protected bool pauseTime;

	private bool finished = true;
	private float previousTimeScale = 0.0f;
	public void Start()
	{
		if (removeControl)
		{
			// Remove control
		}
		if (pauseTime)
		{
			previousTimeScale = Time.timeScale;
			Time.timeScale = 0.0f;
		}

		OnStart();
		finished = false;
	}

	public void End()
	{
		if (removeControl)
		{
			// return control
		}

		if (pauseTime)
		{
			Time.timeScale = previousTimeScale;
		}
		OnEnd();
	}

	protected abstract void OnStart();

	protected abstract bool OnUpdate();

	protected abstract void OnEnd();

	void Update()
	{
		if (finished) return;
		if (OnUpdate())
		{
			End();
			finished = true;
			owner.OnEventFinished();
			Destroy(gameObject);
		}
	}
}
