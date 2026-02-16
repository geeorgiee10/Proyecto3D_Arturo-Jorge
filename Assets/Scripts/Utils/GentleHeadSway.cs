using UnityEngine;

public class GentleHeadSway : MonoBehaviour
{
    [Header("Sway Settings")]
    public float pitchAngle = 1.5f;   // Máximo ángulo de cabeceo
    public float yawAngle   = 0.5f;   // Giro leve en eje horizontal
    public float swaySpeed  = 0.5f;   // Velocidad del movimiento

    void LateUpdate()
    {
        // Tomamos la rotación “real” actual del objeto
        Quaternion lookRotation = transform.rotation;

        // Calculamos un sway suave
        float swayPitch = Mathf.Sin(Time.time * swaySpeed) * pitchAngle;
        float swayYaw   = Mathf.Sin(Time.time * swaySpeed * 0.7f) * yawAngle;

        // Aplicamos el sway **sobre la rotación actual sin acumular errores**
        transform.rotation = lookRotation * Quaternion.Euler(swayPitch, swayYaw, 0f);
    }
}
