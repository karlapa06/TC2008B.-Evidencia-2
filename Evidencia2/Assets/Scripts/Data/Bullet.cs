using UnityEngine;

/// <summary>
/// Controla el comportamiento de cada bala:
/// - Movimiento en la dirección especificada
/// - Desactivación automática al terminar su lifetime o al salir de la pantalla
/// </summary>
public class Bullet : MonoBehaviour
{
    private Vector3 movementDirection;
    private float movementSpeed = 5f;
    public float bulletLifetime = 5f;

    // Se llama al activar la bala
    private void OnEnable()
    {
        Invoke(nameof(Deactivate), bulletLifetime);
    }

    // Movimiento y control de salida de pantalla 
    private void Update()
    {
        transform.Translate(movementDirection * movementSpeed * Time.deltaTime);
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewportPos.x < 0f || viewportPos.x > 1f || viewportPos.y < 0f || viewportPos.y > 1f)
        {
            Deactivate();
        }
    }

    //  Define la dirección de movimiento de la bala 
    public void SetMovementDirection(Vector3 dir)
    {
        movementDirection = dir.normalized;
    }

    // Ajusta la velocidad de la bala 
    public void SetMovementSpeed(float speed)
    {
        movementSpeed = speed;
    }

    //  Desactiva la bala y la devuelve al pool 
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    // Limpia cualquier Invoke pendiente al desactivar la bala
    private void OnDisable()
    {
        CancelInvoke();
    }
}
