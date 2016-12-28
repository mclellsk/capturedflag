using UnityEngine;
using System.Collections.Generic;

namespace CapturedFlag.tk2d
{
    /// <summary>
    /// Scrollable flow component to view game objects with the ability to select specific objects.
    /// </summary>
    [RequireComponent(typeof(tk2dUIScrollableArea))]
    public class tk2dUIScrollableFlow : MonoBehaviour
    {
        /// <summary>
        /// Content to be scrolled through.
        /// </summary>
        public List<GameObject> btns = new List<GameObject>();

        /// <summary>
        /// Scrollable area from tk2d.
        /// </summary>
        public tk2dUIScrollableArea scrollableArea;

        /// <summary>
        /// Optional controller to cycle through the contents of the flow in the left direction.
        /// </summary>
        public tk2dUIItem btnLeft;
        /// <summary>
        /// Optional controller to cycle through the contents of the flow in the right direction.
        /// </summary>
        public tk2dUIItem btnRight;

        /// <summary>
        /// Container for the game object content.
        /// </summary>
        public GameObject contentContainer;
        /// <summary>
        /// Scrolling right on screen identifier.
        /// </summary>
        public GameObject gfxContentRight;
        /// <summary>
        /// Scrolling left on screen identifier.
        /// </summary>
        public GameObject gfxContentLeft;

        /// <summary>
        /// Game object currently selected in the flow.
        /// </summary>
        [HideInInspector]
        public GameObject selectedBtn = null;
        /// <summary>
        /// Game object previously select in the flow.
        /// </summary>
        [HideInInspector]
        public GameObject oldSelectedBtn = null;

        /// <summary>
        /// Scale of content in the flow.
        /// </summary>
        public float scale = 1.5f;
        /// <summary>
        /// Distance between the content inside the container.
        /// </summary>
        public float distanceBetweenContent = 3f;
        /// <summary>
        /// Percent to start scroll flow.
        /// </summary>
        public float scrollPercent = 0f;
        /// <summary>
        /// Determines the size rescaling of content in the flow as they move away from the center point.
        /// The larger this value, the smaller content will look the further from the center it is and vice versa.
        /// It is basically the change in scale for every one unit of change in the distance from the center to 
        /// the content object's current position related to the x-axis.
        /// </summary>
        public float scalingFactor = 0.1f;

        /// <summary>
        /// Selection changed callback.
        /// </summary>
        public event System.Action OnSelectChanged;

        private float _oldBtnX = 0f;

        public void Initialize()
        {
            selectedBtn = null;

            //Reposition the contents of the scrollable flow
            for (int i = 0; i < btns.Count; i++)
            {
                btns[i].transform.position = new Vector3(contentContainer.transform.position.x + distanceBetweenContent * i, contentContainer.transform.position.y, btns[i].transform.position.z);
            }

            if (btns.Count > 0 && btns.FindAll(p => p.activeSelf).Count > 0)
            {
                var contentLength = Mathf.Max(1f, scrollableArea.GetComponent<tk2dUIScrollableArea>().MeasureContentLength());
                scrollableArea.GetComponent<tk2dUIScrollableArea>().ContentLength = contentLength;
            }

            FlowSelect();
        }

        private void Awake()
        {
            Initialize();
            OnSelectChanged += FlowSelect;

            if (btnLeft != null)
                btnLeft.OnClick += new System.Action(Click_Left);
            if (btnRight != null)
                btnRight.OnClick += new System.Action(Click_Right);
        }

        //Simulates a swipe left
        private void Click_Right()
        {
            var activeBtns = btns.FindAll(p => p.activeSelf);
            var spacers = (activeBtns.Count - 1);

            if (spacers > 0)
            {
                if (selectedBtn != null)
                {
                    var btnIndex = activeBtns.FindIndex(p => p == selectedBtn);
                    if (btnIndex >= 0 && btnIndex < (activeBtns.Count - 1))
                    {
                        var position = (btnIndex + 1) * distanceBetweenContent;
                        var length = scrollableArea.GetComponent<tk2dUIScrollableArea>().ContentLength;
                        var trueLength = activeBtns[activeBtns.Count - 1].transform.localPosition.x;
                        var delta = length - trueLength;
                        //Half the difference between the true length and the calculated length, 
                        //divided among every spacer (the offset is larger at the end of the flow).
                        var deltaOffset = (delta / 2f) / spacers;
                        var percentage = (position + (deltaOffset * (btnIndex + 1))) / length;
                        scrollableArea.SetScrollPercentWithoutEvent(percentage * 1f);
                    }
                }
            }
        }

        //Simulates a swipe right
        private void Click_Left()
        {
            var activeBtns = btns.FindAll(p => p.activeSelf);
            var spacers = (activeBtns.Count - 1);

            if (spacers > 0)
            {
                if (selectedBtn != null)
                {
                    var btnIndex = activeBtns.FindIndex(p => p == selectedBtn);
                    if (btnIndex > 0)
                    {
                        var position = (btnIndex - 1) * distanceBetweenContent;
                        var length = scrollableArea.GetComponent<tk2dUIScrollableArea>().ContentLength;
                        var trueLength = activeBtns[activeBtns.Count - 1].transform.localPosition.x;
                        var delta = length - trueLength;
                        var deltaOffset = (delta / 2f) / spacers;
                        var percentage = (position + (deltaOffset * (btnIndex - 1))) / length;
                        scrollableArea.SetScrollPercentWithoutEvent(percentage * 1f);
                    }
                }
            }
        }

        private void Start()
        {
            scrollableArea.SetScrollPercentWithoutEvent(scrollPercent);
        }

        private void FlowSelect()
        {
            SetContentMarkers();
        }

        public void Update()
        {
            float zDepth = (contentContainer.transform.localPosition.x);

            var activeBtns = btns.FindAll(p => p.activeSelf);
            for (int i = 0; i < activeBtns.Count; i++)
            {
                //Change this to represent the maximum number of flow items in the visible area.
                float maxDepthZ = -10f;
                //Truncate to the tenth decimal place and use as percentage of maxDepthZ
                float z = (int)(((1f - Mathf.Abs(activeBtns[i].transform.localPosition.x + zDepth) * scalingFactor)) * maxDepthZ);

                activeBtns[i].transform.localScale = new Vector3(   scale * (Mathf.Max(0.1f, 1f - Mathf.Abs(activeBtns[i].transform.localPosition.x + zDepth) * scalingFactor)),
                                                                    scale * (Mathf.Max(0.1f, 1f - Mathf.Abs(activeBtns[i].transform.localPosition.x + zDepth) * scalingFactor)),
                                                                    activeBtns[i].transform.localScale.z);

                activeBtns[i].transform.localPosition = new Vector3(activeBtns[i].transform.localPosition.x,
                                                                    activeBtns[i].transform.localPosition.y,
                                                                    z);
            }

            //Selection Algorithm for Buttons
            if (btns.Count > 0)
            {
                if (btns[0].transform.position.x != _oldBtnX || selectedBtn == null) //Checks for a change in x-value at all
                {
                    for (int i = 0; i < btns.Count; i++)
                    {
                        if (btns[i].activeSelf)
                        {
                            if (selectedBtn == null)
                            {
                                selectedBtn = btns[i];
                            }
                            else
                            {
                                if (btns[i].transform.position.z < selectedBtn.transform.position.z)
                                {
                                    selectedBtn = btns[i];
                                }
                            }
                        }
                    }

                    //Set old z to the current z position...
                    _oldBtnX = btns[0].transform.position.x;
                }
            }

            //Forces the selected button to remain the closest object to the UI Camera's near plane.
            if (selectedBtn != null)
            {
                selectedBtn.transform.position = new Vector3(selectedBtn.transform.position.x, selectedBtn.transform.position.y, selectedBtn.transform.position.z - 5);
            }

            if (selectedBtn != oldSelectedBtn)
            {
                if (OnSelectChanged != null)
                {
                    OnSelectChanged();
                }
            }

            oldSelectedBtn = selectedBtn;
        }

        private void SetContentMarkers()
        {
            var index = btns.FindIndex(p => p == selectedBtn);
            if (index >= 0)
            {
                if (gfxContentLeft != null)
                    gfxContentLeft.SetActive(index > 0);
                if (gfxContentRight != null)
                    gfxContentRight.SetActive(index < (btns.Count - 1));
            }
        }
    }
}
