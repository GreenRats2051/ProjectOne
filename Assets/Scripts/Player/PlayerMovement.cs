using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    public Transform playerModel;
    public Transform MousePoint;
    [SerializeField]
    private LayerMask layerMask;
    private Collider[] hits;
    [SerializeField]
    private Rigidbody rigidbody;
    private float speed;
    [SerializeField]
    private float speedWalk;
    [SerializeField]
    private float speedCrouch;
    [SerializeField]
    private float walkSoundRadius;

    public void Walk(Vector2 InputAction)
    {
        rigidbody.velocity = new Vector3(InputAction.x * speed, rigidbody.velocity.y, InputAction.y * speed);
        RaycastHit RaycastHit;
        Ray RayMainCamera = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(RayMainCamera, out RaycastHit, Mathf.Infinity, layerMask))
        {
            MousePoint.position = RaycastHit.point;
        }
        playerModel.LookAt(new Vector3(MousePoint.position.x, transform.position.y, MousePoint.position.z));
        if (InputAction != null)
        {
            hits = Physics.OverlapSphere(gameObject.transform.position, speed);
            foreach (Collider hit in hits)
            {
                if (hit.gameObject.tag == "Enemy")
                {
                    if (hit.TryGetComponent<EnemyStatistics>(out EnemyStatistics enemyStatistics) || hit.TryGetComponent<EnemyMovement>(out EnemyMovement enemyMovement))
                    {
                        //enemyMovement.Player = gameObject;
                        enemyStatistics.IsSleep = false;
                        enemyStatistics.IsTrigered = true;
                    }
                }
            }
        }
    }

    public void Crouch(bool isCrouch)
    {
        if (isCrouch)
        {
            speed = speedCrouch;
        }
        else
        {
            speed = speedWalk;
        }
    }
}
