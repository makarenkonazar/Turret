using System.Collections;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ProjectileSimulator _simulator;
    [SerializeField] private Transform _spawnPoint;

    [Header("Ammo")]
    [SerializeField] private AmmoType _ammoType;
    [SerializeField] private float _initialSpeed = 0f;

    [Header("Spawn settings")]
    [SerializeField, Min(1)] private int _count = 10;
    [SerializeField, Min(0f)] private float _rate = 0.0f; // інтервал між снарядами (0 = одразу всі)
    [SerializeField, Range(0f, 45f)] private float _spreadAngle = 0f;
    [SerializeField, Min(0f)] private float _randomSpeedVariation = 0f; // ± відхилення швидкості (0..1) у відсотках

    [Header("Lifecycle")]
    [SerializeField] private bool _destroyAfterSpawn = false; // видалити спавнер після спавну
    [SerializeField] private bool _spawnOnStart = false;

    private Coroutine _spawnCoroutine;


    private void Start()
    {
        if (_spawnOnStart)
            StartSpawn();
    }

    public void StartSpawn()
    {
        if (_simulator == null)
        {
            Debug.LogWarning("ProjectileSpawner: ProjectileSimulator is null.");
            return;
        }

        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);

        _spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    public void StopSpawn()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnRoutine()
    {
        if (_rate <= 0f)
        {
            for (int i = 0; i < _count; i++)
            {
                SpawnOne(i);
            }
        }
        else
        {
            for (int i = 0; i < _count; i++)
            {
                SpawnOne(i);
                yield return new WaitForSeconds(_rate);
            }
        }

        _spawnCoroutine = null;

        if (_destroyAfterSpawn)
            Destroy(gameObject);
    }

    // Спавнить один снаряд з налаштуваннями spread та випадковістю швидкості
    private void SpawnOne(int index)
    {
        Vector3 pos = _spawnPoint.position;
        Vector3 forward = _spawnPoint.forward;

        // Розкид кута: обертаємо forward навколо вектора up і right
        Vector3 dir = ApplySpread(forward, _spreadAngle);

        // Початкова швидкість — із використанням базової initialSpeed з цього спавнера
        float speed = _initialSpeed;

        if (_randomSpeedVariation > 0f)
        {
            float delta = (Random.value * 2f - 1f) * _randomSpeedVariation;
            speed = speed * (1f + delta);
        }

        Vector3 velocity = dir.normalized * speed;

        if (_ammoType != null)
        {
            _simulator.SpawnFromAmmo(pos, velocity, _ammoType);
        }
        else
        {
            // дефолт:
            float mass = 1f;
            float drag = 0.01f;
            float radius = 0.1f;
            float maxDistance = 100f;
            float damage = 10f;

            _simulator.SpawnCustomProjectile(pos, velocity, mass, drag, radius, maxDistance, damage);
        }
    }

    private Vector3 ApplySpread(Vector3 forward, float maxAngleDeg)
    {
        if (maxAngleDeg <= 0f) return forward;

        // Випадковий полярний кут у межах конуса maxAngleDeg
        float angle = Random.Range(0f, maxAngleDeg);
        float azimuth = Random.Range(0f, 360f);

        // Конвертуємо до вектора відхилення
        Quaternion rot = Quaternion.AngleAxis(angle, Quaternion.Euler(0f, azimuth, 0f) * Vector3.up);
        return rot * forward;
    }

    #region quick spawn

    // Контекстне меню в інспекторі для швидкого спавну
    [ContextMenu("Spawn Once (editor)")]
    private void ContextSpawnOnce()
    {
        StartSpawn();
    }

    #endregion
}