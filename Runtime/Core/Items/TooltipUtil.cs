using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains static strings that are useful for dynamically generating tooltips
/// </summary>
public static class TooltipUtil
{
    public static string DamageColor = "<color=orange>";
    public static string Damage = "<color=orange>Damage</color>";

    public static string AttackSpeedColor = "<color=green>";
    public static string AttackSpeed = "<color=green>Attack Speed</color>";

    public static string TierColor = "<color=#fc03e3>";
    public static string Tier = "<color=#fc03e3><b>TIER</b></color>";

    /// <summary>
    /// return a scaling formula string for simple base + (tier * damagePerTier)
    /// </summary>
    public static string BasicScalingDamage(int baseDamage, float damagePerTier)
    {
        return $"{Damage} = <color=white>{baseDamage}</color> + ({Tier} x <color=white>{damagePerTier.ToString("0.#")}</color>)";
    }
    public static string BasicFormula(string prefix, int baseValue, float perTier)
    {
        return $"{prefix} = <color=white>{baseValue}</color> + ({Tier} x <color=white>{perTier.ToString("0.#")}</color>)";
    }

    public static string MultiLine(params string[] strs)
    {
        string result = "";
        for (int i = 0; i < strs.Length; i++)
        {
            result += strs[i];
            if (i < strs.Length - 1)
            {
                result += "<br>";
            }
        }
        return result;
    }
}
