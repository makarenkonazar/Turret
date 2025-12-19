using UnityEngine;
using UnityEngine.UIElements;

public class TurretBarrel : MonoBehaviour
{
    private TurretData _data;

    public void Initialize(TurretData data)
    {
        _data = data;
    }

    public void SetTarget(Vector3 target)
    {
        LocalRotateToAngle(target);
    }

    private void LocalRotateToAngle(Vector3 target)
    {
        Quaternion targetRotation = CalculateLocalTarget(target);

        transform.localRotation = Quaternion.RotateTowards(
            transform.localRotation,
            targetRotation,
            _data.BarrelSpeed * Time.deltaTime
        );
    }

    private Quaternion CalculateLocalTarget(Vector3 target)
    {
        Vector3 localTargetDir = transform.parent.InverseTransformPoint(target) - transform.localPosition;
        float pitch = Vector3.Angle(Vector3.up, localTargetDir);
        pitch = Mathf.Clamp(pitch, _data.minPitchAngle, _data.maxPitchAngle) - 90f;
        return Quaternion.Euler(pitch, 0f, 0f);
    }
}