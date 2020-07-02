using UnityEngine;
using System.Collections;


/*
 *   The script controls the camera rotation
 */
public class MouseLook : MonoBehaviour
{
    public Transform _transform;
    // settings for turning different parts of the tank
    public enum RotationAxes
    {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }

    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityHor = 9.0f;
    public float sensitivityVert = 9.0f;
    public float minimumVert = -45.0f;
    public float maximumVert = 45.0f;
    private float _rotationX = 0;

    void Start()
    {
        _transform = transform;
        Rigidbody body = GetComponent<Rigidbody>();
        if (body != null)
            body.freezeRotation = true;
    }

    void Update()
    {
        if (axes == RotationAxes.MouseX) { // turns only vertically
            _transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityHor, 0);
        } else if (axes == RotationAxes.MouseY) { // turns only horizontally
            _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
            _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);
            float rotationY = transform.localEulerAngles.y;
            _transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
        } else { // rotates horizontally and horizontally
            _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
            _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);

            float delta = Input.GetAxis("Mouse X") * sensitivityHor;
            float rotationY = transform.localEulerAngles.y + delta;

            _transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);            
        }
    }
}