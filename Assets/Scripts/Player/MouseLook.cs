using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    protected Vector2 mouseDelta;
    protected float xRotation = 0f;
    [SerializeField] protected Transform cameraTarget;
    public float mouseSensitivity;
    [SerializeField] protected float xMin;
    [SerializeField] protected float xMax;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnLook(InputValue input)
    {
        mouseDelta.x = input.Get<Vector2>().x * mouseSensitivity * Time.deltaTime;
        mouseDelta.y = input.Get<Vector2>().y * mouseSensitivity * Time.deltaTime;
    }

    void Update()
    {
        DoRotate();
    }

    public virtual void DoRotate()
    {
        xRotation -= mouseDelta.y;
        xRotation = Mathf.Clamp(xRotation, xMin, xMax);

        cameraTarget.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up, mouseDelta.x);
    }
}
