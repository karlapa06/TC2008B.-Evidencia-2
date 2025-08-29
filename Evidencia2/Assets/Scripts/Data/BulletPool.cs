using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance; // Singleton para acceder fácil
    public GameObject bulletPrefab;
    public int poolSize = 100;

    private List<GameObject> bulletPool;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Inicializamos la pool
        bulletPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }

    public GameObject GetBullet()
    {
        foreach (var bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }

        // Si no hay libres → creamos uno nuevo (expansión controlada)
        GameObject newBullet = Instantiate(bulletPrefab);
        bulletPool.Add(newBullet);
        return newBullet;
    }

    public int GetActiveBulletCount()
    {
        int count = 0;
        foreach (var bullet in bulletPool)
        {
            if (bullet.activeInHierarchy) count++;
        }
        return count;
    }
}
