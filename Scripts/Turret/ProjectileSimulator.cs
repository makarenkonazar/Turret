using UnityEngine;
using System.Collections.Generic;

public class ProjectileSimulator : MonoBehaviour
{
    private float gravityScale = 1.0f;

    private struct Projectile
    {
        public Vector3 position;
        public Vector3 lastPosition;
        public Vector3 velocity;

        public float mass;
        public float dragCoefficient;
        public float radius;

        public float maxDistance;
        public float traveledDistance;

        public float damage;
    }

    private List<Projectile> _projectiles = new List<Projectile>();

    private void FixedUpdate()
    {
        SimulateStep();
    }

    private void SimulateStep()
    {
        float dt = Time.fixedDeltaTime;

        for (int i = _projectiles.Count - 1; i >= 0; i--)
        {
            Projectile p = _projectiles[i];
            Vector3 oldPos = p.position;

            p.velocity += Physics.gravity * gravityScale * dt;
            p.velocity *= 1f - p.dragCoefficient * dt;
            Vector3 newPos = oldPos + p.velocity * dt;

            Vector3 dir = newPos - p.position;
            float stepDistance = dir.magnitude;

            if (stepDistance > 0f)
            {
                if (Physics.SphereCast(oldPos, p.radius, dir.normalized, out RaycastHit hit, stepDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                {
                    IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        damageable.TakeDamage(p.damage, DamageType.Fire);
                    }

                    _projectiles.RemoveAt(i);
                    continue;
                }
            }

            p.lastPosition = oldPos;
            p.position = newPos;
            p.traveledDistance += dir.magnitude;
            _projectiles[i] = p;

            if (p.traveledDistance >= p.maxDistance)
            {
                _projectiles.RemoveAt(i);
            }
        }
    }

    public void SpawnCustomProjectile(Vector3 Position, Vector3 Velocity, float Mass, float DragCoefficient, float Radius, float MaxDistance, float Damage)
    {
        Projectile p = new Projectile
        {
            position = Position,
            lastPosition = Position,
            velocity = Velocity,

            mass = Mass,
            dragCoefficient = DragCoefficient,
            radius = Radius,

            maxDistance = MaxDistance,
            traveledDistance = 0,

            damage = Damage
        };
        _projectiles.Add(p);
    }

    public void SpawnFromAmmo(Vector3 position, Vector3 velocity, AmmoType ammo)
    {
        SpawnCustomProjectile(
            position,
            velocity,
            ammo.mass,
            ammo.dragCoefficient,
            ammo.radius,
            ammo.maxDistance,
            ammo.damage
        );
    }

    private void OnDrawGizmos()
    {
        DrawGizmos();
    }

    public void DrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (var p in _projectiles)
        {
            Gizmos.DrawSphere(p.position, p.radius);
            Gizmos.DrawLine(p.lastPosition, p.position);
        }
    }

}