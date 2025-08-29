using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.RightArrow)) movement.x = 1f;
        if (Input.GetKey(KeyCode.LeftArrow))  movement.x = -1f;
        if (Input.GetKey(KeyCode.UpArrow))    movement.y = 1f;
        if (Input.GetKey(KeyCode.DownArrow))  movement.y = -1f;

        transform.Translate(movement * speed * Time.deltaTime);
    }
}
