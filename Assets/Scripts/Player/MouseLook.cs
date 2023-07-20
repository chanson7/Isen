using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    Vector2 mouseDelta;
    float xRotation = 0f;
    [SerializeField] float xMin;
    [SerializeField] float xMax;
    public float mouseSensitivity;
    [SerializeField] Transform cameraTarget;

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
        xRotation -= mouseDelta.y;
        xRotation = Mathf.Clamp(xRotation, xMin, xMax);

        cameraTarget.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up, mouseDelta.x);
    }
}
