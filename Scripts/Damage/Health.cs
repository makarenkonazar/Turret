using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;

    [SerializeField] private EventsContainer _events = new();

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void ChangeHealth(float delta)
    {
        float prevHelth = _currentHealth;
        _currentHealth = Mathf.Clamp(_currentHealth + delta, 0, _maxHealth);
        _events.OnHealthChange?.Invoke(delta);

        if(_currentHealth <= 0 && prevHelth > 0)
            _events.OnDeath?.Invoke();
    
        if(_currentHealth > 0 && prevHelth <= 0)
            _events.OnRevive?.Invoke();

        if(delta < 0)
            _events.OnDamaged?.Invoke();

        if(delta > 0)
            _events.OnHealed?.Invoke();
    }

    public void TakeDamage(float amount) => ChangeHealth(-amount);
    public void Heal(float amount) => ChangeHealth(amount);

    public float GetHealth() => _currentHealth;
    public float GetMaxHealth() => _maxHealth;

    [Serializable]
    private class EventsContainer
    {
        public UnityEvent<float> OnHealthChange = new();
        public UnityEvent OnDamaged = new();
        public UnityEvent OnHealed = new();
        public UnityEvent OnDeath = new();
        public UnityEvent OnRevive = new();
    }
}