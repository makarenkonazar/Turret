using NUnit.Framework;
using UnityEngine;
using System.Reflection;

public class DamageReceiverTests
{
    private GameObject go;
    private Health health;
    private DamageReceiver receiver;

    [SetUp]
    public void Setup()
    {
        go = new GameObject("DamageReceiverTestObject");

        health = go.AddComponent<Health>();
        receiver = go.AddComponent<DamageReceiver>();

        typeof(Health)
            .GetField("_maxHealth", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(health, 100f);

        typeof(Health)
            .GetField("_currentHealth", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(health, 100f);

        typeof(DamageReceiver)
            .GetField("health", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(receiver, health);
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(go);
    }

    [Test]
    public void Damage_WithoutModifier_IsIgnored()
    {
        receiver.TakeDamage(20f, DamageType.Physical);

        Assert.AreEqual(100f, health.GetHealth());
    }

    [Test]
    public void Damage_WithModifier_IsMultiplied()
    {
        var mods = new DamageModifier[]
        {
            new DamageModifier
            {
                type = DamageType.Fire,
                multiplier = 2f
            }
        };

        typeof(DamageReceiver)
            .GetField("modifiers", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(receiver, mods);

        receiver.TakeDamage(10f, DamageType.Fire);

        Assert.AreEqual(80f, health.GetHealth());
    }

    [Test]
    public void Only_FirstMatchingModifier_IsUsed()
    {
        var mods = new DamageModifier[]
        {
            new DamageModifier { type = DamageType.Fire, multiplier = 2f },
            new DamageModifier { type = DamageType.Fire, multiplier = 10f }
        };

        typeof(DamageReceiver)
            .GetField("modifiers", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(receiver, mods);

        receiver.TakeDamage(10f, DamageType.Fire);

        Assert.AreEqual(80f, health.GetHealth());
    }

    [Test]
    public void Damage_WithDifferentType_IgnoresModifier()
    {
        var mods = new DamageModifier[]
        {
            new DamageModifier { type = DamageType.Explosion, multiplier = 5f }
        };

        typeof(DamageReceiver)
            .GetField("modifiers", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(receiver, mods);

        receiver.TakeDamage(10f, DamageType.Physical);

        Assert.AreEqual(100f, health.GetHealth());
    }
}