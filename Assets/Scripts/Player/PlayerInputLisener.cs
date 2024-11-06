using UnityEngine;

public class PlayerInputLisener : MonoBehaviour
{
    public Vector2 inputAction;
    [SerializeField]
    private PlayerMovementController playerMovementController;
    [SerializeField]
    private PlayerMeleeController playerMeleeController;
    [SerializeField]
    private PlayerGunController[] playerGunController;
    [SerializeField]
    private PlayerSelectWeaponController playerSelectWeaponController;
    [SerializeField]
    private PlayerSmokeGrenade playerSmokeGrenade;
    [SerializeField]
    private PlayerUseSkillController playerUseSkillController;
    [SerializeField]
    private PauseController pauseController;
    [SerializeField]
    private KeyCode buttonCrouch;
    [SerializeField]
    private KeyCode buttonAttack;
    [SerializeField]
    private KeyCode buttonReload;
    [SerializeField]
    private KeyCode buttonGrenade;
    [SerializeField]
    private KeyCode buttonUseSkill;
    [SerializeField]
    private KeyCode buttonPause;
    private float mouseScroll;

    void Update()
    {
        inputAction.x = Input.GetAxis("Horizontal");
        inputAction.y = Input.GetAxis("Vertical");
        mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        if (playerMovementController != null)
        {
            playerMovementController.Walk(inputAction);
            playerMovementController.Crouch(Input.GetKey(buttonCrouch));
        }
        if (Input.GetKeyDown(buttonAttack) && playerMeleeController != null)
        {
            playerMeleeController.Attack();
        }
        if (Input.GetKey(buttonAttack) && playerGunController.Length != 0 && playerSelectWeaponController.weaponSwitch > 0)
        {
            playerGunController[playerSelectWeaponController.weaponSwitch - 1].Shoot(playerMovementController);
        }
        if (Input.GetKey(buttonReload) && playerGunController.Length != 0)
        {
            playerGunController[playerSelectWeaponController.weaponSwitch - 1].Reload();
        }
        if (playerSelectWeaponController != null)
        {
            playerSelectWeaponController.SelectWeapon(mouseScroll);
        }
        if (Input.GetKeyDown(buttonGrenade) && playerSmokeGrenade != null)
        {
            playerSmokeGrenade.Spawn(playerMovementController.MousePoint.position);
        }
        if (Input.GetKeyDown(buttonUseSkill) && playerUseSkillController != null)
        {
            playerUseSkillController.UseSkill();
        }
        if (Input.GetKeyDown(buttonPause) && pauseController != null)
        {
            pauseController.OpenPause();
        }
    }
}
