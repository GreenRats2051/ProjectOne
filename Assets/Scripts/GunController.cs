using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject Bullet; //����
    public Transform StartShoot; //������ ��������
    public Vector3 TargetPoint; //����� ��������
    public Vector2 ForceShoot; //����������� ��������
    public PlayerController PlayerController; //PlayerController ������
    public float TimeShoot; //����� ��������
    public float TimeNextShoot; //����� ���������� �������� (������� � �������)
    public float ShootForce; //�������� ����
    public float SpreadX; //������� ���� �� ��� X
    public float SpreadY; //������� ���� �� ��� Y
    public float TimeDestroyBullet; //����� ����������� ����

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && Time.time >= TimeShoot)
        {
            TimeShoot = Time.time + 1 / TimeNextShoot;
            TargetPoint = new Vector3(PlayerController.Point.x, PlayerController.Point.y + 1f, PlayerController.Point.z);
            Vector3 DirWithoutSpread = TargetPoint - StartShoot.position;
            ForceShoot.x = Random.Range(-SpreadX, SpreadX);
            ForceShoot.y = Random.Range(-SpreadY, SpreadY);
            Vector3 DirWithSpread = DirWithoutSpread + new Vector3(ForceShoot.x, ForceShoot.y, 0);
            ForceShoot.x = Random.Range(-SpreadX, SpreadX);
            ForceShoot.y = Random.Range(-SpreadY, SpreadY);
            DirWithSpread = DirWithoutSpread + new Vector3(ForceShoot.x, ForceShoot.y, 0);
            GameObject Current_Bullet = Instantiate(Bullet, StartShoot.position, Quaternion.identity);
            Current_Bullet.transform.forward = DirWithSpread.normalized;
            Current_Bullet.GetComponent<Rigidbody>().AddForce(DirWithSpread.normalized * ShootForce, ForceMode.Impulse);
            Destroy(Current_Bullet, TimeDestroyBullet);
        }
    }
}
