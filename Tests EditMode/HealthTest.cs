using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using System.Reflection;

public class HealthTests
{
    private GameObject go;
    private Health health;

    private UnityEvent<float> onHealthChange;
    private UnityEvent onDamaged;
    private UnityEvent onHealed;
    private UnityEvent onDeath;
    private UnityEvent onRevive;

    [SetUp]
    public void Setup()
    {
        go = new GameObject("HealthTestObject");
        health = go.AddComponent<Health>();

        typeof(Health)
            .GetField("_maxHealth", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(health, 100f);

        typeof(Health)
            .GetField("_currentHealth", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(health, 100f);

        var eventsField = typeof(Health)
            .GetField("_events", BindingFlags.NonPublic | BindingFlags.Instance);

        var events = eventsField.GetValue(health);
        var eventsType = events.GetType();

        onHealthChange = (UnityEvent<float>)eventsType.GetField("OnHealthChange").GetValue(events);
        onDamaged      = (UnityEvent)eventsType.GetField("OnDamaged").GetValue(events);
        onHealed       = (UnityEvent)eventsType.GetField("OnHealed").GetValue(events);
        onDeath        = (UnityEvent)eventsType.GetField("OnDeath").GetValue(events);
        onRevive       = (UnityEvent)eventsType.GetField("OnRevive").GetValue(events);
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(go);
    }

    [Test]
    public void TakeDamage_DecreasesHealth()
    {
        health.TakeDamage(25f);
        Assert.AreEqual(75f, health.GetHealth());
    }

    [Test]
    public void Heal_IncreasesHealth()
    {
        health.TakeDamage(50f);
        health.Heal(30f);
        Assert.AreEqual(80f, health.GetHealth());
    }

    [Test]
    public void Health_IsClamped_ToMax()
    {
        health.Heal(100f);
        Assert.AreEqual(100f, health.GetHealth());
    }

    [Test]
    public void Health_IsClamped_ToZero()
    {
        health.TakeDamage(200f);
        Assert.AreEqual(0f, health.GetHealth());
    }

    [Test]
    public void OnDamaged_Called_OnNegativeDelta()
    {
        int count = 0;
        onDamaged.AddListener(() => count++);

        health.TakeDamage(10f);

        Assert.AreEqual(1, count);
    }

    [Test]
    public void OnHealed_Called_OnPositiveDelta()
    {
        int count = 0;
        onHealed.AddListener(() => count++);

        health.Heal(10f);

        Assert.AreEqual(1, count);
    }

    [Test]
    public void OnDeath_Called_Once_WhenHealthReachesZero()
    {
        int count = 0;
        onDeath.AddListener(() => count++);

        health.TakeDamage(200f);
        health.TakeDamage(50f);

        Assert.AreEqual(1, count);
    }

    [Test]
    public void OnRevive_Called_WhenHealedFromZero()
    {
        int count = 0;
        onRevive.AddListener(() => count++);

        health.TakeDamage(200f);
        health.Heal(10f);

        Assert.AreEqual(1, count);
    }

    [Test]
    public void OnRevive_NotCalled_IfNotDead()
    {
        int count = 0;
        onRevive.AddListener(() => count++);

        health.Heal(10f);

        Assert.AreEqual(0, count);
    }

    [Test]
    public void OnHealthChange_ReceivesCorrectDelta()
    {
        float receivedDelta = 0f;
        onHealthChange.AddListener(d => receivedDelta = d);

        health.TakeDamage(15f);

        Assert.AreEqual(-15f, receivedDelta);
    }
}