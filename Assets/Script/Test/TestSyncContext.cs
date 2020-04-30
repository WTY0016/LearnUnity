using System.Threading;
using System.Threading.Tasks;
using Script.AlogLearn;
using UnityEngine;

public class TestSyncContext
{
    public void Test()
    {
        var context = SynchronizationContext.Current;
        Debug.Log($"MainThread {Thread.CurrentThread.ManagedThreadId}");
        Task.Run(() =>
        {
            Debug.Log($"TaskThread{Thread.CurrentThread.ManagedThreadId}");
            context.Post(Event, "");
            context.Post(Event, "");
            context.Post(Event, "");
            context.Post(Event, "");
            context.Post(Event, "");

        });

    }

    private void Event(object o)
    {
        Debug.Log($"EventThread {Thread.CurrentThread.ManagedThreadId}");
    }
    [AutoRun]
    public void Test2()
    {
        var context = new SynchronizationContext();
        Debug.Log($"MainThread {Thread.CurrentThread.ManagedThreadId}");
        Task.Run(() =>
        {
            Debug.Log($"new ThreadId {Thread.CurrentThread.ManagedThreadId}"); 
            context.Post(Event, 0);
            context.Post(Event, 0);
            context.Post(Event, 0);
            context.Post(Event, 0);
        });
    }
}