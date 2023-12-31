using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sirenix.OdinInspector;

namespace UI
{
    public class DOTweenAnimator : UIMonoBehaviour
    {
        [HideInInspector] public Vector2 animPos;
        
        [SerializeField] private Ease animEase = Ease.OutQuad;
        [SerializeField] private Ease animBackEase = Ease.Unset;
        
        [SerializeField] [Range(0, 5)] private float animDuration;
        [Range(0, 5)] public float animDelay;
        
        [HideInInspector] public Vector2 defaultPos;

        private void Awake()
        {
            defaultPos = RectTransform.anchoredPosition;
        }

        #if UNITY_EDITOR
        private void Reset()
        {
            animPos = RectTransform.anchoredPosition;
        }
        #endif

        [Button("Play")]
        public TweenerCore<Vector2, Vector2, VectorOptions> Play(float delay = -1, float duration = -1)
        {
            RectTransform.DOKill();
            
            OnStart();
            var tween = RectTransform.DOAnchorPos(animPos, duration < 0 ? animDuration : animDuration + duration).
                SetEase(animEase).
                SetDelay(delay < 0 ? animDelay : animDelay + delay);
            return tween;
        }
        
        [Button("PlayBack")]
        public TweenerCore<Vector2, Vector2, VectorOptions> PlayBack(float delay = -1, float duration = -1)
        {
            RectTransform.DOKill();
            
            OnStartBack();
            var tween = RectTransform.DOAnchorPos(defaultPos, duration < 0 ? animDuration : animDuration + duration).
                SetEase(animBackEase == Ease.Unset ? animEase : animBackEase).
                SetDelay(delay < 0 ? animDelay : animDelay + delay);
            return tween;
        }

        public void OnStart()
        {
            RectTransform.anchoredPosition = defaultPos;
        }

        public void OnStartBack()
        {
            RectTransform.anchoredPosition = animPos;
        }
    }
}