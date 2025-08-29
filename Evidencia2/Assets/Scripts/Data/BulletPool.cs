using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gestiona la pool de balas para optimizar la creación y destrucción de objetos.
/// Permite obtener balas activas y desactivarlas al salir de pantalla o al terminar su lifetime.
/// </summary>
public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;
    public GameObject bulletPrefab;
    public int poolSize = 200;


    private List<GameObject> bulletPool;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);


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
