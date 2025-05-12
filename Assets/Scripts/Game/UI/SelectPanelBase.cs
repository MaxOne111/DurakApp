using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Game.UI
{
    public abstract class SelectPanelBase<T>
        : MonoBehaviour
    {
        [SerializeField] private GameObject root;

        [SerializeField] private LayoutGroup groupToUpdate;
        
        protected abstract IDictionary<Button, T> Fill(out Action clear, out Action<T> select);

        public async Task<T> Select(CancellationToken cancellationToken)
        {
            root.SetActive(true);

            var source = new TaskCompletionSource<T>(cancellationToken);
            var token = new CompositeDisposable();
            
            var buttons = Fill(out var clear, out var select);
            cancellationToken.Register(Clear);
            
            foreach (var button in buttons.Keys)
            {
                token.Add(button.onClick.AddDisposableListener(Listen));
                
                void Listen()
                {
                    select.Invoke(buttons[button]);
                    Clear();
                    
                    token.Dispose();
                    source.SetResult(buttons[button]);
                }
            }

            StartCoroutine(UpdateGroup());

            return await source.Task;

            void Clear()
            {
                clear.Invoke();
                
                root.SetActive(false);
            }

            IEnumerator UpdateGroup()
            {
                yield return new WaitForEndOfFrame();

                groupToUpdate.enabled = false;

                groupToUpdate.CalculateLayoutInputVertical();

                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) groupToUpdate.transform);

                groupToUpdate.enabled = true;
            }
        }
    }
}