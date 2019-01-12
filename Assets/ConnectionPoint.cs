using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ConnectionPoint : MonoBehaviour
{
    SpriteRenderer sr;
    new CircleCollider2D collider;

    public PhysicsObject physicsObject;

    public ConnectionPoint connected;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        collider = GetComponent<CircleCollider2D>();

        SetVissible(false);
    }

    private void Start()
    {
        PlayerWelder.allPoints.Add(this);
    }

    public void SetVissible(bool vissible)
    {
        if (connected)
        {
            sr.enabled = false;
            collider.enabled = false;
        }
        else
        {
            sr.enabled = vissible;
            collider.enabled = vissible;
        }
    }
}
