using UnityEngine;

[CreateAssetMenu(fileName = "GunSettings", menuName = "ProjectOne/EnemyGunSettings")]
public class EnemyGunSettings : ScriptableObject
{
    public GameObject Bullet;
    public int Damage;
    public float TimeNextShoot;
    public float ShootForce;
    public float TimeDestroyBullet;
    public float SpreadX;
    public float SpreadY;
}
