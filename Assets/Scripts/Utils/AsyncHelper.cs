using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Utils
{
    public static class AsyncHelper
    {
        public static async Task WaitWhile(Func<bool> func)
        {
            while (func.Invoke())
            {
                await Task.Yield();
            }
        }
        public static async Task WaitUntil(Func<bool> func)
        {
            while (!func.Invoke())
            {
                await Task.Yield();
            }
        }
    }
}