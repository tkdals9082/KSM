namespace KSM.Utility
{
    using System.Collections.Generic;
    
    public static class QueueExtensions
    {
        public static void EnqueueChunk<T>(this Queue<T> queue, IEnumerable<T> enumerable)
        {
            foreach(var data in enumerable)
            {
                queue.Enqueue(data);
            }
        }

        public static IEnumerable<T> DequeueChunk<T>(this Queue<T> queue, int chunkSize)
        {
            for (int i = 0; i < chunkSize && queue.Count > 0; i++)
            {
                yield return queue.Dequeue();
            }
        }
    }
}