using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ProjectileAnimation", menuName = "Scriptable Objects/Animation/Projectile")]
public class Projectile : SpellAnimation
{
    [SerializeField] private Sprite[] projectiles; // sequence of a gif
    [SerializeField] private float frameRate = 10f;
    public override IEnumerator Play(Vector2 origin, Vector2 target, Action beginCallback, Action effectCallback, Action endCallback)
    {
        beginCallback?.Invoke();
        var projectileGO = Instantiate(prefab, GameManager.Instance.AnimationLayer);
        RectTransform rt = projectileGO.GetComponent<RectTransform>();
        Image img = projectileGO.GetComponent<Image>();

        img.sprite = projectiles.Length > 0 ? projectiles[0] : null;
        rt.anchoredPosition = origin;

        Vector2 dir = target - origin;
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
        rt.localRotation = Quaternion.Euler(0, 0, angle);

        float time = 0f;
        float frameTimer = 0f;
        int frameIndex = 0;

        while(time < duration)
        {
            float t = time / duration;
            Vector2 pos = Vector2.Lerp(origin, target, t);
            rt.anchoredPosition = pos;

            frameTimer += Time.deltaTime;
            if (frameTimer >= 1f / frameRate)
            {
                frameTimer -= 1f / frameRate;
                frameIndex = (frameIndex + 1) % projectiles.Length;
                img.sprite = projectiles[frameIndex];
            }
            time += Time.deltaTime;
            yield return null;
        }
        effectCallback?.Invoke();
        rt.anchoredPosition = target;
        Destroy(projectileGO);

        // Maybe particle effect

        endCallback?.Invoke();
    }
}
