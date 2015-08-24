using System;
using System.Reflection;

namespace MoreBeautification
{
    public static class ReflectionUtils
    {
        /// <summary>
        /// Uses reflection to get the field value from an object.
        /// https://stackoverflow.com/a/3303182/1011428
        /// </summary>
        ///
        /// <param name="type">The instance type.</param>
        /// <param name="instance">The instance object.</param>
        /// <param name="fieldName">The field's name which is to be fetched.</param>
        ///
        /// <returns>The field value from the object.</returns>
        public static FieldInfo GetInstanceField(Type type, string fieldName)
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            return type.GetField(fieldName, bindFlags);
        }

        public static MethodInfo GetInstanceMethod(Type type, string methodName)
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            return type.GetMethod(methodName, bindFlags);
        }

        public static object InvokeInstanceMethod(MethodInfo method, object instance, params object[] args)
        {
            return method.Invoke(instance, args);
        }
    }
}

