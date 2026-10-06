using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapons : MonoBehaviour
{
    [Header("Weapon Particle Systems")]
    [SerializeField] private ParticleSystem[] lasers;

    [Header("Aim Settings")]
    [Tooltip("Crosshair'in 3D dünyadaki hedef derinliği (50 ile 80 arası idealdir)")]
    [SerializeField] private float aimDistance = 60f;

    [Tooltip("Crosshair UI objesi")]
    [SerializeField] private RectTransform crosshairRect;

    private Camera mainCamera;
    private bool isFiring = false;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        AimLasers();
        HandleInput();
    }

    private void AimLasers()
    {
        if (mainCamera == null || crosshairRect == null) return;

        // 1. Kameradan crosshair'in ekrandaki tam koordinatına bir ışın fırlat
        Ray ray = mainCamera.ScreenPointToRay(crosshairRect.position);

        // 2. Bu ışın üzerinde kameradan 'aimDistance' kadar ilerideki nokta 
        // ekranda TAM OLARAK crosshair'in merkezidir!
        Vector3 targetWorldPosition = ray.GetPoint(aimDistance);

        // 3. Kanatlardaki lazerleri doğrudan bu hedef noktaya çevir
        if (lasers != null)
        {
            foreach (var laser in lasers)
            {
                if (laser == null) continue;
                laser.transform.LookAt(targetWorldPosition);
            }
        }
    }

    private void HandleInput()
    {
        bool mouseFiring = Mouse.current != null && Mouse.current.leftButton.isPressed;
        bool spaceFiring = Keyboard.current != null && Keyboard.current.spaceKey.isPressed;

        if (mouseFiring || spaceFiring)
        {
            if (!isFiring)
            {
                isFiring = true;
                SetLaserEmission(true);
            }
        }
        else
        {
            if (isFiring)
            {
                isFiring = false;
                SetLaserEmission(false);
            }
        }
    }

    private void SetLaserEmission(bool active)
    {
        if (lasers == null) return;

        foreach (var laser in lasers)
        {
            if (laser == null) continue;

            var emission = laser.emission;
            emission.enabled = active;

            if (active && !laser.isPlaying)
            {
                laser.Play();
            }
            else if (!active && laser.isPlaying)
            {
                laser.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }
    }

    private void OnDisable()
    {
        SetLaserEmission(false);
    }
}