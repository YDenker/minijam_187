using System;
using UnityEngine;

[Serializable]
public abstract class Effect : ScriptableObject
{
    public virtual void Apply(IEffected effected)
    {
        Debug.LogWarning("Did Nothing");
    }
}
