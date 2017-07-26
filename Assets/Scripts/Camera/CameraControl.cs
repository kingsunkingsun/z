using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float DampTime = 0.2f;                 
    public float ScreenEdgeBuffer = 4f;           
    public float MinSize = 6.5f;                  
    [HideInInspector] public Transform[] Targets; 


    private Camera _camera;                        
    private float _zoomSpeed;                      
    private Vector3 _moveVelocity;                 
    private Vector3 _desiredPosition;
    private Transform _transform;

    private void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
        _transform = transform;
    }


    private void FixedUpdate()
    {
        Move();
        Zoom();
    }


    private void Move()
    {
        FindAveragePosition();

        _transform.position = Vector3.SmoothDamp(_transform.position, _desiredPosition, ref _moveVelocity, DampTime);
    }


    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        for (int i = 0; i < Targets.Length; i++)
        {
            if (!Targets[i].gameObject.activeSelf)
                continue;

            averagePos += Targets[i].position;
            numTargets++;
        }

        if (numTargets > 0)
            averagePos /= numTargets;

        averagePos.y = _transform.position.y;

        _desiredPosition = averagePos;
    }


    private void Zoom()
    {
        float requiredSize = FindRequiredSize();
        _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, requiredSize, ref _zoomSpeed, DampTime);
    }


    private float FindRequiredSize()
    {
        Vector3 desiredLocalPos = _transform.InverseTransformPoint(_desiredPosition);

        float size = 0f;

        for (int i = 0; i < Targets.Length; i++)
        {
            if (!Targets[i].gameObject.activeSelf)
                continue;

            Vector3 targetLocalPos = transform.InverseTransformPoint(Targets[i].position);

            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.y));

            size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.x) / _camera.aspect);
        }
        
        size += ScreenEdgeBuffer;

        size = Mathf.Max(size, MinSize);

        return size;
    }


    public void SetStartPositionAndSize()
    {
        FindAveragePosition();

        _transform.position = _desiredPosition;

        _camera.orthographicSize = FindRequiredSize();
    }
}