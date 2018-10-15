using System;

namespace TailRecursive
{
    public class SyncTailFunc<T, R>
    {
        public Func<Func<T, R>, T, R> Func { get; }

        private SyncTailFunc(Func<Func<T, R>, T, R> func)
        {
            Func = func ?? throw new ArgumentNullException(nameof(func));
        }

        public static SyncTailFunc<T, R> As(Func<Func<T, R>, T, R> func)
        {
            return new SyncTailFunc<T, R>(func);
        }

        public R Run(T item)
        {
            T newItem = item;
            R result = default(R);
            bool completed = false;
            while (!completed)
            {
                completed = true;
                result = Func(p =>
                {
                    completed = false;
                    newItem = p;
                    return result;
                }, newItem);
            }
            return result;
        }
    }
}
