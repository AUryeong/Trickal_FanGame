using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI
{
    public class DOTweenSequenceAnimator : UIMonoBehaviour
    {
        public DOTweenAnimator[] doTweenAnimators;
        private Sequence doTweenSequence;
        private Sequence doTweenBackSequence;
        
        [Header("Animator Value")]
        [SerializeField] [Range(0, 5)] private float animDelay;
        [SerializeField] private bool autoEnableRestart;

        [Button("Play")]
        public Sequence Play(float delay = -1)
        {
            if (doTweenSequence == null)
            {
                doTweenSequence = DOTween.Sequence();
                doTweenSequence.SetAutoKill(false);

                TweenCallback onStart = () => { };
                foreach (var animator in doTweenAnimators)
                {
                    doTweenSequence.Insert(animator.animDelay, animator.Play());
                    onStart += animator.OnStart;
                }

                doTweenSequence.OnStart(onStart);
                doTweenSequence.SetDelay(delay < 0 ? animDelay : delay);
            }
            else
            {
                doTweenBackSequence?.Pause();
                doTweenSequence.Restart();
            }

            return doTweenSequence;
        }

        [Button("Play Back")]
        public Sequence PlayBack(float delay = -1)
        {
            if (doTweenBackSequence == null)
            {
                doTweenBackSequence = DOTween.Sequence();
                doTweenBackSequence.SetAutoKill(false);

                TweenCallback onStart = () => { };
                foreach (var animator in doTweenAnimators)
                {
                    doTweenBackSequence.Insert(animator.animDelay, animator.PlayBack());
                    onStart += animator.OnStartBack;
                }

                doTweenBackSequence.OnStart(onStart);
                doTweenBackSequence.SetDelay(delay < 0 ? animDelay : delay);
            }
            else
            {
                doTweenSequence?.Pause();
                doTweenBackSequence.Restart();
            }

            return doTweenBackSequence;
        }
        
        #if UNITY_EDITOR
        private void Reset()
        {
            doTweenAnimators = GetComponentsInChildren<DOTweenAnimator>();
        }
        #endif

        private void OnEnable()
        {
            if (autoEnableRestart)
                Play();
        }
    }
}