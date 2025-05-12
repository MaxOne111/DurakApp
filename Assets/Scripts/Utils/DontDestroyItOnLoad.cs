using UnityEngine;

namespace Utils
{
    public sealed class DontDestroyItOnLoad
        : MonoBehaviour
    {
        [SerializeField] private Object target;
        
        private void Awake()
        {
            DontDestroyOnLoad(target);
        }
    }
}