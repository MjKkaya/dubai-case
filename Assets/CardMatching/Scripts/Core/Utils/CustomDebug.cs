using System.Diagnostics;
using Debug = UnityEngine.Debug;


namespace CardMatching.Core.Utils
{
    public static class CustomDebug
    {
        [Conditional("DEBUG_LOG")]
        public static void Log(string logStr)
        {
            Debug.Log(logStr);
        }
    }
}