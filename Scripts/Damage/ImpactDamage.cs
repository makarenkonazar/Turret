using UnityEngine;

public class ImpactDamage : MonoBehaviour
{
    [SerializeField] private float multiplier = 1f;
    [SerializeField] private float minImpact = 2f;
    [SerializeField] private DamageReceiver receiver;

    private void Awake()
    {
        Utility.AutoAssign(ref receiver, this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        float impact = collision.relativeVelocity.magnitude;

        if (impact >= minImpact)
        {
            float damage = (impact - minImpact) * multiplier;
            receiver.TakeDamage(damage, DamageType.Physical);
        }
    }
}
