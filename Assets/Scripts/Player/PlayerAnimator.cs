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
        animator.SetFloat("MoveX", playerInputLisener.inputAction.x);
        animator.SetFloat("MoveY", playerInputLisener.inputAction.y);
        animator.SetFloat("PlayerAngle", playerMovementController.playerModel.rotation.eulerAngles.y);
        animator.SetInteger("IndexWeapon", playerSelectWeaponController.weaponSwitch);
    }
}
