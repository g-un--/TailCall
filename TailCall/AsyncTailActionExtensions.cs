using System;
using System.Threading.Tasks;

namespace TailRecursive
{
    public static class AsyncTailActionExtensions
    {
        public static AsyncTailAction<T> InterceptWith<T>(this AsyncTailAction<T> tailAction, Func<Func<T, Task>, T, Task> interceptor)
        {
            return AsyncTailAction<T>.As((me, item) =>
            {
                return interceptor(t => tailAction.Action(me, t), item);
            });
        }

        public static AsyncTailAction<T> InterceptWith<T>(this AsyncTailAction<T> tailAction, Action<T> action)
        {
            return tailAction.InterceptWith((me, item) =>
            {
                action(item);
                return me(item);
            });
        }

        public static AsyncTailAction<T> InterceptWith<T>(this AsyncTailAction<T> tailAction, Func<T, Task> asyncAction)
        {
            return tailAction.InterceptWith((me, item) =>
            {
                return asyncAction(item).ContinueWith(_ => me(item)).Unwrap();
            });
        }

        public static Func<T, Task> ToAction<T>(this AsyncTailAction<T> tailAction)
        {
            return new Func<T, Task>(t => tailAction.Run(t));
        }
    }
}
