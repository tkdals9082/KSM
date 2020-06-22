namespace KSM.Utility
{
    using Neleus.LambdaCompare;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using UnityEngine.Events;

    public static class UnityEventLambda
    {
        private static MethodInfo addListenerMethod = null;
        private static MethodInfo removeListenerMethod = null;

        private const string addListenerMethodName = "AddListener";
        private const string removeListenerMethodName = "RemoveListener";

        private static Dictionary<int, (List<Expression<Action>> expressions, List<Action> actions)> dict = new Dictionary<int, (List<Expression<Action>> expressions, List<Action> actions)>();

        #region Lambda Extension

        // i can't use extension because there should be a generic dictionary.
        // how can i use both extension method and generic field??
        public static void AddListener(/*this*/ UnityEvent unityEvent, Expression<Action> expression)
        {
            Action action = expression.Compile();

            int hash = unityEvent.GetHashCode();

            if (dict.ContainsKey(hash))
            {
                dict[hash].expressions.Add(expression);
                dict[hash].actions.Add(action);
            }
            else
            {
                List<Expression<Action>> expressions = new List<Expression<Action>>() { expression };
                List<Action> actions = new List<Action>() { action };

                dict.Add(hash, (expressions, actions));
            }

            if (addListenerMethod == null) addListenerMethod = typeof(UnityEventBase).GetMethod(addListenerMethodName, BindingFlags.NonPublic | BindingFlags.Instance);
            object[] param = new object[] { action.Target, action.Method };
            addListenerMethod.Invoke(unityEvent, param);
        }

        public static void RemoveListener(/*this*/ UnityEvent unityEvent, Expression<Action> expression)
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

            if(expressions.Count == 0)
                dict.Remove(hash);
        }

        #endregion
    }
}