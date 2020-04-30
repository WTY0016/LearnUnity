using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Log
{
    private static readonly string OutputPath = Path.Combine(Application.dataPath, "log.txt");
    private static Log _instance;
    private static Log Instance => _instance ?? (_instance = new Log());
    private readonly StreamWriter _fileStream;
    private Log()
    {
        _fileStream = File.AppendText(OutputPath);
    }

    ~Log()
    {
        _fileStream.Close();        
    }
    private void LogConsole(object msg) => Debug.Log(msg);

    private void LogFile(object msg)
    {
        _fileStream.Write($"{DateTime.Now} {msg} \n");
    }
    
    public static void L(object o)
    {
        StackTrace trace = new StackTrace(true);
        foreach (var frame in trace.GetFrames())
        {
            Debug.Log(frame.GetMethod());
        }
    }

    public static void F(object o)
    {
        Instance.LogFile(o);
    }
}
