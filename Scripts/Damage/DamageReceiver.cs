using System;
using UnityEngine;

public class DamageReceiver : MonoBehaviour, IDamageable
{
    [SerializeField] private Health health;
    [SerializeField] private DamageModifier[] modifiers;

    private void Awake()
    {
        Utility.AutoAssign(ref health, this);
    }

    public void TakeDamage(float amount, DamageType type)
    {
        DamageModifier currentModifier = null;

        foreach (DamageModifier mod in modifiers ?? Array.Empty<DamageModifier>())
        {
            if (mod.type == type)
            {
                currentModifier = mod;
                break;
            }
        }

        float final = amount * (currentModifier?.multiplier ?? 0f);
        health.TakeDamage(final);
    }
}

public enum DamageType
{
    Physical,
    Fire,
    Explosion,
    Poison
}

[Serializable]
public class DamageModifier
{
    public DamageType type;
    public float multiplier = 1f;
}