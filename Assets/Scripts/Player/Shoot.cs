
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] WeaponData currentWeapon;
    [SerializeField] LayerMask hittableLayers;

    [Header("References")]
    [SerializeField] Camera playerCamera;
    [SerializeField] AudioSource audioSource;

    [Header("Zoom")]
    [SerializeField] float zoomSpeed = 10f;

    private int ammo;
    private float nextFireTime;
    private float normalFOV;

    void Start()
    {
        if (playerCamera == null)
        {
            Debug.LogError("Player Camera is missing!");
            enabled = false;
            return;
        }

        normalFOV = playerCamera.fieldOfView;

        if (currentWeapon != null)
        {
            ammo = currentWeapon.magazineSize;
        }
        else
        {
            Debug.LogError("No WeaponData assigned!");
        }
    }

    void Update()
    {
        if (currentWeapon == null || playerCamera == null)
            return;

        HandleShootingInput();
        HandleZoomInput();
    }

    // Handles mouse and controller shooting
    void HandleShootingInput()
    {
        bool mouseHeld = Mouse.current != null &&
                         Mouse.current.leftButton.isPressed;

        bool mousePressed = Mouse.current != null &&
                            Mouse.current.leftButton.wasPressedThisFrame;

        bool controllerHeld = Gamepad.current != null &&
                              Gamepad.current.rightTrigger.isPressed;

        bool controllerPressed = Gamepad.current != null &&
                                 Gamepad.current.rightTrigger.wasPressedThisFrame;

        bool shouldShoot = currentWeapon.automatic
            ? mouseHeld || controllerHeld
            : mousePressed || controllerPressed;

        if (shouldShoot)
        {
            Shooting();
        }
    }

    // Main shooting function
    public void Shooting()
    {
        if (!CanFire())
            return;

        RecordShot();

        Vector3 origin = playerCamera.transform.position;
        Vector3 direction = playerCamera.transform.forward;

        // Draw the raycast line in the Scene view
        Debug.DrawRay(
            origin,
            direction * currentWeapon.range,
            Color.red,
            1f
        );


        // Play the current weapon's shooting sound
        if (audioSource != null && currentWeapon.shootingSound != null)
        {
            audioSource.PlayOneShot(currentWeapon.shootingSound);
        }


        // Layer-filtered raycast
        if (Physics.Raycast(
            origin,
            direction,
            out RaycastHit hit,
            currentWeapon.range,
            hittableLayers,
            QueryTriggerInteraction.Ignore))
        {
            // Green line shows where the raycast hit
            Debug.DrawLine(origin, hit.point, Color.green, 1f);

            Debug.Log("Hit: " + hit.transform.name);

            EnemyHealth enemy =
                hit.collider.GetComponent<EnemyHealth>();

            // Also check parent in case EnemyHealth is on a parent object
            if (enemy == null)
            {
                enemy = hit.collider.GetComponentInParent<EnemyHealth>();
            }

            if (enemy != null)
            {
                enemy.TakeDamage(currentWeapon.damage);
                Debug.Log("Enemy damaged!");
            }
        }
    }

    // Checks ammo and cooldown
    bool CanFire()
    {
        return ammo > 0 && Time.time >= nextFireTime;
    }

    // Records each shot
    void RecordShot()
    {
        ammo--;

        nextFireTime = Time.time + currentWeapon.cooldown;

        Debug.Log("Ammo: " + ammo);
    }

    // Changes the active weapon
    public void SetWeapon(WeaponData newWeapon)
    {
        if (newWeapon == null)
            return;

        currentWeapon = newWeapon;

        ammo = currentWeapon.magazineSize;

        // Reset camera zoom when switching weapons
        playerCamera.fieldOfView = normalFOV;

        Debug.Log("Weapon switched to: " + currentWeapon.weaponName);
        Debug.Log("Ammo: " + ammo);
    }

    // Adds ammunition
    public void AddAmmo(int amount)
    {
        if (currentWeapon == null)
            return;

        ammo = Mathf.Min(
            ammo + amount,
            currentWeapon.magazineSize
        );

        Debug.Log("Ammo restored: " + ammo);
    }

    // Handles zoom input
    void HandleZoomInput()
    {
        bool mouseZoom = Mouse.current != null &&
                         Mouse.current.rightButton.isPressed;

        bool controllerZoom = Gamepad.current != null &&
                              Gamepad.current.leftTrigger.isPressed;

        bool zooming = (mouseZoom || controllerZoom) &&
                       currentWeapon.zoomFOV < normalFOV;

        SetZoom(zooming);
    }

    // Smoothly changes camera FOV
    void SetZoom(bool zooming)
    {
        float targetFOV = zooming
            ? currentWeapon.zoomFOV
            : normalFOV;

        playerCamera.fieldOfView = Mathf.Lerp(
            playerCamera.fieldOfView,
            targetFOV,
            Time.deltaTime * zoomSpeed
        );
    }

}
