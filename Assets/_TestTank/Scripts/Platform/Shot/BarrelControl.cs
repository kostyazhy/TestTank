﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Class controls the rotation of the barrel and makes a shot
 */

public class BarrelControl : MonoBehaviour {
    //use Crosshair.cs
    public delegate void LocateTarget(bool goodTarget);
    public static event LocateTarget LocatedTargetEvent;

    //use MovePlatform.cs
    public delegate void RotatePlatform(float valueRotetate);
    public static event RotatePlatform RotatePlatformEvent;

    [SerializeField]
    GameObject shellPrefab;

    public Transform cameraTrf;
    public Transform barrel;

    float gravity = -Physics.gravity.y;
    public float speed = 60f;
    bool shotReady = false;
    Vector3 angelShot;

    void Update()
    {
        Ray ray = new Ray(cameraTrf.position, cameraTrf.forward * 5000);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.name != null) {
                shotReady = Launch(hit.point, ref angelShot);
                if (shotReady) {
                    RotateBarrel(angelShot);
                    ToGateOfPlatform();
                }
                LocatedTargetEvent?.Invoke(shotReady);

            }
        } else {
            LocatedTargetEvent?.Invoke(false);
            shotReady = false;
        }
        if (Input.GetMouseButtonDown(1) && shotReady) {
                StartCoroutine(CreateShell());
        }        
    }

    // looking for a suitable angle for the shot
    public bool Launch(Vector3 targetPoint, ref Vector3 angelShot)
    {
        Vector3 launchPoint = barrel.position;
        // finding the distance and direction to the target
        Vector2 dir;
        dir.x = targetPoint.x - launchPoint.x;
        dir.y = targetPoint.z - launchPoint.z;
        float x = dir.magnitude;
        float y = -launchPoint.y;
        dir /= x;

        float s2 = speed * speed;

        float r = s2 * s2 - gravity * (gravity * x * x + 2f * y * s2);
        if (r < 10f) {
            Debug.Log("Launch velocity insufficient for range!");
            return false;
        }
        // find the angles of inclination for barrel
        float tanTheta = (s2 + Mathf.Sqrt(r)) / (gravity * x);
        float cosTheta = Mathf.Cos(Mathf.Atan(tanTheta));
        float sinTheta = cosTheta * tanTheta;
        float angel = Mathf.Atan(tanTheta) * (180 / Mathf.PI);
        if (angel >= 80 || angel < 0) {
            Debug.Log("The angle is impossible!");
            return false;
        }
        angelShot = new Vector3(speed * cosTheta * dir.x, speed * sinTheta, speed * cosTheta * dir.y);

        return true;
    }

    // define the horizontal limit and if necessary turn the platform around
    bool ToGateOfPlatform() {
        float _angel = Vector3.Angle(transform.forward, cameraTrf.forward);
        if (_angel > 20) {
            Vector3 targetPos = cameraTrf.position;
            targetPos.y = transform.position.y;
            Vector3 dir = targetPos - transform.position;
            Vector3 forward = transform.forward;

            float between = Vector3.SignedAngle(dir, forward, Vector3.up);
            if (between > 0)
                RotatePlatformEvent?.Invoke(1);
            else
                RotatePlatformEvent?.Invoke(-1);
            return false;
        }
        return true;
    }

    // turning the barrel
    void RotateBarrel(Vector3 angel)
    {
        barrel.rotation = Quaternion.LookRotation(angel);
    }

    // creating a shell when fired
    IEnumerator CreateShell()
    {
        GameObject shell = Instantiate(shellPrefab, barrel.position, Quaternion.identity);
        Transform shellTransform = shell.transform;
        Shell shellScript = shell.GetComponent<Shell>();
        yield return new WaitForSeconds(0.5f);
        shellScript.Shoot(barrel, speed);
    }
}
