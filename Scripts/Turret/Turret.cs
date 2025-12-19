using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    protected TurretBase _turretBase;
    protected TurretBarrel _turretBarrel;

    protected abstract TurretData _data { get; }

    protected virtual void Awake()
    {
        _turretBase = GetComponentInChildren<TurretBase>();
        _turretBarrel = GetComponentInChildren<TurretBarrel>();

        _turretBase.Initialize(_data);
        _turretBarrel.Initialize(_data);
    }

    public virtual void SetTarget(Vector3 target)
    {
        _turretBase.SetTarget(target);
        _turretBarrel.SetTarget(target);
    }

    protected abstract void Fire();
}