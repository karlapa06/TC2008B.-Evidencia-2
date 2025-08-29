using System.Collections;
using UnityEngine;

/// <summary>
/// Controla al jefe del juego: 
/// - Movimiento con teclado (flechas) 
/// - Disparo de tres patrones mecánicamente distintos (circular, espiral y lineal con movimiento)
/// </summary>
public class BossController : MonoBehaviour
{
    // --- Movimiento del boss ---
    [Header("Movimiento")]
    public float moveSpeed = 5f; 

    // --- Disparo ---
    [Header("Disparo")]
    public Transform[] firePoints;          
    private int currentPattern = 0;         

    [Header("Colores de patrones")]
    public Color circularColor = Color.red;
    public Color spiralColor = Color.blue;
    public Color lineColor = Color.green;

    private void Start()
    {
        StartCoroutine(PatternRoutine());
    }

    private void Update()
    {
        HandleMovement();
    }

    // Manejo de movimiento con teclado 
    private void HandleMovement()
    {
        float moveX = 0f;
        float moveY = 0f;

        // Detecta teclas de flecha
        if (Input.GetKey(KeyCode.RightArrow)) moveX = 1f;
        if (Input.GetKey(KeyCode.LeftArrow)) moveX = -1f;
        if (Input.GetKey(KeyCode.UpArrow)) moveY = 1f;
        if (Input.GetKey(KeyCode.DownArrow)) moveY = -1f;

        Vector3 moveDir = new Vector3(moveX, moveY, 0).normalized;

        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }

    // Patrones
    private IEnumerator PatternRoutine()
    {
        while (true)
        {
            switch (currentPattern)
            {
                case 0: yield return StartCoroutine(PatternCircular(10f)); break;
                case 1: yield return StartCoroutine(PatternSpiral(10f)); break;
                case 2: yield return StartCoroutine(PatternLineMoving(10f)); break;
            }

            // Cambia al siguiente patrón (rotativo)
            currentPattern = (currentPattern + 1) % 3;
        }
    }

    // Patron 1
    private IEnumerator PatternCircular(float duration)
    {
        float timer = 0f;
        int bulletsPerShot = 12;

        while (timer < duration)
        {
            float rotationOffset = Time.time * 50f;

            for (int i = 0; i < bulletsPerShot; i++)
            {
                float angle = i * (360f / bulletsPerShot) + rotationOffset;
                Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.up;
                SpawnBullet(dir, transform.position, circularColor);
            }

            timer += 0.5f;
            yield return new WaitForSeconds(0.5f);
        }
    }

    // Patron 2
    private IEnumerator PatternSpiral(float duration)
    {
        float timer = 0f;
        int bulletsPerShot = 6;
        float angleStep = 30f;
        float currentAngle = 0f;

        while (timer < duration)
        {
            for (int i = 0; i < bulletsPerShot; i++)
            {
                Vector3 dir = Quaternion.Euler(0, 0, currentAngle + i * angleStep) * Vector3.up;
                SpawnBullet(dir, transform.position, spiralColor);
            }

            currentAngle += 15f; 
            timer += 0.2f;
            yield return new WaitForSeconds(0.2f);
        }
    }

    // Patron 3
    private IEnumerator PatternLineMoving(float duration)
    {
        float timer = 0f;
        float moveAmplitude = 2f;       
        float moveSpeedPattern = 2f;    

        while (timer < duration)
        {
            float offsetX = Mathf.Sin(Time.time * moveSpeedPattern) * moveAmplitude;
            Vector3 spawnPos = transform.position + new Vector3(offsetX, 0, 0);

            foreach (var point in firePoints)
            {
                Vector3 dir = Vector3.down;
                Vector3 bulletPos = spawnPos + (point.position - transform.position);
                SpawnBullet(dir, bulletPos, lineColor);
            }

            timer += 0.3f;
            yield return new WaitForSeconds(0.3f);
        }
    }

    // Activacion de balas
    private void SpawnBullet(Vector3 direction, Vector3 position, Color color)
    {
        GameObject bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = position;
        bullet.SetActive(true);

        Bullet b = bullet.GetComponent<Bullet>();
        b.SetMovementDirection(direction);

        SpriteRenderer sr = bullet.GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = color;

        TrailRenderer tr = bullet.GetComponent<TrailRenderer>();
        if (tr != null)
        {
            tr.enabled = true;
            tr.startColor = color;
            tr.endColor = new Color(color.r, color.g, color.b, 0f);
        }
    }
}
