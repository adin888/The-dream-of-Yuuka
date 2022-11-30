using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreComponent : MonoBehaviour, IlogicUpdate
{
    protected Core m_Core;

    protected virtual void Awake()
    {
        m_Core = transform.parent.GetComponent<Core>();

        if (m_Core == null) { Debug.LogError("There is no Core on the parent"); }
        m_Core.AddComponent(this);
    }
    public virtual void LogicUpdate() { }
}
