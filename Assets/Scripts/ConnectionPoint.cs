using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ConnectionPoint : MonoBehaviour
{
    SpriteRenderer sr;
    new CircleCollider2D collider;

    public PhysicsObject physicsObject;

    public ConnectionPoint connected;

    public static bool vissible = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        collider = GetComponent<CircleCollider2D>();

        UpdateVissibility();
    }

    private void Start()
    {
        PlayerWelder.allPoints.Add(this);
    }


    public void UpdateVissibility()
    {
        UpdateVissibility(vissible);
    }

    public void UpdateVissibility(bool vissible)
    {
        if (connected)
        {
            sr.enabled = false;
            collider.enabled = false;
        }
        else
        {
            if (!sr)
                Debug.LogError("Missing sr", gameObject);
            sr.enabled = vissible;
            collider.enabled = vissible;
        }
    }
}
