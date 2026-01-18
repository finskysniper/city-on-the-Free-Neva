using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponData : ItemData
{
    [Header("Weapon Info")]
    public WeaponClass weaponClass;

    [Header("Stats")]
    public int baseDamage = 20;
    public int magazineSize = 30;
    public float range = 100f;

    [Header("Critical")]
    public float critChance = 0.1f;
    public float critMultiplier = 2f;

    [Header("Accuracy")]
    [Range(0f, 1f)]
    public float accuracy = 0.9f;

    [Header("Audio")]
    public WeaponAudioProfile audioProfile;
}
