using UnityEngine;

[ExecuteAlways]
public class CameraRectFitter : MonoBehaviour
{
    const float MOVE_AMOUNT_PER_FRAME = 0.3f;

    [SerializeField] Transform[] _thingsToContain;

    Camera _camera;
    Vector3 _direction;
    Vector3 _centrePoint;
    bool _settled;

    int _lastScreenWidth = Screen.width;
    int _lastScreenHeight = Screen.height;

    public void Awake()
    {
        CalculateDirection();
        CalculateCentrePoint();
    }

    public void OnEnable() =>
        _camera = GetComponent<Camera>();

    public void Update()
    {
        if (ScreenSizeChanged())
            _settled = false;
        _lastScreenWidth = Screen.width;
        _lastScreenHeight = Screen.height;

        if (_settled)
            return;

        var tfm = transform;

        if (CanSeeAllThings())
        {
            var curPos = tfm.position;
            tfm.position = curPos + _direction * MOVE_AMOUNT_PER_FRAME;
            if (CanSeeAllThings())
                return;

            tfm.position = curPos;
            _settled = true;
        }
        else tfm.position -= _direction * MOVE_AMOUNT_PER_FRAME;
    }

    public void OnValidate()
    {
        _settled = false;
        CalculateDirection();
        CalculateCentrePoint();
    }

    void CalculateDirection() =>
        _direction = transform.rotation * Vector3.forward;

    void CalculateCentrePoint()
    {
        if (!(_thingsToContain?.Length > 0))
            return;

        var centre = _thingsToContain[0].position;
        for (var i = 1; i != _thingsToContain.Length; ++i)
            centre += _thingsToContain[i].position;

        _centrePoint = centre / _thingsToContain.Length;
    }

    bool ScreenSizeChanged() =>
        _lastScreenWidth != Screen.width || _lastScreenHeight != Screen.height;

    bool CanSeeAllThings()
    {
        foreach (var target in _thingsToContain)
        {
            var viewportPoint = _camera.WorldToViewportPoint(target.position);
            if (viewportPoint.x is < 0 or > 1 || viewportPoint.y is < 0 or > 1 || viewportPoint.z <= 0)
                return false;
        }

        return true;
    }
}
