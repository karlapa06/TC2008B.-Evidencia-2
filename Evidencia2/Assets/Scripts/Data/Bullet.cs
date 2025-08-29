using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 movementDirection; // Dirección de la bala
    private float movementSpeed = 5f;  // Velocidad de la bala
    public float bulletLifetime = 3f;  // Tiempo antes de desactivarse

    private void OnEnable()
    {
        // Cuando se activa, programamos su destrucción
        Invoke(nameof(Deactivate), bulletLifetime);
    }

    public void SetMovementDirection(Vector3 dir)
    {
        movementDirection = dir.normalized;
    }

    public void SetMovementSpeed(float speed)
    {
        movementSpeed = speed;
    }

    private void Update()
    {
        // Movimiento constante en la dirección asignada
        transform.Translate(movementDirection * movementSpeed * Time.deltaTime, Space.World);
    }

    private void Deactivate()
    {
        // En vez de destruir → se desactiva (para el pool)
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        // Cancelar invocaciones pendientes para evitar errores
        CancelInvoke();
    }
}
