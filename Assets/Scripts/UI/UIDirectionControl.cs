using UnityEngine;

public class UIDirectionControl : MonoBehaviour
{
    public bool UseRelativeRotation = true;  


    private Quaternion _relativeRotation;
    private Transform _transform;

    private void Start()
    {
        _transform = transform;
        _relativeRotation = _transform.parent.localRotation;
    }


    private void Update()
    {
        if (UseRelativeRotation)
            _transform.rotation = _relativeRotation;
    }
}
