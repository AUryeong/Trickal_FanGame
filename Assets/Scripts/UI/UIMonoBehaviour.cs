using UnityEngine;

namespace UI
{
    public class UIMonoBehaviour : MonoBehaviour
    {
        private RectTransform rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                return rectTransform ??= GetComponent<RectTransform>();
            }
        }
    }
}