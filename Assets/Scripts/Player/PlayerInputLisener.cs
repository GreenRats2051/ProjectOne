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
    private DroneDestroyer droneDestroyer;
    [SerializeField]
    private DroneHeal droneHeal;
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
    private KeyCode healDrone;
    [SerializeField]
    private KeyCode explosiveDrone;
    [SerializeField]
    private KeyCode buttonOpenDeveloperMenu;
    [SerializeField]
    private KeyCode buttonPause;
    private float mouseScroll;
    private bool pauseActive;
    private bool MoveDrone = false;

    void Update()
    {
        inputAction.x = Input.GetAxis("Horizontal");
        inputAction.y = Input.GetAxis("Vertical");
        mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        if (!pauseActive)
        {
            if(droneDestroyer.IsDroneAllive())
            {
                MoveDrone= true;
            }
            else
            {
                MoveDrone = false;
            }
            if (playerMovementController != null&&!MoveDrone)
            {
                playerMovementController.Walk(inputAction);
                playerMovementController.Crouch(Input.GetKey(buttonCrouch));
            }
            else
            {
                droneDestroyer.ActionDrone(inputAction.x, inputAction.y, Input.GetKeyDown(KeyCode.Space));
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
            if (Input.GetKeyDown(healDrone) && playerUseSkillController != null)
            {
                droneHeal.CreateDrone();
            }
            if (Input.GetKeyDown(explosiveDrone) && playerUseSkillController != null)
            {
                droneDestroyer.CreateDrone();
                
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
