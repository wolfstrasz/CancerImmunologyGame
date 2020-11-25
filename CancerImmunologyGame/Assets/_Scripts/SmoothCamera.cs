using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(Camera))]
public class SmoothCamera : SSystem<SmoothCamera>
{

    [SerializeField, Range(1.0f, 20.0f)]
    float zoomDistance = 8.0f;

    [SerializeField]
    public GameObject currentTarget;
    private Vector3 focusPosition;
    private Vector3 targetPosition;

    //[SerializeField, Range(0.1f, 0.5f)] 
    //private float distanceDeltaToFocus = 0.3f;
    [SerializeField, Range(0.5f, 1.0f)] 
    private float focusCenteringSpeed = 0.943f;


    // REMOVE THIS FROM HERE
    // FIRST TUTORIAL STARTUP
    public float distanceDeltaToFocus = 0.2f;
    public bool nextTut = false;
    public bool isCameraFocused = false;

    void Awake()
    {
        //focusPosition = focus.position;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    // Target's position will change in Update, so camera should move focus here
    void FixedUpdate()
    {
        UpdateFocusPoint();
        if (!nextTut)
        {
        //    Debug.Log("CHECK FOR FIRST TUT");

            if (isCameraFocused)
            {
          //      Debug.Log("SHOW TUTORIAL FROM CAMERA CALL");
                UIManager.Instance.NextTutorial();
                nextTut = true;
            }
        }
    }

    // Focus objects can change in Update, so camera moves here
    void LateUpdate()
    {
        Vector3 lookDirection = transform.forward;
        Vector3 lookPosition = focusPosition - lookDirection * zoomDistance;
        transform.position = lookPosition;
    }

    private void HandleInput()
    {

    }

    private void UpdateFocusPoint()
    {
        targetPosition = currentTarget.transform.position;
        float distance = Vector3.Distance(targetPosition, focusPosition);
        //// Check if we need to centre the camera (REMOVE THIS WHEN REMOVING FIRST TUT ACTIVATION
        if (distance < 0.2f /*distanceDeltaToFocus*/)
        {
            isCameraFocused = true;
         //   Debug.Log("THIS HAPPENS");
        } else
        {
            isCameraFocused = false;
        }

     //   Debug.Log(distance);

        if (distance > 0.01f)
        {
            // Time.deltaTime switch to unscaledDeltaTime to cover when game is paused
            focusPosition = Vector3.Lerp(targetPosition, focusPosition, (float)System.Math.Pow((1.0f - focusCenteringSpeed), Time.deltaTime));
        }
    }

}



