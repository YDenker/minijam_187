using UnityEngine;

public class Poison : MonoBehaviour
{
    public bool IsPoisioned => amount > 0;
    public GameObject prefab;
    public DamageEffect DamageEffect;

    public bool Done => done;

    private int amount;
    private GameObject debuffImage;
    private bool done = false;
    public void Intoxicate(int amount)
    {
        if (this.amount <= 0)
        {
            debuffImage = Instantiate(prefab, this.transform);
        }
        debuffImage.GetComponent<HoverAmount>().Populate(amount, amount);
        this.amount += amount;
    }

    public void Cleanse()
    {
        if (amount > 0)
        {
            Destroy(debuffImage);
        }
        amount = 0;
    }

    public void Tick(IEffected effected)
    {
        done = false;
        DamageEffect.ApplyRoundBased(effected, amount, () => done = true);
        amount--;

        debuffImage.GetComponent<HoverAmount>().Populate(amount, amount);

        if (amount <= 0)
        {
            Destroy(debuffImage);
        }
    }
}