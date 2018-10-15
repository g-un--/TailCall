using System;

namespace TailRecursive
{
    public static class SyncTailFuncExtensions
    {
        public static SyncTailFunc<T, R> InterceptWith<T,R>(this SyncTailFunc<T,R> tailFunc, Func<Func<T, R>, T, R> interceptor)
        {
            return SyncTailFunc<T,R>.As((me, item) =>
            {
                return interceptor(t => tailFunc.Func(me, t), item);
            });
        }

        public static SyncTailFunc<T, R> InterceptWith<T,R>(this SyncTailFunc<T,R> tailFunc, Action<T> action)
        {
            return tailFunc.InterceptWith((me, item) =>
            {
                action(item);
                return me(item);
            });
        }

        public static Func<T, R> ToFunc<T,R>(this SyncTailFunc<T,R> tailFunc)
        {
            return new Func<T, R>(t => tailFunc.Run(t));
        }
    }
}
