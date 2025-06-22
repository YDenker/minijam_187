using System;
using System.Collections;
using UnityEngine;
[Serializable]
public abstract class SpellAnimation : ScriptableObject
{
    public GameObject prefab;
    public float duration = 2f;
    public abstract IEnumerator Play(Vector2 origin, Vector2 target, Action beginCallback, Action effectCallback, Action endCallback);
}
