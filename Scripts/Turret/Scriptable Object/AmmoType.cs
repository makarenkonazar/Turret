using UnityEngine;

[CreateAssetMenu(fileName = "AmmoType", menuName = "Ammo/AmmoType")]
public class AmmoType : ScriptableObject
{
    [Header("Physical parameters")]
    public float mass;
    public float dragCoefficient;
    public float radius;

    [Space(10)]
    [Header("Behavior parameters")]
    public float maxDistance;
    public float damage;
}