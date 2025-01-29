using UnityEngine;

namespace _Main.InputSystem
{
public class KeyboardInput : MonoBehaviour
{
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }

    private void Update()
    {
        // Klavye girdilerini kontrol et
        Horizontal = 0;
        Vertical = 0;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) Vertical = 1; // Yukarı
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) Vertical = -1; // Aşağı
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) Horizontal = -1; // Sola
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) Horizontal = 1; // Sağa
    }
}
}