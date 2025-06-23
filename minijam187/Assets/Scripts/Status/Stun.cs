using UnityEngine;

public class Stun : MonoBehaviour
{
    public bool IsStunned => duration > 0;
    public GameObject prefab;

    public bool Done => done;

    private int duration;
    private GameObject debuffImage;
    private bool done = false;
    public void Bonk(int duration)
    {
        if (this.duration <= 0)
        {
            debuffImage = Instantiate(prefab, this.transform);
        }
        this.duration += duration + 1;
    }

    public void Cleanse()
    {
        if (duration > 0)
        {
            Destroy(debuffImage);
        }
        duration = 0;
    }

    public void Tick()
    {
        done = false;
        duration--;

        if (duration <= 0)
        {
            Destroy(debuffImage);
        }
    }
}

