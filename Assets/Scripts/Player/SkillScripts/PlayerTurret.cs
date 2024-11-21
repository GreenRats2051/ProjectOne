using UnityEngine;

public class PlayerTurret : MonoBehaviour
{
    [SerializeField]
    private Transform startShoot;
    [SerializeField]
    private Transform upperPart;
    [SerializeField]
    private Transform enemy;
    [SerializeField]
    private GameObject bullet;
    private enum StateMachine { RotateLeft, RotateRight }
    [SerializeField]
    private StateMachine stateMachine;
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private int shootForce;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float angleRotation;
    private float procentAngle;
    [SerializeField]
    private float spreadX;
    [SerializeField]
    private float spreadY;
    [SerializeField]
    private float timeNextShoot;
    private float timeShoot;
    [SerializeField]
    private float timeDestroyBullet;
    private bool isAttack;

    void Start()
    {
        angleRotation /= 2;
    }

    void Update()
    {
        Rotate();
        Aim();
    }

    void Rotate()
    {
        if (!isAttack)
        {
            if (stateMachine == StateMachine.RotateRight)
            {
                procentAngle += Time.deltaTime / 2;
                if (procentAngle >= 1)
                {
                    procentAngle = 1;
                    stateMachine = StateMachine.RotateLeft;
                }
            }
            else if (stateMachine == StateMachine.RotateLeft)
            {
                procentAngle -= Time.deltaTime / 2;
                if (procentAngle <= 0)
                {
                    procentAngle = 0;
                    stateMachine = StateMachine.RotateRight;
                }
            }
            upperPart.localRotation = Quaternion.Euler(upperPart.rotation.x, Mathf.Lerp(-angleRotation, angleRotation, procentAngle), upperPart.rotation.z);
        }
    }

    void Aim()
    {
        RaycastHit RaycastHit;
        Ray Ray = new Ray(startShoot.position, startShoot.forward);
        if (Physics.Raycast(Ray, out RaycastHit, 100f))
        {
            lineRenderer.SetPosition(1, new Vector3(0, 0, RaycastHit.point.z));
            if (RaycastHit.collider.tag == "Enemy" && RaycastHit.collider.isTrigger == false)
            {
                enemy = RaycastHit.transform;
                Attack(RaycastHit);
            }
            else
            {
                Rotate();
            }
        }
        if (enemy != null && upperPart.localRotation.y <= angleRotation / 100 && upperPart.localRotation.y >= -angleRotation / 100)
        {
            isAttack = true;
            upperPart.LookAt(new Vector3(enemy.position.x, upperPart.position.y, enemy.position.z));
        }
        else if (enemy == null || upperPart.localRotation.y >= angleRotation / 100 || upperPart.localRotation.y <= -angleRotation / 100)
        {
            isAttack = false;
            enemy = null;
            Rotate();
        }
    }

    void Attack(RaycastHit RaycastHit)
    {
        isAttack = true;
        timeShoot += Time.deltaTime;
        if (timeShoot >= timeNextShoot)
        {
            timeShoot = 0;
            Vector3 dirWithoutSpread = RaycastHit.point - startShoot.position;
            Vector2 forceShoot = new Vector2(Random.Range(-spreadX, spreadX), Random.Range(-spreadY, spreadY));
            Vector3 dirWithSpread = dirWithoutSpread + new Vector3(forceShoot.x, forceShoot.y, 0);
            GameObject currentBulletObject = Instantiate(bullet, startShoot.position, startShoot.rotation);
            currentBulletObject.transform.forward = dirWithSpread.normalized;
            currentBulletObject.GetComponent<Rigidbody>().AddForce(dirWithSpread.normalized * shootForce, ForceMode.Impulse);
            currentBulletObject.AddComponent<PlayerBullet>();
            currentBulletObject.GetComponent<PlayerBullet>().Damage = damage;
            Destroy(currentBulletObject, timeDestroyBullet);
        }
    }
}
