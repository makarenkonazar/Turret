using UnityEngine;

public class TurretBase : MonoBehaviour
{
    private TurretData _data;

    public void Initialize(TurretData data)
    {
        _data = data;
    }

    public void SetTarget(Vector3 WorldPosition)
    {
        LocalRotateToTarget(WorldPosition);
    }

    private void LocalRotateToTarget(Vector3 target)
    {
        Quaternion targetRotation = CalculateLocalTarget(target);
        transform.localRotation = Quaternion.RotateTowards(
            transform.localRotation,
            targetRotation,
            _data.BaseSpeed * Time.deltaTime
        );
    }

    private Quaternion CalculateLocalTarget(Vector3 target)
    {
        Vector3 localTargetDir = transform.parent.InverseTransformPoint(target) - transform.localPosition;
        localTargetDir.y = 0;

        float angle = Vector3.SignedAngle(Vector3.forward, localTargetDir, Vector3.up);
        angle = Mathf.Clamp(angle, _data.minYawAngle, _data.maxYawAngle);

        localTargetDir = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;

        return Quaternion.LookRotation(localTargetDir, Vector3.up);
    }

}