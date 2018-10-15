using System;
using System.Threading.Tasks;

namespace TailRecursive
{
    public class AsyncTailAction<T>
    {
        public Func<Func<T, Task>, T, Task> Action { get; }

        private AsyncTailAction(Func<Func<T, Task>, T, Task> action)
        {
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public static AsyncTailAction<T> As(Func<Func<T, Task>, T, Task> action)
        {
            return new AsyncTailAction<T>(action);
        }

        public async Task Run(T item)
        {
            T newItem = item;
            bool completed = false;
            while (!completed)
            {
                completed = true;
                await Action(p =>
                {
                    completed = false;
                    newItem = p;
                    return Task.CompletedTask;
                }, newItem);
            }
        }
    }
}
