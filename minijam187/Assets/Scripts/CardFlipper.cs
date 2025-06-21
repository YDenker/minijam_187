using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class CardFlipper
{
    public float duration = 1f;
    public AnimationCurve flipCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public IEnumerator FlipCoroutine(RectTransform targetObject, Action halfwayCallback, Action endCallback)
    {
        float time = 0f;
        float halfDuration = duration * 0.5f;
        Vector3 currentScale = targetObject.localScale;

        while (time < halfDuration)
        {
            float t = time / halfDuration;
            float curvedT = flipCurve.Evaluate(t);

            float scaleX = Mathf.Lerp(currentScale.x, 0.01f, curvedT);
            targetObject.localScale = new Vector3(scaleX,currentScale.y,currentScale.z);

            time += Time.deltaTime;
            yield return null;

        }

        halfwayCallback?.Invoke();

        time = 0f;
        while (time < halfDuration)
        {
            float t = time / halfDuration;
            float curvedT = flipCurve.Evaluate(t);

            float scaleX = Mathf.Lerp(0.01f, currentScale.x, curvedT);
            targetObject.localScale = new Vector3(scaleX, currentScale.y, currentScale.z);

            time += Time.deltaTime;
            yield return null;
        }

        targetObject.localScale = currentScale;

        endCallback?.Invoke();
    }
}
