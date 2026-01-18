using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Weapon Audio Profile")]
public class WeaponAudioProfile : ScriptableObject
{
    public WeaponClass weaponClass;

    [Header("Fire")]
    public AudioClip fire;
    public AudioClip fireSilenced;

    [Header("Reload")]
    public AudioClip reload;

    [Header("Equip")]
    public AudioClip pullOut;
    public AudioClip putAway;

    [Header("Dry Fire")]
    public AudioClip emptyClick;
}
