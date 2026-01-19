using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axel axel;
    }

    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;

    public float turnSensitivity = 1.0f;
    public float maxSteerAngle = 30.0f;

    public Vector3 _centerOfMass;

    public List<Wheel> wheels;

    float moveInput;
    float steerInput;
    bool isBraking;

    private Rigidbody carRb;

    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;
    }


    void FixedUpdate()
    {
        Move();
        Steer();
        Brake();
    }


    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<float>();
    }

    public void OnSteer(InputAction.CallbackContext ctx)
    {
        steerInput = ctx.ReadValue<float>();
    }

    public void OnBrake(InputAction.CallbackContext ctx)
    {
        isBraking = ctx.ReadValue<float>() > 0.1f;
    }

    // -------- CAR LOGIC --------

    void Move()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque =
                moveInput * 600 * maxAcceleration * Time.fixedDeltaTime;
        }
    }

    void Steer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                float targetAngle =
                    steerInput * turnSensitivity * maxSteerAngle;

                wheel.wheelCollider.steerAngle =
                    Mathf.Lerp(
                        wheel.wheelCollider.steerAngle,
                        targetAngle,
                        0.6f
                    );
            }
        }
    }

    void Brake()
    {
        if (isBraking || Mathf.Abs(moveInput) < 0.01f)
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque =
                    300 * brakeAcceleration * Time.fixedDeltaTime;
            }
        }
        else
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.brakeTorque = 0f;
            }
        }
    }
}
