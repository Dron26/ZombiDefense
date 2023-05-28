using System.Collections;
using System.Collections.Generic;
using Plugins.Joystick_Pack.Scripts.Joysticks;
using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
    public float speed;
    public DynamicJoystick _dynamicJoystick;
    public Rigidbody rb;

    public void FixedUpdate()
    {
        Vector3 direction = Vector3.forward * _dynamicJoystick.Vertical + Vector3.right * _dynamicJoystick.Horizontal;
        rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }
}