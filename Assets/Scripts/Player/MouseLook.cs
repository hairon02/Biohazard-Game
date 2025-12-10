using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun; // <--- Importar

public class MouseLook : MonoBehaviourPun
{
    [Header("References")]
    public Transform cameraHolder;

    [Header("Mouse Settings")]
    public float sensitivity = 20f;
    public float minPitch = -80f;
    public float maxPitch = 80f;

    float xRotation = 0f;

    void Start()
    {
        if (!photonView.IsMine)
        {
            // 1. Apagar la cámara (para no ver desde sus ojos)
            Camera cam = cameraHolder.GetComponent<Camera>(); // O GetComponentInChildren
            if (cam != null) cam.enabled = false;

            // 2. Apagar el AudioListener (para no escuchar doble)
            AudioListener audioL = cameraHolder.GetComponent<AudioListener>(); // O GetComponentInChildren
            if (audioL != null) audioL.enabled = false;

            // 3. Apagar este script
            this.enabled = false;
            return;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (cameraHolder == null)
        {
            Debug.LogError("CameraHolder no asignado en PlayerLook");
        }
    }

    void Update()
    {
        Look();
    }

    void Look()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        float mouseX = mouseDelta.x * sensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * sensitivity * Time.deltaTime;

        // Rotación vertical (cámara)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minPitch, maxPitch);
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotación horizontal (player)
        transform.Rotate(Vector3.up * mouseX);
    }
}
