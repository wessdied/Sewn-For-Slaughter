using UnityEngine;

public class LogFilter : MonoBehaviour
{
    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Warning && logString.Contains("Color primaries 0 is unknown"))
        {
            // Ignore this warning
            return;
        }
        else
        {
            Debug.unityLogger.Log(type, logString);
        }
    }
}
