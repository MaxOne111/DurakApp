using System;
using System.Linq;
using UnityEngine;

namespace Game.Bootstrapping
{
    public sealed class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod]
        private static void Bootstrap()
        {
            var assembly = typeof(Bootstrapper).Assembly;
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                var methods = type.GetMethods();
                foreach (var method in methods)
                {
                    var isValid = method
                        .CustomAttributes
                        .Any(value => value.AttributeType == typeof(BootstrapMethodAttribute));

                    if (!isValid)
                    {
                        continue;
                    }

                    if (!method.IsStatic)
                    {
                        Debug.LogWarning(
                            $"Method \"{method.DeclaringType}.{method.Name}\" is not static so it can't be bootstrap method");
                        continue;
                    }

                    if (method.GetParameters().Length != 0)
                    {
                        Debug.LogWarning(
                            $"Method \"{method.DeclaringType}.{method.Name}\" has parameters so it can't be bootstrap method");
                        continue;
                    }

                    method.Invoke(null, Array.Empty<object>());
                }
            }
        }
    }
}