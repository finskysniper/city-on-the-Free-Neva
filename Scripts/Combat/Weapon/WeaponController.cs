using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public GameObject weaponObject;
    public Transform firePoint;

    [Header("Weapon Data")]
    public WeaponData data;

    [Header("Audio")]
    public WeaponAudioProfile audioProfile;
    public AudioSource audioSource;

    [Header("Timings")]
    public float pullOutTime = 0.5f;
    public float putAwayTime = 0.5f;
    public float reloadTime = 1.5f;

    enum WeaponState
    {
        Unequipped,
        Equipping,
        Equipped,
        Unequipping,
        Reloading
    }

    WeaponState state = WeaponState.Unequipped;
    int currentAmmo;

    void Start()
    {
        weaponObject.SetActive(false);
        currentAmmo = data.magazineSize;

        animator.Play("Unarmed");
    }

    // ================= PUBLIC API =================

    public void ToggleWeapon()
    {
        if (state != WeaponState.Unequipped && state != WeaponState.Equipped)
            return;

        if (state == WeaponState.Unequipped)
            StartCoroutine(PullOut());
        else
            StartCoroutine(PutAway());
    }

    public void TryShoot()
    {
        if (state != WeaponState.Equipped)
            return;

        if (currentAmmo <= 0)
        {
            PlaySound(audioProfile.emptyClick);
            Reload();
            return;
        }

        Shoot();
    }

    public void Reload()
    {
        if (state != WeaponState.Equipped)
            return;

        StartCoroutine(ReloadRoutine());
    }

    // ================= STATE ROUTINES =================

    IEnumerator PullOut()
    {
        state = WeaponState.Equipping;
    
        animator.Play("PullOut");
      PlaySound(audioProfile.pullOut);
    
        yield return new WaitForSeconds(pullOutTime);
    
        weaponObject.SetActive(true);
        animator.Play("Armed");
    
        state = WeaponState.Equipped;
      }

    IEnumerator PutAway()
    {
        state = WeaponState.Unequipping;

        animator.Play("PutAway");
        PlaySound(audioProfile.putAway);

        yield return new WaitForSeconds(putAwayTime);

        weaponObject.SetActive(false);
        animator.Play("Unarmed");

        state = WeaponState.Unequipped;
    }

    IEnumerator ReloadRoutine()
    {
        state = WeaponState.Reloading;

        animator.Play("Reload");
        PlaySound(audioProfile.reload);

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = data.magazineSize;
        state = WeaponState.Equipped;
    }

    // ================= LOGIC =================

    void Shoot()
    {
        currentAmmo--;
        PlaySound(audioProfile.fire);

        if (Camera.main == null || Mouse.current == null)
            return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, data.range))
        {
            Targetable target = hit.collider.GetComponent<Targetable>();
            if (target != null)
            {
                DamageData dmg = DamageCalculator.Calculate(data);
                target.TakeDamage(dmg);
            }
        }
    }

    // ================= AUDIO =================

    void PlaySound(AudioClip clip)
    {
        if (clip == null || audioSource == null)
            return;

        audioSource.PlayOneShot(clip);
    }
}
