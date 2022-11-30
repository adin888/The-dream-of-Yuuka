using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Core : MonoBehaviour
{
    private readonly List<CoreComponent> M_CoreComponents = new List<CoreComponent>();

    private void Awake()
    {
        
    }
    public void LogicUpdate()
    {
        foreach(CoreComponent component in M_CoreComponents)
        {
            component.LogicUpdate();
        }
    }
    public void AddComponent(CoreComponent component)
    {
        if(!M_CoreComponents.Contains(component))
        {
            M_CoreComponents.Add(component);
        }
    }
    public T GetCoreComponent<T>() where T : CoreComponent
    {
        var component = M_CoreComponents.OfType<T>().FirstOrDefault();

        if (component)
            return component;

        component = GetComponentInChildren<T>();

        if (component)
            return component;

        Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}");
        return null;
    }
    public T GetCoreComponent<T>(ref T value) where T : CoreComponent
    {
        value = GetCoreComponent<T>();
        return value;
    }
}
