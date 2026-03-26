using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public static class ScriptableObjectDataBase
{
    private static readonly Dictionary<Type, Dictionary<string, Object>> SO_DATABASE = new();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Initialize()
    {
        Debug.Log("Initializing ScriptableObjectDataBase...");

        SO_DATABASE.Clear();
        Register<CharacterTemplateSO>();
    }

    private static void Register<T>() where T : Object
    {
        var type = typeof(T);

        if (SO_DATABASE.ContainsKey(type))
        {
            Debug.LogWarning($"ScriptableObject with name {type.Name} already exists in database.");
            return;
        }

        SO_DATABASE[type] = new Dictionary<string, Object>();

        T[] templates = Resources.LoadAll<T>("");
        foreach (var template in templates)
        {
            SO_DATABASE[type][template.name] = template;
        }

        Debug.Log($"[DATABASE] Loaded {templates.Length} {type.Name}(s)");
    }

    /// <summary>
    /// Get an object by name referenced in resources files of the project.
    /// </summary>
    /// <param name="name">The name of the desired object.</param>
    /// <typeparam name="T">The type of the desired object. Type must be an <see cref="Object"/></typeparam>
    /// <returns>The object found, null if nothing is found.</returns>
    public static T Get<T>(string name) where T : Object
    {
        var type = typeof(T);

        if (SO_DATABASE.TryGetValue(type, out var typeDictionary))
        {
            if (typeDictionary.TryGetValue(name, out var scriptableObject))
            {
                return scriptableObject as T;
            }
        }

        Debug.LogError("Unable to find a scriptable object with name:" + name + "of type" + type);
        return null;
    }
}