using UnityEngine;
using UnityEngine.UI;

public class Burn : MonoBehaviour
{
    public bool IsBurning => duration > 0;
    public GameObject prefab;
    public DamageEffect DamageEffect;

    public bool Done => done;

    private int amount;
    private int duration;
    private GameObject debuffImage;
    private bool done = false;

    public void Ignite(int amount, int duration)
    {
        if (this.duration > 0)
        {
            int total = this.amount * this.duration;
            amount = total > amount * duration ? this.amount : amount;
            duration = total > amount * duration ? this.duration : duration;
        }
        else
        {
            debuffImage = Instantiate(prefab, this.transform);
        }
        debuffImage.GetComponent<HoverAmount>().Populate(amount, duration);
        this.amount = amount;
        this.duration = duration;
    }

    public void Cleanse()
    {
        if(duration > 0)
        {
            Destroy(debuffImage);
        }
        amount = 0;
        duration = 0;
    }

    public void Tick(IEffected effected)
    {
        done = false;
        DamageEffect.ApplyRoundBased(effected, amount, () => done = true);
        duration--;

        debuffImage.GetComponent<HoverAmount>().Populate(amount, duration);

        if (duration <= 0)
        {
            Destroy(debuffImage);
        }
    }
}
