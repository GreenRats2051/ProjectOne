using UnityEngine;
using UnityEngine.UI;

public class PlayerStatistics : MonoBehaviour
{
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private Slider armorSlider;
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int health;
    public int healthValue
    {
        get => health;
        set
        {
            health = value;
            healthSlider.value = health;
        }
    }
    [SerializeField]
    private int maxArmor;
    [SerializeField]
    private int armor;
    public int armorValue
    {
        get => armor;
        set
        {
            armor = value;
            armorSlider.value = armor;
        }
    }

    void Start()
    {
        healthValue = maxHealth;
        armorValue = maxArmor;
        healthSlider.maxValue = maxHealth;
        armorSlider.maxValue = maxArmor;
        healthSlider.value = healthValue;
        armorSlider.value = armor;
    }

    public void GetHit(int damage)
    {
        if (armorValue > 0 && health > 0)
        {
            armorValue -= damage;
        }
        else if (armorValue <= 0 && health > 0)
        {
            healthValue -= damage;
        }
        else if (healthValue <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Healing(int plusHealth)
    {
        if (healthValue > 0)
        {
            healthValue += plusHealth;
        }
    }
}
