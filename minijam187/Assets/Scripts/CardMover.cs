using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class CardMover
{
    public float duration = 0.2f;
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0,0,1,1);

    public IEnumerator MoveCoroutine(RectTransform targetObject, Vector2 targetPos, float targetZAngle, Action endCallback)
    {
        Vector2 startPos = targetObject.anchoredPosition;
        float startZAngle = targetObject.localEulerAngles.z;

        if (startZAngle > 180f) startZAngle -= 360f;
        if (targetZAngle > 180f) targetZAngle -= 360f;


        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            float curvedT = moveCurve.Evaluate(t);

            targetObject.anchoredPosition = Vector2.Lerp(startPos, targetPos, curvedT);

            float zAngle = Mathf.Lerp(startZAngle, targetZAngle, curvedT);
            targetObject.localRotation = Quaternion.Euler(0, 0, zAngle);

            time += Time.deltaTime;
            yield return null;
        }

        targetObject.anchoredPosition = targetPos;
        endCallback?.Invoke();
    }
}
