using System.Collections;
using UnityEngine;

namespace Game.Durak.UI
{
    public abstract class MessagePopupBase<T>
        : MonoBehaviour
    {
        [SerializeField] private Vector2 initialSize;
        [SerializeField] private Vector2 initialPosition;
        [SerializeField] private Vector2 targetSize;
        [SerializeField] private Vector2 targetPosition;
        [SerializeField] private Vector2 hideSize;
        [SerializeField] private Vector2 hidePosition;
            
        [SerializeField] private float showDuration;
        [SerializeField] private AnimationCurve showSizeCurve;
        [SerializeField] private AnimationCurve showPositionCurve;
        
        [SerializeField] private float visibilityDuration;

        [SerializeField] private float hideDuration;
        [SerializeField] private AnimationCurve hideSizeCurve;
        [SerializeField] private AnimationCurve hidePositionCurve;

        [SerializeField] private RectTransform target;

        protected abstract void BeforeShow(T value);
        protected virtual void AfterShow() {}
        
        public IEnumerator Show(T value)
        {
            BeforeShow(value);
            
            target.sizeDelta = initialSize;
            target.anchoredPosition = initialPosition;
            
            for (var time = 0f; time <= showDuration; time += Time.deltaTime)
            {
                var progress = time / hideDuration;
                
                target.sizeDelta = Vector2.Lerp(initialSize, targetSize, showSizeCurve.Evaluate(progress));
                target.anchoredPosition = Vector2.Lerp(initialPosition, targetPosition, showPositionCurve.Evaluate(progress));
            
                yield return new WaitForEndOfFrame();    
            }

            for (var time = 0f; time <= visibilityDuration; time += Time.deltaTime)
            {
                yield return new WaitForEndOfFrame();
            }
            
            for (var time = 0f; time <= hideDuration; time += Time.deltaTime)
            {
                var progress = time / hideDuration;

                if (!target)
                    break;
                    
                target.sizeDelta = Vector2.Lerp(targetSize, hideSize, hideSizeCurve.Evaluate(progress));
                target.anchoredPosition = Vector2.Lerp(targetPosition, hidePosition, hidePositionCurve.Evaluate(progress));
            
                yield return new WaitForEndOfFrame();    
            }
            
            AfterShow();
        }
    }
}