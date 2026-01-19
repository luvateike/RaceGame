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
        Move(moveInput);
        Steer(steerInput);
        Brake(moveInput);
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


    void Move(float movement)
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque =
                movement * 600 * maxAcceleration * Time.fixedDeltaTime;
        }
    }

    void Steer(float movement)
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                float targetAngle =
                    movement * turnSensitivity * maxSteerAngle;

                wheel.wheelCollider.steerAngle =
                    Mathf.Lerp(
                        wheel.wheelCollider.steerAngle,
                        targetAngle,
                        0.6f
                    );
            }
        }
    }

    void Brake(float movement)
    {
        if (isBraking || Mathf.Abs(movement) < 0.01f)
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
