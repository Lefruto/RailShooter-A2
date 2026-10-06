using UnityEngine;
using UnityEngine.InputSystem;

public class CrosshairController : MonoBehaviour
{
    private RectTransform crosshairRect;

    void Awake()
    {
        crosshairRect = GetComponent<RectTransform>();
        // İmleci gizlemek istersen alttaki satırı açabilirsin:
        // Cursor.visible = false;
    }

    void Update()
    {
        if (Mouse.current != null)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            crosshairRect.position = mousePosition;
        }
    }
}