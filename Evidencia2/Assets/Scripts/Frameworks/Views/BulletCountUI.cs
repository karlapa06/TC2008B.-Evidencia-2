using UnityEngine;
using TMPro;

/// <summary>
/// Controla el contador de balas activas en la UI.
/// Muestra en tiempo real cuántas balas están activas actualmente en pantalla.
/// </summary>
public class BulletCounterUI : MonoBehaviour
{
    public TMP_Text bulletCounterText;

    private void Update()
    {
        int activeBullets = BulletPool.Instance.GetActiveBulletCount();
        bulletCounterText.text = $"Balas: {activeBullets}";
    }
}
