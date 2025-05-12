using System.Collections;
using UnityEngine;

namespace Game.Durak.UI
{
    public abstract class AnimatedIndicatorBase
        : MonoBehaviour
    {
        [SerializeField] private AnimationCurve sizeCurve;
        [SerializeField] private AnimationCurve positionCurve;

        [SerializeField] private Vector2 startPosition;
        [SerializeField] private Vector2 endPosition;

        [SerializeField] private Vector2 startSize;
        [SerializeField] private Vector2 endSize;

        [SerializeField] private RectTransform target;

        protected abstract IEnumerator BeginShow();
        protected abstract IEnumerator EndShow();
        
        public IEnumerator Show(float duration)
        {
            yield return BeginShow();

            target.anchoredPosition = startPosition;
            target.sizeDelta = startSize;
            
            for (var timer = 0f; timer <= duration; timer += Time.deltaTime)
            {
                var progress = timer / duration;

                var sizeProgress = sizeCurve.Evaluate(progress);
                target.sizeDelta = Vector2.Lerp(startSize, endSize, sizeProgress);

                var positionProgress = positionCurve.Evaluate(progress);
                target.anchoredPosition = Vector2.Lerp(startPosition, endPosition, positionProgress);

                yield return new WaitForEndOfFrame();
            }
            
            yield return EndShow();
        }
    }
}