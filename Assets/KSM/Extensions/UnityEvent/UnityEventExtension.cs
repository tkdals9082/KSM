namespace KSM.Utility
{
    using CEC.ExposedUnityEvents.Internals;
    using System.Collections;
    using System.Reflection;
    using UnityEngine.Events;

    public static class UnityEventExtension
    {
        private static FieldInfo callsField;
        private static FieldInfo runtimeCallsField;

        private static MethodInfo findMethod;
        private const string callsFieldName = "m_Calls";
        private const string runtimeCallsFieldName = "m_RuntimeCalls";
        private const string findMethodName = "Find";

        static readonly object[] twoArgs = new object[2];

        #region IsAlreadyAdded

        public static bool IsAlreadyAdded(this UnityEvent e, UnityAction action)
        {
            if (callsField == null) callsField = typeof(UnityEventBase).GetField(callsFieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            var callsObj = callsField.GetValue(e);

            if (runtimeCallsField == null) runtimeCallsField = callsObj.GetType().GetField(runtimeCallsFieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            var runtimeCalls = runtimeCallsField.GetValue(callsObj);

            twoArgs[0] = action.Target;
            twoArgs[1] = action.Method;

            foreach (var call in runtimeCalls as IEnumerable)
            {
                /*if (findMethod == null)*/ findMethod = call.GetType().GetMethod(findMethodName, BindingFlags.Public | BindingFlags.Instance);
                bool isFind = (bool)findMethod.Invoke(call, twoArgs);
                if (isFind) return true;
            }

            return false;
        }

        public static bool IsAlreadyAdded<T0>(this UnityEvent<T0> e, UnityAction<T0> action)
        {
            if (callsField == null) callsField = typeof(UnityEventBase).GetField(callsFieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            var callsObj = callsField.GetValue(e);

            if (runtimeCallsField == null) runtimeCallsField = callsObj.GetType().GetField(runtimeCallsFieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            var runtimeCalls = runtimeCallsField.GetValue(callsObj);

            twoArgs[0] = action.Target;
            twoArgs[1] = action.Method;

            foreach (var call in runtimeCalls as IEnumerable)
            {
                /*if (findMethod == null)*/ findMethod = call.GetType().GetMethod(findMethodName, BindingFlags.Public | BindingFlags.Instance);
                bool isFind = (bool)findMethod.Invoke(call, twoArgs);
                if (isFind) return true;
            }

            return false;
        }

        public static bool IsAlreadyAdded<T0,T1>(this UnityEvent<T0,T1> e, UnityAction<T0,T1> action)
        {
            if (callsField == null) callsField = typeof(UnityEventBase).GetField(callsFieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            var callsObj = callsField.GetValue(e);

            if (runtimeCallsField == null) runtimeCallsField = callsObj.GetType().GetField(runtimeCallsFieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            var runtimeCalls = runtimeCallsField.GetValue(callsObj);

            twoArgs[0] = action.Target;
            twoArgs[1] = action.Method;

            foreach (var call in runtimeCalls as IEnumerable)
            {
                /*if (findMethod == null)*/ findMethod = call.GetType().GetMethod(findMethodName, BindingFlags.Public | BindingFlags.Instance);
                bool isFind = (bool)findMethod.Invoke(call, twoArgs);
                if (isFind) return true;
            }

            return false;
        }

        public static bool IsAlreadyAdded<T0,T1,T2>(this UnityEvent<T0,T1,T2> e, UnityAction<T0,T1,T2> action)
        {
            if (callsField == null) callsField = typeof(UnityEventBase).GetField(callsFieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            var callsObj = callsField.GetValue(e);

            if (runtimeCallsField == null) runtimeCallsField = callsObj.GetType().GetField(runtimeCallsFieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            var runtimeCalls = runtimeCallsField.GetValue(callsObj);

            twoArgs[0] = action.Target;
            twoArgs[1] = action.Method;

            foreach (var call in runtimeCalls as IEnumerable)
            {
                /*if (findMethod == null)*/ findMethod = call.GetType().GetMethod(findMethodName, BindingFlags.Public | BindingFlags.Instance);
                bool isFind = (bool)findMethod.Invoke(call, twoArgs);
                if (isFind) return true;
            }

            return false;
        }

        #endregion

        #region Add / Remove Persistent listeners

        public static void AddPersistentListener(this UnityEvent e, UnityAction action)
        {
            object persistentCallsObject = null;
            if (!UnityEventReflection.TryAccessPersistentCalls(e, ref persistentCallsObject)) return;
            int index = e.GetPersistentEventCount();
            UnityEventReflection.AddPersistentListner(persistentCallsObject, index, action.Target as UnityEngine.Object, action.Method);
        }

        public static void AddPersistentListener<T0>(this UnityEvent<T0> e, UnityAction<T0> action)
        {
            object persistentCallsObject = null;
            if (!UnityEventReflection.TryAccessPersistentCalls(e, ref persistentCallsObject)) return;
            int index = e.GetPersistentEventCount();
            UnityEventReflection.AddPersistentListner(persistentCallsObject, index, action.Target as UnityEngine.Object, action.Method);
        }

        public static void AddPersistentListener<T0,T1>(this UnityEvent<T0,T1> e, UnityAction<T0,T1> action)
        {
            object persistentCallsObject = null;
            if (!UnityEventReflection.TryAccessPersistentCalls(e, ref persistentCallsObject)) return;
            int index = e.GetPersistentEventCount();
            UnityEventReflection.AddPersistentListner(persistentCallsObject, index, action.Target as UnityEngine.Object, action.Method);
        }

        public static void AddPersistentListener<T0,T1,T2>(this UnityEvent<T0,T1,T2> e, UnityAction<T0,T1,T2> action)
        {
            object persistentCallsObject = null;
            if (!UnityEventReflection.TryAccessPersistentCalls(e, ref persistentCallsObject)) return;
            int index = e.GetPersistentEventCount();
            UnityEventReflection.AddPersistentListner(persistentCallsObject, index, action.Target as UnityEngine.Object, action.Method);
        }

        public static void AddPersistentListener<T0,T1,T2,T3>(this UnityEvent<T0,T1,T2,T3> e, UnityAction<T0,T1,T2,T3> action)
        {
            object persistentCallsObject = null;
            if (!UnityEventReflection.TryAccessPersistentCalls(e, ref persistentCallsObject)) return;
            int index = e.GetPersistentEventCount();
            UnityEventReflection.AddPersistentListner(persistentCallsObject, index, action.Target as UnityEngine.Object, action.Method);
        }

        #endregion

    }
}