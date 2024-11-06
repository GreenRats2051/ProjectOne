using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    public Transform playerModel;
    public Transform MousePoint;
    [SerializeField]
    private Rigidbody rigidbody;
    private float speed;
    [SerializeField]
    private float speedWalk;
    [SerializeField]
    private float speedCrouch;

    public void Walk(Vector2 InputAction)
    {
        rigidbody.velocity = new Vector3(InputAction.x * speed, rigidbody.velocity.y, InputAction.y * speed);
        RaycastHit RaycastHit;
        Ray RayMainCamera = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(RayMainCamera, out RaycastHit, Mathf.Infinity))
        {
            MousePoint.position = new Vector3(RaycastHit.point.x, transform.position.y, RaycastHit.point.z);
        }
        playerModel.LookAt(new Vector3(MousePoint.position.x, transform.position.y, MousePoint.position.z));
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
