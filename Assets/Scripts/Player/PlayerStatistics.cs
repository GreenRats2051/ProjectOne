using UnityEngine;
using UnityEngine.UI;

public class PlayerStatistics : MonoBehaviour
{
    [SerializeField]
    private Slider healthSliderLeftSlider;
    [SerializeField]
    private Slider healthSliderRightSlider;
    [SerializeField]
    private Slider armorSliderLeftSlider;
    [SerializeField]
    private Slider armorSliderRightSlider;
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
            healthSliderLeftSlider.value = health;
            healthSliderRightSlider.value = health;
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
            armorSliderLeftSlider.value = armor;
            armorSliderRightSlider.value = armor;
        }
    }

    void Start()
    {
        healthValue = maxHealth;
        armorValue = maxArmor;
        healthSliderLeftSlider.maxValue = maxHealth;
        healthSliderRightSlider.maxValue = maxHealth;
        armorSliderLeftSlider.maxValue = maxArmor;
        armorSliderRightSlider.maxValue = maxArmor;
        healthSliderLeftSlider.value = health;
        healthSliderRightSlider.value = health;
        armorSliderLeftSlider.value = armor;
        armorSliderRightSlider.value = armor;
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
