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
    public float maxSpeed = 300.0f;
    public float brakeAcceleration = 50.0f;

    public float turnSensitivity = 1.0f;
    public float maxSteerAngle = 30.0f;

    public Vector3 _centerOfMass;

    public List<Wheel> wheels;

    public float moveInput;
    public float steerInput;
    public bool isBraking;

    private Rigidbody carRb;

    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;
        //carRb.maxLinearVelocity = maxSpeed;
    }


    void FixedUpdate()
    {
        Move(moveInput);
        Steer(steerInput);
        Brake(moveInput);

        Debug.Log(carRb.linearVelocity.magnitude);
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


    public void Move(float movement)
    {
        float speed = carRb.linearVelocity.magnitude;

        foreach (var wheel in wheels)
        {
            if (speed < maxSpeed)
            {
                wheel.wheelCollider.motorTorque =
                    movement * 600 * maxAcceleration * Time.fixedDeltaTime;
            }
            else
            {
                wheel.wheelCollider.motorTorque = 0f;
                speed = maxSpeed;
            }
        }
    }

    public void Steer(float steering)
    {
        float speed = carRb.linearVelocity.magnitude;
        float speedSteerFactor = Mathf.Lerp(1f, 0.3f, speed / maxSpeed);

        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                float targetAngle =
                    steering * turnSensitivity * maxSteerAngle * speedSteerFactor;

                wheel.wheelCollider.steerAngle =
                    Mathf.Lerp(
                        wheel.wheelCollider.steerAngle,
                        targetAngle,
                        0.6f
                    );
            }
        }
    }

    public void Brake(float movement)
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
