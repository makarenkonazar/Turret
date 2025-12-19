using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TurretController : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _maxFocusDistance = 50f;

    [SerializeField] private bool _lookAtCamera;
    private Turret _turret;

    void Awake()
    {
        _turret = GetComponent<Turret>();
    }

    void Update()
    {
        if (!_lookAtCamera)
        {
            Vector3 target = UpdateTargetPosition();
            _turret.SetTarget(target);
        }
        else
        {
            _turret.SetTarget(_cameraTransform.position);
        }
    }

    private Vector3 UpdateTargetPosition()
    {
        if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit, _maxFocusDistance))
        {
            return hit.point;
        }
        else
        {
            return _cameraTransform.position + _cameraTransform.forward * _maxFocusDistance;
        }
    }
}