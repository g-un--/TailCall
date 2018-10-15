using System;

namespace TailRecursive
{
    public class SyncTailAction<T>
    {
        public Action<Action<T>, T> Action { get; }

        private SyncTailAction(Action<Action<T>, T> action)
        {
            Action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public static SyncTailAction<T> As(Action<Action<T>, T> action)
        {
            return new SyncTailAction<T>(action);
        }

        public void Run(T item)
        {
            T newItem = item;
            bool completed = false;
            while (!completed)
            {
                completed = true;
                Action(p =>
                {
                    completed = false;
                    newItem = p;
                }, newItem);
            }
        }
    }
}
