using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace VRising.WebServer.Hosting
{
    public class TaskRunner : MonoBehaviour
    {
        public static ConcurrentQueue<RisingTask> TaskQueue = new();
        public static ConcurrentDictionary<Guid, RisingTaskResult> TaskResults = new();

        public void Update()
        {
            if (!TaskQueue.TryDequeue(out var task))
            {
                return;
            }

            object result;
            try
            {
                result = task.ResultFunction.Invoke();
            }
            catch (Exception e)
            {
                TaskResults[task.TaskId] = new RisingTaskResult { Exception = e };
                return;
            }

            TaskResults[task.TaskId] = new RisingTaskResult { Result = result };
        }
    }

    public class RisingTask
    {
        public Guid TaskId { get; set; } = Guid.NewGuid();

        public Func<object> ResultFunction { get; set; }
    }

    public class RisingTaskResult
    {
        public object Result { get; set; }
        public Exception Exception { get; set; }
    }
}
