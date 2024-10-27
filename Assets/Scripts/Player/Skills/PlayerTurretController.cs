using UnityEngine;

public class PlayerTurretController : MonoBehaviour
{
    [SerializeField]
    private Transform StartShoot;
    [SerializeField]
    private Transform UpperPart;
    [SerializeField]
    private Transform Enemy;
    [SerializeField]
    private GameObject Bullet;
    [SerializeField]
    private LineRenderer LineRenderer;
    private enum StateMachine { RotateLeft, RotateRight }
    [SerializeField]
    private StateMachine stateMachine;
    [SerializeField]
    private int ShootForce;
    [SerializeField]
    private int Damage;
    [SerializeField]
    private float AngleRotation;
    private float ProcentRotate;
    [SerializeField]
    private float SpreadX;
    [SerializeField]
    private float SpreadY;
    [SerializeField]
    private float TimeNextShoot;
    private float TimeShoot;
    [SerializeField]
    private float TimeDestroyBullet;
    private bool IsAttack;

    void Start()
    {
        AngleRotation /= 2;
    }

    void Update()
    {
        Rotate();
        Aim();
    }

    void Rotate()
    {
        if (IsAttack == false)
        {
            if (stateMachine == StateMachine.RotateRight)
            {
                ProcentRotate += Time.deltaTime / 2;
                if (ProcentRotate >= 1)
                {
                    ProcentRotate = 1;
                    stateMachine = StateMachine.RotateLeft;
                }
            }
            else if (stateMachine == StateMachine.RotateLeft)
            {
                ProcentRotate -= Time.deltaTime / 2;
                if (ProcentRotate <= 0)
                {
                    ProcentRotate = 0;
                    stateMachine = StateMachine.RotateRight;
                }
            }
            UpperPart.localRotation = Quaternion.Euler(UpperPart.rotation.x, Mathf.Lerp(-AngleRotation, AngleRotation, ProcentRotate), UpperPart.rotation.z);
        }
    }

    void Aim()
    {
        RaycastHit RaycastHit;
        Ray Ray = new Ray(StartShoot.position, StartShoot.forward);
        if (Physics.Raycast(Ray, out RaycastHit, 100f))
        {
            LineRenderer.SetPosition(1, new Vector3(0, 0, RaycastHit.point.z));
            if (RaycastHit.collider.tag == "Enemy" && RaycastHit.collider.isTrigger == false)
            {
                Enemy = RaycastHit.transform;
                Attack(RaycastHit);
            }
            else
            {
                Rotate();
            }
        }
        if (Enemy != null && UpperPart.localRotation.y <= AngleRotation / 100 && UpperPart.localRotation.y >= -AngleRotation / 100)
        {
            IsAttack = true;
            UpperPart.LookAt(new Vector3(Enemy.position.x, UpperPart.position.y, Enemy.position.z));
        }
        else if (Enemy == null || UpperPart.localRotation.y >= AngleRotation / 100 || UpperPart.localRotation.y <= -AngleRotation / 100)
        {
            IsAttack = false;
            Enemy = null;
            Rotate();
        }
    }

    void Attack(RaycastHit RaycastHit)
    {
        IsAttack = true;
        TimeShoot += Time.deltaTime;
        if (TimeShoot >= TimeNextShoot)
        {
            TimeShoot = 0;
            Vector3 dirWithoutSpread = RaycastHit.point - StartShoot.position;
            Vector2 ForceShoot = new Vector2(Random.Range(-SpreadX, SpreadX), Random.Range(-SpreadY, SpreadY));
            Vector3 dirWithSpread = dirWithoutSpread + new Vector3(ForceShoot.x, ForceShoot.y, 0);
            GameObject currentBulletObject = Instantiate(Bullet, StartShoot.position, StartShoot.rotation);
            currentBulletObject.transform.forward = dirWithSpread.normalized;
            currentBulletObject.GetComponent<Rigidbody>().AddForce(dirWithSpread.normalized * ShootForce, ForceMode.Impulse);
            currentBulletObject.GetComponent<PlayerBulletController>().Damage = Damage;
            Destroy(currentBulletObject, TimeDestroyBullet);
        }
    }
}
