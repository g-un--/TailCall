using System;

namespace TailRecursive
{
    public static class SyncTailActionExtensions
    {
        public static SyncTailAction<T> InterceptWith<T>(this SyncTailAction<T> tailAction, Action<Action<T>, T> interceptor)
        {
            return SyncTailAction<T>.As((me, item) =>
            {
                interceptor(t => tailAction.Action(me, t), item);
            });
        }

        public static SyncTailAction<T> InterceptWith<T>(this SyncTailAction<T> tailAction, Action<T> action)
        {
            return tailAction.InterceptWith((me, item) =>
            {
                action(item);
                me(item);
            });
        }

        public static Action<T> ToAction<T>(this SyncTailAction<T> tailAction)
        {
            return new Action<T>(t => tailAction.Run(t));
        }
    }
}
