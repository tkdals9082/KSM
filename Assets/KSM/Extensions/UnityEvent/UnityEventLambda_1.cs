namespace KSM.Utility
{
    using Neleus.LambdaCompare;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using UnityEngine.Events;

    public static class UnityEventLambda<T0>
    {
        private static MethodInfo addListenerMethod = null;
        private static MethodInfo removeListenerMethod = null;

        private const string addListenerMethodName = "AddListener";
        private const string removeListenerMethodName = "RemoveListener";

        private static Dictionary<int, (List<Expression<Action<T0>>> expressions, List<Action<T0>> actions)> dict = new Dictionary<int, (List<Expression<Action<T0>>> expressions, List<Action<T0>> actions)>();

        #region Lambda Extension

        // Although extension method is looks better and easy to use,
        // implicit conversion to UnityAction has higher priority than Expression<Action>.
        public static void AddListener(/*this*/ UnityEvent<T0> unityEvent, Expression<Action<T0>> expression)
        {
            Action<T0> action = expression.Compile();

            int hash = unityEvent.GetHashCode();

            if (dict.ContainsKey(hash))
            {
                dict[hash].expressions.Add(expression);
                dict[hash].actions.Add(action);
            }
            else
            {
                List<Expression<Action<T0>>> expressions = new List<Expression<Action<T0>>>() { expression };
                List<Action<T0>> actions = new List<Action<T0>>() { action };

                dict.Add(hash, (expressions, actions));
            }

            if (addListenerMethod == null) addListenerMethod = typeof(UnityEventBase).GetMethod(addListenerMethodName, BindingFlags.NonPublic | BindingFlags.Instance);
            object[] param = new object[] { action.Target, action.Method };
            addListenerMethod.Invoke(unityEvent, param);
        }

        public static void RemoveListener(/*this*/ UnityEvent<T0> unityEvent, Expression<Action<T0>> expression)
        {
            int hash = unityEvent.GetHashCode();
            if (!dict.ContainsKey(hash)) return;

            // Find index of same expression
            var expressions = dict[hash].expressions;
            int removeIdx = expressions.FindIndex(exp => Lambda.Eq(exp, expression));
            if (removeIdx == -1) return;

            // Call RemoveListener(object target, MethodInfo method) with the same index of actions
            var actions = dict[hash].actions;

            if (removeListenerMethod == null) removeListenerMethod = typeof(UnityEventBase).GetMethod(removeListenerMethodName, BindingFlags.NonPublic | BindingFlags.Instance);
            object[] param = new object[] { actions[removeIdx].Target, actions[removeIdx].Method };
            removeListenerMethod.Invoke(unityEvent, param);

            expressions.RemoveAt(removeIdx);
            actions.RemoveAt(removeIdx);
        }

        #endregion
    }
}