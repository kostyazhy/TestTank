﻿ using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour {

    Transform _transform;
    private CharacterController _charController;
    public float speed = 10f;
    public float speedRotate = 5f;
    float gravity = Physics.gravity.y;
    Vector3 moveDirection = new Vector3(0, 0, 0);

    private void OnEnable()
    {
        Shot.RotatePlatformEvent += RotatePlatform;
    }


    private void OnDisable()
    {
        Shot.RotatePlatformEvent -= RotatePlatform;
    }

    void Start () {
        _transform = transform;
        _charController = GetComponent<CharacterController>();
    }

    void Update()
    {
        moveDirection = new Vector3(0, moveDirection.y, Input.GetAxis("Vertical"));
        moveDirection.y = gravity;
        moveDirection *= speed;
        moveDirection = _transform.TransformDirection(moveDirection);
        _charController.Move(moveDirection * Time.deltaTime);
        _transform.Rotate(0, Input.GetAxis("Horizontal") * speedRotate, 0, Space.World);
    }

    void RotatePlatform(float value)
    {
        _transform.Rotate(0, value * speedRotate, 0, Space.World);
    }

}
