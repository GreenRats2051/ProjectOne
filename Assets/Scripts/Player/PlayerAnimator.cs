using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private PlayerInputLisener playerInputLisener;
    [SerializeField]
    private PlayerMovement playerMovementController;
    [SerializeField]
    private PlayerSelectWeapon playerSelectWeaponController;

    void Update()
    {
        animator.SetFloat("WalkX", playerInputLisener.inputAction.x);
        animator.SetFloat("WalkY", playerInputLisener.inputAction.y);
        animator.SetFloat("PlayerRotate", playerMovementController.playerModel.rotation.eulerAngles.y);
        animator.SetFloat("CurrentWeapon", playerSelectWeaponController.weaponSwitch);
    }
}
