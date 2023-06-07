using System;
using System.Reflection;
using UnityEngine;
using Logger = Outpath_Modding.GameConsole.Logger;

namespace Outpath_Modding.API.Extensions
{
    public static class ObjectExtension
    {
        public static void CopyProperties(this object target, object source)
        {
            Type type = target.GetType();

            if (type != source.GetType())
                return;

            foreach (PropertyInfo sourceProperty in type.GetProperties())
                type.GetProperty(sourceProperty.Name)?.SetValue(target, sourceProperty.GetValue(source, null), null);
        }

        public static T Clone<T>(this T scriptableObject) where T : ScriptableObject
        {
            if (scriptableObject == null)
            {
                Logger.Error($"ScriptableObject was null. Returning default {typeof(T)} object.");
                return (T)ScriptableObject.CreateInstance(typeof(T));
            }

            T instance = UnityEngine.Object.Instantiate(scriptableObject);
            instance.name = scriptableObject.name;
            return instance;
        }
    }
}
