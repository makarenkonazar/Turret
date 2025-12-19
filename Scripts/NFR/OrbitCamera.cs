using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _distance = 10f;
    [SerializeField] private float _zoomSpeed = 2f;
    [SerializeField] private float _minDistance = 3f;
    [SerializeField] private float _maxDistance = 20f;

    [SerializeField] private float _rotationSpeed = 100f;
    [SerializeField] private float _minYAngle = -20f;
    [SerializeField] private float _maxYAngle = 80f;

    private float _yaw = 0f;
    private float _pitch = 20f;

    void LateUpdate()
    {
        if (!_target) return;

        if (Input.GetMouseButton(1))
        {
            _yaw += Input.GetAxis("Mouse X") * _rotationSpeed * Time.deltaTime;
            _pitch -= Input.GetAxis("Mouse Y") * _rotationSpeed * Time.deltaTime;
            _pitch = Mathf.Clamp(_pitch, _minYAngle, _maxYAngle);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        _distance -= scroll * _zoomSpeed;
        _distance = Mathf.Clamp(_distance, _minDistance, _maxDistance);

        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -_distance);

        transform.position = _target.position + offset;
        transform.LookAt(_target);
    }
}
