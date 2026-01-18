using UnityEngine;

public static class DamageCalculator
{
    public static DamageData Calculate(WeaponData weapon)
    {
        DamageData dmg = new DamageData();

        bool crit = Random.value <= weapon.critChance;

        dmg.isCritical = crit;
        dmg.finalDamage = crit
            ? Mathf.RoundToInt(weapon.baseDamage * weapon.critMultiplier)
            : weapon.baseDamage;

        return dmg;
    }
}
