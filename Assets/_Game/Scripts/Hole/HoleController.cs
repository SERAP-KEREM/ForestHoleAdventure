using UnityEngine;
using _Main.InputSystem;

namespace _Main.Hole
{
public class HoleController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // Hole'nin hareket hızı
   [SerializeField] private FloatingJoystick floatingJoystick; // Joystick referansı
    [SerializeField]private KeyboardInput keyboardInput; // Klavye input referansı

    private void Start()
    {
        // Sahnedeki FloatingJoystick ve KeyboardInput bileşenlerini bul
        floatingJoystick = FindObjectOfType<FloatingJoystick>();
        keyboardInput = FindObjectOfType<KeyboardInput>();
    }

    private void Update()
    {
        Move(); // Her frame'de hareket fonksiyonunu çağır
    }

    private void Move()
    {
        // Joystick girdilerini al
        float horizontal = floatingJoystick.Horizontal;
        float vertical = floatingJoystick.Vertical;

        // Klavye girdilerini al
        horizontal += keyboardInput.Horizontal;
        vertical += keyboardInput.Vertical;

        // Yön vektörünü oluştur
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        // Eğer yön vektörü yeterince büyükse hareket et
        if (direction.magnitude >= 0.1f)
        {
            // Hole'yi hareket ettir
            transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}
}