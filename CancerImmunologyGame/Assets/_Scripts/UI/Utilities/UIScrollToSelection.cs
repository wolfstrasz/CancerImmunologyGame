using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ImmunotherapyGame.UI
{

    [RequireComponent(typeof(ScrollRect))]
    public class UIScrollToSelection : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private ScrollType scrollDirection;
		[SerializeField] private int childDepthCheck = 1;
        [SerializeField] [ReadOnly] float timeToStopHorizontalScroll = 0.1f;
        [SerializeField] [ReadOnly] float timeToStopVerticalScroll = 0.01f;

        // SCROLL REFERENCES
        protected ScrollRect scrollRect { get; set; }
        protected RectTransform scrollRectTransform { get; set; }
        protected RectTransform scrollContent
        {
            get { return scrollRect != null ? scrollRect.content : null; }
        }

        // EVENT SYSTEM SELECTION
        protected EventSystem CurrentEventSystem => EventSystem.current;
        protected GameObject LastCheckedGameObject { get; set; }
        protected GameObject CurrentSelectedGameObject => EventSystem.current.currentSelectedGameObject;

        // SCROLLING
        protected RectTransform CurrentTargetRectTransform { get; set; }
        private bool scrollToVerticalSelection = false;
        private bool scrollToHorizontalSelection = false;

        private bool MainReferencesMissing => (scrollRect == null || scrollContent == null || scrollRectTransform == null);

        //*** METHODS - PROTECTED ***//
        protected virtual void Awake()
        {
            scrollRect = GetComponent<ScrollRect>();
            scrollRectTransform = scrollRect.GetComponent<RectTransform>();
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
                Debug.Log("Update selected");

                // Get transform of current object
                Transform targetTransform = CurrentSelectedGameObject.transform;


                // Check if selected object is part of the content
                // By finding transform object parent that is child of the content
                int depth = childDepthCheck;

                while (targetTransform != null && depth > 0 && targetTransform.parent != scrollContent.gameObject.transform)
				{
                    targetTransform = targetTransform.parent;
                    --depth;
                }

                // If going up the parent tree we do not reach the content then we just not update layout
                if (targetTransform == null || depth < 0)
				{
                    CurrentTargetRectTransform = null;
                    timeToStopHorizontalScroll = 0f;
                    timeToStopVerticalScroll = 0f;
                } 
                else // The target that has to be in screen is the object that is child of the content
				{
                    CurrentTargetRectTransform = targetTransform.GetComponent<RectTransform>();
                }

                LastCheckedGameObject = CurrentSelectedGameObject;
                timeToStopHorizontalScroll = 0.1f;
                timeToStopVerticalScroll = 0.1f;

                // Update position depending on selection
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

			float selectionTopBound = selection.anchoredPosition.y + scrollContent.anchoredPosition.y + selection.rect.height; // positive
            float selectionBottomBound = selection.anchoredPosition.y + scrollContent.anchoredPosition.y - selection.rect.height; // Negative

			float offlimitsValue = 0f;


            if (selectionTopBound > 0f)
            {
                offlimitsValue = (selectionTopBound / scrollContent.rect.height);
            }
            else if (selectionBottomBound < -scrollRectTransform.rect.height)
			{
				offlimitsValue =  (selectionBottomBound - (-scrollRectTransform.rect.height)) / scrollContent.rect.height;
            }
			scrollRect.verticalNormalizedPosition += offlimitsValue;

			if (Mathf.Abs(offlimitsValue) <= 0.001f || scrollRect.verticalNormalizedPosition <= 0f || scrollRect.verticalNormalizedPosition >= 1f)
			{
                if (Time.timeScale == 0f)
				{
                    scrollToVerticalSelection = false;
				}
                else
				{
                    timeToStopVerticalScroll -= Time.deltaTime;
                    if (timeToStopVerticalScroll <= 0f)
                    {
                        scrollToVerticalSelection = false;
                    }
                }
               
            }

		}

		private void UpdateHorizontalScrollPosition(RectTransform selection)
		{
            // Check if it out of bounds
            float selectionLeftBound = selection.anchoredPosition.x - selection.rect.width + scrollContent.anchoredPosition.x;
            float selectionRightBound = selection.anchoredPosition.x + selection.rect.width  + scrollContent.anchoredPosition.x;
            float offlimitsValue = 0f;

            if (selectionLeftBound < 0)
			{
                offlimitsValue = selectionLeftBound / scrollContent.rect.width;
            } else if (selectionRightBound > scrollRectTransform.rect.width)
			{
                offlimitsValue = (selectionRightBound - scrollRectTransform.rect.width) / scrollContent.rect.width;
            }

            scrollRect.horizontalNormalizedPosition += offlimitsValue;
            if (Mathf.Abs(offlimitsValue) <= 0.001f || scrollRect.horizontalNormalizedPosition <= 0f || scrollRect.horizontalNormalizedPosition >= 1f) 
            {

                if (Time.timeScale == 0f)
                {
                    scrollToVerticalSelection = false;
                }
                else
                {
                    timeToStopHorizontalScroll -= Time.deltaTime;
                    if (timeToStopHorizontalScroll <= 0f)
                    {
                        scrollToHorizontalSelection = false;
                    }
                }
            }
        }

		public enum ScrollType
        {
            VERTICAL,
            HORIZONTAL,
            BOTH
        }
    }

}
