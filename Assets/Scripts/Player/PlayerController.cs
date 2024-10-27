using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Camera MainCamera;
    [SerializeField]
    private Transform PlayerModel;
    public Transform MousePoint;
    [SerializeField]
    private Transform StartSmokeGrenade;
    [SerializeField]
    private GameObject SmokeGrenade;
    private Vector2 InputAction;
    [SerializeField]
    private Rigidbody Rigidbody;
    [SerializeField]
    private Slider HealthSliderLeftSlider;
    [SerializeField]
    private Slider HealthSliderRightSlider;
    [SerializeField]
    private Slider ArmorSliderLeftSlider;
    [SerializeField]
    private Slider ArmorSliderRightSlider;
    [SerializeField]
    private Weapon[] PlayerWeapons;
    [SerializeField]
    private int MaxHealth;
    [SerializeField]
    private int Health;
    private int HealthValue
    {
        get => Health;
        set
        {
            Health = value;
            HealthSliderLeftSlider.value = Health;
            HealthSliderRightSlider.value = Health;
        }
    }
    [SerializeField]
    private int MaxArmor;
    [SerializeField]
    private int Armor;
    private int ArmorValue
    {
        get => Armor;
        set
        {
            Armor = value;
            ArmorSliderLeftSlider.value = Armor;
            ArmorSliderRightSlider.value = Armor;
        }
    }
    private int WeaponSwitch;
    private int CurrentWeaponValue;
    private int CurrentWeapon
    {
        get => CurrentWeaponValue;
        set
        {
            CurrentWeaponValue = value;
        }
    }
    private float MouseScroll;
    [SerializeField]
    private float Speed;
    [SerializeField]
    public float ThrowHeight;

    void Start()
    {
        HealthSliderLeftSlider.maxValue = MaxHealth;
        HealthSliderRightSlider.maxValue = MaxHealth;
        HealthSliderLeftSlider.value = HealthValue;
        HealthSliderRightSlider.value = HealthValue;
        ArmorSliderLeftSlider.maxValue = MaxArmor;
        ArmorSliderRightSlider.maxValue = MaxArmor;
        ArmorSliderLeftSlider.value = ArmorValue;
        ArmorSliderRightSlider.value = ArmorValue;
    }

    void Update()
    {
        Movements();
        SelectWeapons();
        ThrowGrenade();
    }

    void Movements()
    {
        InputAction.x = Input.GetAxis("Horizontal");
        InputAction.y = Input.GetAxis("Vertical");
        Rigidbody.velocity = new Vector3(InputAction.x * Speed, Rigidbody.velocity.y, InputAction.y * Speed);
        RaycastHit RaycastHit;
        Ray RayMainCamera = MainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(RayMainCamera, out RaycastHit, Mathf.Infinity))
        {
            MousePoint.position = new Vector3(RaycastHit.point.x, transform.position.y, RaycastHit.point.z);
        }
        PlayerModel.LookAt(new Vector3(MousePoint.position.x, transform.position.y, MousePoint.position.z));
    }

    void SelectWeapons()
    {
        MouseScroll = Input.GetAxis("Mouse ScrollWheel");
        CurrentWeapon = WeaponSwitch;
        if (MouseScroll > 0)
        {
            if (WeaponSwitch >= PlayerWeapons.Length - 1)
            {
                WeaponSwitch = 0;
            }
            else
            {
                WeaponSwitch++;
            }
        }
        else if (MouseScroll < 0)
        {
            if (WeaponSwitch <= 0)
            {
                WeaponSwitch = PlayerWeapons.Length - 1;
            }
            else
            {
                WeaponSwitch--;
            }
        }
        if (CurrentWeapon != WeaponSwitch)
        {
            for (int i = 0; i < PlayerWeapons.Length; i++)
            {
                if (i == WeaponSwitch && PlayerWeapons[i].IsHavePlayer == true)
                {
                    PlayerWeapons[i].PlayerWeaponObject.SetActive(true);
                }
                else
                {
                    PlayerWeapons[i].PlayerWeaponObject.SetActive(false);
                }
            }
        }
    }

    void PickUpWeapons(Collider Collider)
    {
        if (Collider.tag == "Weapon")
        {
            for (int i = 0; i < PlayerWeapons.Length; i++)
            {
                if (Collider.name == PlayerWeapons[i].PlayerWeaponObject.name && PlayerWeapons[i].GunController != null && PlayerWeapons[i].IsHavePlayer == false)
                {
                    PlayerWeapons[i].IsHavePlayer = true;
                    PlayerWeapons[i].PlayerWeaponObject.SetActive(true);
                    Destroy(Collider.gameObject);
                }
                else if (Collider.name == PlayerWeapons[i].PlayerWeaponObject.name && PlayerWeapons[i].GunController != null && PlayerWeapons[i].IsHavePlayer == true)
                {
                    int RandomMagazine = UnityEngine.Random.Range(1, 3);
                    PlayerWeapons[i].GunController.Magazine += RandomMagazine;
                    Destroy(Collider.gameObject);
                }
            }
        }
    }

    void PickUpConsumables(Collider Collider)
    {
        if (Collider.tag == "Consumables")
        {

        }
    }

    public void GetHit(int Damage)
    {
        if (ArmorValue > 0 && Health > 0)
        {
            ArmorValue -= Damage;
        }
        else if (ArmorValue <= 0 && Health > 0)
        {
            HealthValue -= Damage;
        }
        else if (HealthValue <= 0)
        {
            Destroy(gameObject);
        }
    }

    void ThrowGrenade()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject SpawnSmoke = Instantiate(SmokeGrenade, StartSmokeGrenade.position, Quaternion.identity);
            Rigidbody Rigidbody = SpawnSmoke.GetComponentInChildren<Rigidbody>();
            ThrowToTarget(Rigidbody, StartSmokeGrenade.position);
        }
    }

    void ThrowToTarget(Rigidbody Rigidbody, Vector3 TransformPoint)
    {
        Vector3 Direction = MousePoint.position - TransformPoint;
        Vector3 DirectionXZ = new Vector3(Direction.x, 0, Direction.z);
        float Time = Mathf.Sqrt(2 * ThrowHeight / 9.81f) + Mathf.Sqrt(2 * (ThrowHeight - (TransformPoint.y - MousePoint.position.y)) / 9.81f);
        Vector3 VelocityXZ = DirectionXZ / Time;
        float VelocityY = 9.81f * Time / 2;
        Vector3 InitialVelocity = VelocityXZ + Vector3.up * VelocityY;
        Rigidbody.velocity = InitialVelocity;
    }

    void OnTriggerEnter(Collider Collider)
    {
        PickUpWeapons(Collider);
        PickUpConsumables(Collider);
    }
}

[Serializable]
public class Weapon
{
    public GameObject PlayerWeaponObject;
    public PlayerGunController GunController;
    public bool IsHavePlayer;
}
