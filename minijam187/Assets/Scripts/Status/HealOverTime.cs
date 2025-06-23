using UnityEngine;

public class HealOverTime : MonoBehaviour
{
    public bool IsAffected => duration > 0;
    public GameObject prefab;

    public HealEffect HealEffect;

    public bool Done => done;

    private int amount;
    private int duration;
    private GameObject buffImage;
    private bool done = false;

    public void Mend(int amount, int duration)
    {
        if (this.duration > 0)
        {
            int total = this.amount * this.duration;
            amount = total > amount * duration ? this.amount : amount;
            duration = total > amount * duration ? this.duration : duration;
        }
        else
        {
            buffImage = Instantiate(prefab, this.transform);
        }
        buffImage.GetComponent<HoverAmount>().Populate(amount, duration);
        this.amount = amount;
        this.duration = duration;
    }

    public void Remove()
    {
        if (duration > 0)
        {
            Destroy(buffImage);
        }
        amount = 0;
        duration = 0;
    }

    public void Tick(IEffected effected)
    {
        done = false;
        HealEffect.ApplyRoundBased(effected, amount, () => done = true);
        duration--;

        buffImage.GetComponent<HoverAmount>().Populate(amount, duration);

        if (duration <= 0)
        {
            Destroy(buffImage);
        }
    }
}
