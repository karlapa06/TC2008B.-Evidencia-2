using UnityEngine;
using TMPro;

public class BulletCounterUI : MonoBehaviour
{
    public TMP_Text bulletCounterText;

    private void Update()
    {
        // Muestra en tiempo real el número de balas activas
        int activeBullets = BulletPool.Instance.GetActiveBulletCount();
        bulletCounterText.text = $"Balas: {activeBullets}";
    }
}
