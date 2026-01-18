using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatController : MonoBehaviour
{
    public static PlayerCombatController Instance;

    public bool isCombatMode;

    [Header("Cursor")]
    public Texture2D defaultCursor;
    public Texture2D combatCursor;
    public Vector2 cursorHotspot;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip enterCombatSound;
    public AudioClip exitCombatSound;

    [Header("Weapon")]
    public WeaponController weapon;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SetCombatMode(false);
    }

    void Update()
    {
        HandleCombatToggle();
        HandleWeaponInput();
    }

    // ---------------- COMBAT MODE ----------------

    void HandleCombatToggle()
    {
        if (Keyboard.current != null && Keyboard.current.aKey.wasPressedThisFrame)
        {
            SetCombatMode(!isCombatMode);
        }
    }

    // ---------------- WEAPON INPUT ----------------

    void HandleWeaponInput()
    {
        if (Keyboard.current == null || weapon == null)
            return;

        // 🔫 Доставать / убирать оружие — ВСЕГДА
        //if (Keyboard.current.digit1Key.wasPressedThisFrame)
        //{
        //    weapon.ToggleWeapon();
        //}

        // 🔄 Перезарядка — всегда (оружие само решит)
       // if (Keyboard.current.rKey.wasPressedThisFrame)
       //{
       //     weapon.Reload();
       // }

        // 🔥 Стрельба — только в боевом режиме
        if (isCombatMode &&
            Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            weapon.TryShoot();
        }
    }

    // ---------------- STATE ----------------

    void SetCombatMode(bool value)
    {
        isCombatMode = value;

        UnityEngine.Cursor.SetCursor(
            isCombatMode ? combatCursor : defaultCursor,
            cursorHotspot,
            UnityEngine.CursorMode.Auto
        );

        if (audioSource != null)
            audioSource.PlayOneShot(isCombatMode ? enterCombatSound : exitCombatSound);
    }
}
