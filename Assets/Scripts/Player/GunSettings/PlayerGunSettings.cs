using UnityEngine;

[CreateAssetMenu(fileName = "GunSettings", menuName = "ProjectOne/PlayerGunSettings")]
public class PlayerGunSettings : ScriptableObject
{
    public GameObject Bullet;
    public int Damage;
    public int MaxAmmo;
    public int MaxMagazine;
    public float TimeNextShoot;
    public float ShootForce;
    public float TimeDestroyBullet;
    public float SpreadX;
    public float SpreadY;
}
