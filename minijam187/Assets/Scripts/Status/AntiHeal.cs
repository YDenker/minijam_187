using UnityEngine;

public class AntiHeal : MonoBehaviour
{
    public bool IsAfflicted => duration > 0;
    public GameObject prefab;

    public bool Done => done;

    private int duration;
    private GameObject debuffImage;
    private bool done = false;
    public void Infest(int duration)
    {
        if (this.duration <= 0)
        {
            debuffImage = Instantiate(prefab, this.transform);
        }
        this.duration += duration;
    }

    public void Cleanse()
    {
        if (duration > 0)
        {
            Destroy(debuffImage);
        }
        duration = 0;
    }

    public void Tick(IEffected effected)
    {
        done = false;
        duration--;

        if (duration <= 0)
        {
            Destroy(debuffImage);
        }
    }
}
