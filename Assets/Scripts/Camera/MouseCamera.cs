using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;

public class MouseCamera : MonoBehaviour
{
    private Camera mCamera;
    public Transform LookAtPosition = null;

    private void Awake()
    {
        mCamera = GetComponent<Camera>();
        if (mCamera == null)
        {
            Debug.LogError(GetType() + "camera Get Error ……");
        }

    }
    private void Start()
    {
        /*Lookatposition();*/
    }

    private void LateUpdate()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {

            if (UnityEngine.Input.GetKey(KeyCode.LeftAlt))
            {
                CameraRotate();

                CameraFOV();

                CameraMove();
            }
        }

    }

    #region Camera Rotation


    public int yRotationMinLimit = -70;
    public int yRotationMaxLimit = 70;

    public float xRotationSpeed = 250.0f;
    public float yRotationSpeed = 120.0f;

    private float xRotation = 0.0f;
    private float yRotation = 0.0f;
    private float angle = 0.0f;


    void CameraRotate()
    {
        if (UnityEngine.Input.GetMouseButton(0))
        {

            xRotation = UnityEngine.Input.GetAxis("Mouse X") * xRotationSpeed * 0.02f;
            yRotation = -1f * UnityEngine.Input.GetAxis("Mouse Y") * yRotationSpeed * 0.02f;

            /*            mCamera.transform.RotateAround(LookAtPosition.localPosition, Vector3.up, xRotation);*/
            Quaternion q1 = Quaternion.AngleAxis(xRotation, Vector3.up);

            Matrix4x4 r1 = Matrix4x4.Rotate(q1);
            Matrix4x4 invP1 = Matrix4x4.TRS(-LookAtPosition.localPosition, Quaternion.identity, Vector3.one);
            r1 = invP1.inverse * r1 * invP1;
            Vector3 newCameraPos1 = r1.MultiplyPoint(transform.localPosition);
            transform.localPosition = newCameraPos1;

            // transform.LookAt(LookAtPosition);
            transform.localRotation = q1 * transform.localRotation;

            Vector3 C = transform.localPosition - LookAtPosition.transform.localPosition;

            if ((Vector3.Dot(C.normalized, Vector3.up) > 0.9848f) && (yRotation>0f)) // this is about 80-degrees
            {
                return;
            }
            if ((Vector3.Dot(C.normalized, Vector3.up) < -0.9848f) && (yRotation < 0f)) // this is about 80-degrees
            {
                return;
            }

            Quaternion q = Quaternion.AngleAxis(yRotation, transform.right);

            Matrix4x4 r = Matrix4x4.Rotate(q);
            Matrix4x4 invP = Matrix4x4.TRS(-LookAtPosition.localPosition, Quaternion.identity, Vector3.one);
            r = invP.inverse * r * invP;
            Vector3 newCameraPos = r.MultiplyPoint(transform.localPosition);
            transform.localPosition = newCameraPos;

            // transform.LookAt(LookAtPosition);
            transform.localRotation = q * transform.localRotation;

/*            mCamera.transform.RotateAround(LookAtPosition.localPosition, transform.right, yRotation);*/

        }
    }

    #endregion

    #region Camera fov


    public float fovSpeed = 50.0f;

    // change distance between lookat and camera
    public void CameraFOV()
    {

        float wheel = UnityEngine.Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime  * fovSpeed;
        Vector3 line = LookAtPosition.localPosition - mCamera.transform.localPosition;
        float dist = line.magnitude;
        Vector3 dir = line.normalized;
        Vector3 newpos = mCamera.transform.localPosition + wheel * dir;
        Vector3 newline = LookAtPosition.localPosition - newpos;
        bool flag = true;
        if(Vector3.Dot(line,newline) < 0f)
                flag = false;
        float newdist = newline.magnitude;
        
        if(newdist > 0.5f && flag)
        {
            mCamera.transform.localPosition = newpos;
        }


    }

    #endregion


    #region Camera Move

    float _mouseX = 0;
    float _mouseY = 0;
    public float moveSpeed = 1;

    // change camera pos(camera's X and Y direction) and Lookat pos
    public void CameraMove()
    {
        if (UnityEngine.Input.GetMouseButton(1))
        {
            _mouseX = -1f * UnityEngine.Input.GetAxis("Mouse X") ;
            _mouseY = -1f * UnityEngine.Input.GetAxis("Mouse Y") ;

            Vector3 moveDir = (_mouseX * mCamera.transform.right + _mouseY * mCamera.transform.up);
            mCamera.transform.position += 0.1f*moveDir;
            LookAtPosition.transform.position += moveDir;

        }

    }

    #endregion

    #region tools ClampValue


    float ClampValue(float value, float min, float max)
    {
        if (value < -360)
            value += 360;
        if (value > 360)
            value -= 360;
        return Mathf.Clamp(value, min, max);
    }

    public void Lookatposition()
    {
        // Viewing vector is from transform.localPosition to the lookat position
        Vector3 V = LookAtPosition.localPosition - transform.localPosition;
        Vector3 W = Vector3.Cross(-V, Vector3.up);
        Vector3 U = Vector3.Cross(W, -V);
        // transform.localRotation = Quaternion.LookRotation(V, U);
        transform.localRotation = Quaternion.FromToRotation(Vector3.up, U);
        Quaternion alignU = Quaternion.FromToRotation(transform.forward, V);
        transform.localRotation = alignU * transform.localRotation;
    }

    #endregion
}