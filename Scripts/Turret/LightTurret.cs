using UnityEngine;

public class LightTurret : Turret
{
    [SerializeField] private TurretData _turretData;

    [Space(10)]
    [Header("Bullet Settings")]
    [SerializeField] private ProjectileSimulator _projectileSimulator;
    [SerializeField] private AmmoType _ammoType;
    [SerializeField] private float _initialSpeed = 820f;
    [SerializeField] private Transform _firePoint;
    
    protected override TurretData _data => _turretData;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
    }
    protected override void Fire()
    {
        Vector3 Vel = _firePoint.forward * _initialSpeed;
        _projectileSimulator.SpawnFromAmmo(_firePoint.position, Vel, _ammoType);
    }
}