using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace VRising.WebServer.Hosting
{
    public class TaskRunner : MonoBehaviour
    {
        public static ConcurrentQueue<RisingTask> TaskQueue = new();
        public static ConcurrentDictionary<Guid, object> TaskResults = new();

        public void Update()
        {
            if (TaskQueue.TryDequeue(out var task))
            {
                TaskResults[task.TaskId] = task.ResultFunction.Invoke();
            }
        }
    }

    public class RisingTask
    {
        public Guid TaskId { get; set; } = Guid.NewGuid();

        public Func<object> ResultFunction { get; set; }
    }
}
