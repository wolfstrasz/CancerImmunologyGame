using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ImmunotherapyGame.UI
{
    /// Credit zero3growlithe
    /// sourced from: http://forum.unity3d.com/threads/scripts-useful-4-6-scripts-collection.264161/page-2#post-2011648

    [RequireComponent(typeof(ScrollRect))]
    public class UIScrollToSelection : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private ScrollType scrollDirection;
        //[SerializeField]
        //private float scrollSpeed = 10f;
        [SerializeField]
        private int childDepthCheck = 1;

        // SCROLL REFERENCES
        protected RectTransform ScrollWindow { get; set; }
        protected ScrollRect TargetScrollRect { get; set; }
        protected RectTransform TargetScrollContent
        {
            get { return TargetScrollRect != null ? TargetScrollRect.content : null; }
        }

        // EVENT SYSTEM SELECTION
        protected EventSystem CurrentEventSystem => EventSystem.current;
        protected GameObject LastCheckedGameObject { get; set; }
        protected GameObject CurrentSelectedGameObject => EventSystem.current.currentSelectedGameObject;

        // SCROLLING
        protected RectTransform CurrentTargetRectTransform { get; set; }
        private bool scrollToVerticalSelection = false;
        private bool scrollToHorizontalSelection = false;

        private bool MainReferencesMissing => (TargetScrollRect == null || TargetScrollContent == null || ScrollWindow == null);

        //*** METHODS - PROTECTED ***//
        protected virtual void Awake()
        {
            TargetScrollRect = GetComponent<ScrollRect>();
            ScrollWindow = TargetScrollRect.GetComponent<RectTransform>();
        }

        protected virtual void Update()
        {
            if (MainReferencesMissing)
                return;

            UpdateReferences();

            RectTransform selection = CurrentTargetRectTransform;
            if (selection == null)
                return;

			if (scrollToHorizontalSelection)
				UpdateHorizontalScrollPosition(selection);

			if (scrollToVerticalSelection)
                UpdateVerticalScrollPosition(selection);
        }

        //*** METHODS - PRIVATE ***//
        private void UpdateReferences()
        {
            // update current selected rect transform
            if (CurrentSelectedGameObject != LastCheckedGameObject && CurrentSelectedGameObject != null)
            {
                Transform targetTransform = CurrentSelectedGameObject.transform;

                // Find object that is a child of the scroll rect but a parent of the target object at maximum height parentHeight
                int depth = childDepthCheck;

                while (targetTransform != null && depth > 0 && targetTransform.parent != transform)
				{
                    targetTransform = targetTransform.parent;
                    --depth;

                }

                CurrentTargetRectTransform = targetTransform == null ? null 
                    : targetTransform.GetComponent<RectTransform>();
            
                LastCheckedGameObject = CurrentSelectedGameObject;

                switch (scrollDirection)
                {
                    case ScrollType.VERTICAL:
                        scrollToVerticalSelection = true;
                        scrollToHorizontalSelection = false;
                        break;
                    case ScrollType.HORIZONTAL:
                        scrollToHorizontalSelection = true;
                        scrollToVerticalSelection = false;

                        break;
                    case ScrollType.BOTH:
                        scrollToVerticalSelection = true;
                        scrollToHorizontalSelection = true;
                        break;
                }
            }
        }


        private void UpdateVerticalScrollPosition(RectTransform selection)
        {
            // move the current scroll rect to correct position
            float selectionCentrePosition = -selection.anchoredPosition.y - (selection.rect.height * (0.5f - selection.pivot.y));
            float elementHalfHeight = selection.rect.height / 2f;
            float windowHeight = ScrollWindow.rect.height;
            float contentAnchorPosition = TargetScrollContent.anchoredPosition.y;

            // get the element offset value depending on the cursor move direction
            float offlimitsValue = GetScrollOffset(selectionCentrePosition, contentAnchorPosition, elementHalfHeight, windowHeight);

            // move the target scroll rect
            TargetScrollRect.verticalNormalizedPosition += (offlimitsValue / TargetScrollContent.rect.height);
            if (Mathf.Abs(offlimitsValue) < 1f)
                scrollToVerticalSelection = false;
        }

        private void UpdateHorizontalScrollPosition(RectTransform selection)
        {
            // move the current scroll rect to correct position
            float selectionPosition = -selection.anchoredPosition.x - (selection.rect.width * (0.5f - selection.pivot.x));

            float elementWidth = selection.rect.width;
            float maskWidth = ScrollWindow.rect.width;
            float contentAnchorPosition = -TargetScrollContent.anchoredPosition.x;
            
            // get the element offset value depending on the cursor move direction
            float offlimitsValue = -GetScrollOffset(selectionPosition, contentAnchorPosition, elementWidth, maskWidth);
            // move the target scroll rect
            TargetScrollRect.horizontalNormalizedPosition += (offlimitsValue / TargetScrollContent.rect.width);

            if (Mathf.Abs(offlimitsValue) < 1f)
                scrollToHorizontalSelection = false;
        }

        private float GetScrollOffset(float targetPosition, float contentAnchorPosition, float targetHalfLength, float windowLength)
        {
            
            if (targetPosition - targetHalfLength < contentAnchorPosition)
            {
                return (contentAnchorPosition - targetPosition + targetHalfLength) ;
            }
			else if (targetPosition + targetHalfLength > contentAnchorPosition + windowLength)
			{
				return (contentAnchorPosition + windowLength) - (targetPosition + targetHalfLength);
			}

			return 0;
        }


        public enum ScrollType
        {
            VERTICAL,
            HORIZONTAL,
            BOTH
        }
    }

}
