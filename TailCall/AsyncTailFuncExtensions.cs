using System;
using System.Threading.Tasks;

namespace TailRecursive
{
    public static class AsyncTailFuncExtensions
    {
        public static AsyncTailFunc<T, R> InterceptWith<T, R>(this AsyncTailFunc<T, R> tailFunc, Func<Func<T, Task<R>>, T, Task<R>> interceptor)
        {
            return AsyncTailFunc<T, R>.As((me, item) =>
            {
                return interceptor(t => tailFunc.Func(me, t), item);
            });
        }

        public static AsyncTailFunc<T, R> InterceptWith<T, R>(this AsyncTailFunc<T, R> tailFunc, Action<T> action)
        {
            return tailFunc.InterceptWith((me, item) =>
            {
                action(item);
                return me(item);
            });
        }

        public static AsyncTailFunc<T, R> InterceptWith<T, R>(this AsyncTailFunc<T, R> tailFunc, Func<T, Task> asyncAction)
        {
            return tailFunc.InterceptWith((me, item) =>
            {
                return asyncAction(item).ContinueWith(_ => me(item)).Unwrap();
            });
        }

        public static Func<T, Task<R>> ToFunc<T, R>(this AsyncTailFunc<T, R> tailFunc)
        {
            return new Func<T, Task<R>>(t => tailFunc.Run(t));
        }
    }
}
