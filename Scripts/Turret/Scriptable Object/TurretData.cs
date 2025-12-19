using UnityEngine;

[CreateAssetMenu(fileName = "TurretData", menuName = "Turret/TurretData")]
public class TurretData : ScriptableObject
    
{
    [Header("Turret Base Setting")]
    public float BaseSpeed = 90f;
    public float maxYawAngle = 90f;
    public float minYawAngle = -90f;

    [Space(10)][Header("Turret Barrel Setting")]
    public float BarrelSpeed = 90f;
    public float maxPitchAngle = 30f;
    public float minPitchAngle = -30f;
}