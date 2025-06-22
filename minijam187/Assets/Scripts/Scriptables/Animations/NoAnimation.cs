using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "NoAnimation", menuName = "Scriptable Objects/Animation/No Animation")]
public class NoAnimation : SpellAnimation
{
    public override IEnumerator Play(Vector2 origin, Vector2 target, Action beginCallback, Action effectCallback, Action endCallback)
    {
        beginCallback?.Invoke();
        yield return new WaitForSeconds(duration);
        effectCallback?.Invoke();
        yield return new WaitForEndOfFrame();
        endCallback?.Invoke();
    }
}