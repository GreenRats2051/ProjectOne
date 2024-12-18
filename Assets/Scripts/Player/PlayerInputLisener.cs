using UnityEngine;

public class PlayerInputLisener : MonoBehaviour
{
    public Vector2 inputAction;
    [SerializeField]
    private PlayerMovement playerMovementController;
    [SerializeField]
    private PlayerMelee playerMeleeController;
    [SerializeField]
    private PlayerGun[] playerGunController;
    [SerializeField]
    private PlayerSelectWeapon playerSelectWeaponController;
    [SerializeField]
    private PlayerUseSmokeGrenade playerSmokeGrenade;
    [SerializeField]
    private PlayerUseSkill playerUseSkillController;
    [SerializeField]
    private Pause pauseController;
    [SerializeField]
    private KeyCode buttonCrouch;
    [SerializeField]
    private KeyCode buttonAttack;
    [SerializeField]
    private KeyCode buttonShoot;
    [SerializeField]
    private KeyCode buttonReload;
    [SerializeField]
    private KeyCode buttonGrenade;
    [SerializeField]
    private KeyCode buttonUseSkill;
    [SerializeField]
    private KeyCode buttonOpenDeveloperMenu;
    [SerializeField]
    private KeyCode buttonPause;
    private float mouseScroll;
    private bool pauseActive;

    void Update()
    {
        inputAction.x = Input.GetAxis("Horizontal");
        inputAction.y = Input.GetAxis("Vertical");
        mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        if (!pauseActive)
        {
            if (playerMovementController != null)
            {
                playerMovementController.Walk(inputAction);
                playerMovementController.Crouch(Input.GetKey(buttonCrouch));
            }
            if (Input.GetKeyDown(buttonAttack) && playerMeleeController != null)
            {
                playerMeleeController.Attack();
            }
            if (Input.GetKey(buttonShoot) && playerGunController.Length != 0 && playerSelectWeaponController.weaponSwitch > 0)
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
        }
        if (Input.GetKeyDown(buttonPause) && pauseController != null)
        {
            pauseController.OpenPause();
        }
    }

    public void ActivateScripts(bool isActive)
    {
        pauseActive = !isActive;
    }
}
