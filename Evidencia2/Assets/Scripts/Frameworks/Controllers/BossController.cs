using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BossController : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float xLimit = 7f;
    public float yLimit = 4f;

    [Header("Disparo")]
    public Transform[] firePoints;       // Puntos de disparo para patrón lineal
    public float bulletSpeed = 5f;       // Velocidad de las balas

    [Header("Colores")]
    public Color circularColor = Color.red;
    public Color spiralColor = Color.blue;
    public Color lineColor = Color.green;

    private int currentPattern = 0;

    private void OnEnable()
    {
        TimeManager.OnPatternChange += NextPattern;
    }

    private void OnDisable()
    {
        TimeManager.OnPatternChange -= NextPattern;
    }

    private void Start()
    {
        StartCoroutine(PatternRoutine());
    }

    private void Update()
    {
        HandleMovement();
    }

    // --- Movimiento con teclado ---
    private void HandleMovement()
    {
        float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.RightArrow)) moveX = 1f;
        if (Input.GetKey(KeyCode.LeftArrow))  moveX = -1f;
        if (Input.GetKey(KeyCode.UpArrow))    moveY = 1f;
        if (Input.GetKey(KeyCode.DownArrow))  moveY = -1f;

        Vector3 movement = new Vector3(moveX, moveY, 0f) * speed * Time.deltaTime;
        transform.Translate(movement);

        // Limitar movimiento dentro de la pantalla
        float clampedX = Mathf.Clamp(transform.position.x, -xLimit, xLimit);
        float clampedY = Mathf.Clamp(transform.position.y, -yLimit, yLimit);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    // --- Cambiar patrón ---
    private void NextPattern()
    {
        currentPattern = (currentPattern + 1) % 3;
    }

    // --- Coroutine principal que controla los patrones ---
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
            NextPattern();
        }
    }

    // --- Patrón 1: Circular ---
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

    // --- Patrón 2: Espiral ---
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

    // --- Patrón 3: Lineal con movimiento ---
    private IEnumerator PatternLineMoving(float duration)
    {
        float timer = 0f;
        float moveAmplitude = 2f;
        float moveSpeed = 2f;

        while (timer < duration)
        {
            float offsetX = Mathf.Sin(Time.time * moveSpeed) * moveAmplitude;
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

    // --- Función que instancia y activa la bala ---
    private void SpawnBullet(Vector3 direction, Vector3 position, Color color)
    {
        GameObject bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = position;
        bullet.SetActive(true);

        Bullet b = bullet.GetComponent<Bullet>();
        b.SetMovementDirection(direction);
        b.SetMovementSpeed(bulletSpeed);

        // Color visual
        SpriteRenderer sr = bullet.GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = color;

        // Trail opcional
        TrailRenderer tr = bullet.GetComponent<TrailRenderer>();
        if (tr != null)
        {
            tr.enabled = true;
            tr.startColor = color;
            tr.endColor = new Color(color.r, color.g, color.b, 0f);
        }
    }
}
