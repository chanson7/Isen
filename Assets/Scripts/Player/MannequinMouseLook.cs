using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//this class exists because mannequins should not be able to rotate when they are idle
public class MannequinMouseLook : MouseLook
{

    bool isMoving = false;
    float yRotation = 0f;
    [SerializeField] float yMin;
    [SerializeField] float yMax;

    void OnMove(InputValue input)
    {
        isMoving = input.Get<Vector2>().magnitude > 0 ? true : false;
    }

    public override void DoRotate()
    {
        if (isMoving)
        {
            xRotation -= mouseDelta.y;
            xRotation = Mathf.Clamp(xRotation, xMin, xMax);

            cameraTarget.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
            transform.Rotate(Vector3.up, mouseDelta.x);

        }
        else
        {
            xRotation -= mouseDelta.y;
            xRotation = Mathf.Clamp(xRotation, xMin, xMax);

            yRotation += mouseDelta.x;
            yRotation = Mathf.Clamp(yRotation, yMin, yMax);

            cameraTarget.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
    }
}
