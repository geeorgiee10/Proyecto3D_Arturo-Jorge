using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [Header("Referencias")]
    public Transform cameraPivot;
    public Transform cameraTransform;

    [Header("Sensibilidad")]
    public float mouseSensitivity = 90f;

    [Header("Límites de cámara")]
    public float minPitch = -25f;
    public float maxPitch = 35f;

    [Header("Suavizado (God of War feel)")]
    public float rotationSmoothTime = 0.08f;

    private Vector2 lookInput;
    private float pitch;
    private float yaw;

    private float pitchVelocity;
    private float yawVelocity;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float delaySeconds = 2f;

    private Renderer[] renderers;

    private void Awake()
    {
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        if (cameraPivot == null && cameraTransform != null)
            cameraPivot = cameraTransform.parent;

        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();

        renderers = GetComponentsInChildren<Renderer>(true);
        Ocultar();
    }

    void Start()
    {
        yaw = transform.eulerAngles.y;
        pitch = 0f;

        cameraPivot.localRotation = Quaternion.identity;
        StartCoroutine(StartInput());
    }

    IEnumerator StartInput()
    {
        yield return new WaitForSeconds(delaySeconds);
        Mostrar();
        playerInput?.ActivateInput();
    }

    private void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    void Update()
    {
        HandleLook();
    }

    private void HandleLook()
    {
        float targetYaw = yaw + lookInput.x * mouseSensitivity * Time.deltaTime;
        float targetPitch = pitch - lookInput.y * mouseSensitivity * Time.deltaTime;

        targetPitch = Mathf.Clamp(targetPitch, minPitch, maxPitch);

        yaw = Mathf.SmoothDampAngle(yaw, targetYaw, ref yawVelocity, rotationSmoothTime);
        pitch = Mathf.SmoothDampAngle(pitch, targetPitch, ref pitchVelocity, rotationSmoothTime);

        // Rotación del personaje (yaw)
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        // Rotación vertical solo en el pivot
        cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private void Ocultar()
    {
        foreach (var r in renderers)
            r.enabled = false;
    }

    private void Mostrar()
    {
        foreach (var r in renderers)
            r.enabled = true;
    }
}
