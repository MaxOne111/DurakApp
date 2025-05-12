using System;
using Infrastructure.Enums;
using UnityEngine;

namespace Infrastructure
{
    public static class VerboseProvider
    {
        public static void Log(string message, EVerboseMode mode = EVerboseMode.Message, Func<Exception> exception = null)
        {
            exception ??= () => new ArgumentException(message);
            
            switch (mode)
            {
                case EVerboseMode.None:
                    return;
                
                case EVerboseMode.Message:
                    Debug.Log(message);
                    break;
                
                case EVerboseMode.Warning:
                    Debug.LogWarning(message);
                    break;
                
                case EVerboseMode.Error:
                    Debug.LogError(message);
                    break;
                
                case EVerboseMode.Exception:
                    throw exception.Invoke();
                    break;
                
                default:
                    throw new ArgumentException();
            }
        }
    }
}