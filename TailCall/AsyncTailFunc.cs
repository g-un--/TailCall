using System;
using System.Threading.Tasks;

namespace TailRecursive
{
    public class AsyncTailFunc<T, R>
    {
        public Func<Func<T, Task<R>>, T, Task<R>> Func { get; }

        private AsyncTailFunc(Func<Func<T, Task<R>>, T, Task<R>> func)
        {
            Func = func ?? throw new ArgumentNullException(nameof(func));
        }

        public static AsyncTailFunc<T, R> As(Func<Func<T, Task<R>>, T, Task<R>> func)
        {
            return new AsyncTailFunc<T, R>(func);
        }

        public async Task<R> Run(T item)
        {
            T newItem = item;
            R result = default(R);
            Task<R> defaultResultTask = Task.FromResult(default(R));
            bool completed = false;
            while (!completed)
            {
                completed = true;
                result = await Func(p =>
                {
                    completed = false;
                    newItem = p;
                    return defaultResultTask;
                }, newItem);
            }
            return result;
        }
    }
}
