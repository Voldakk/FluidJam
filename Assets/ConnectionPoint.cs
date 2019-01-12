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

        Show();
    }

    private void Start()
    {
        PlayerWelder.allPoints.Add(this);
    }

    public void Hide()
    {
        sr.enabled = false;
        collider.enabled = false;
    }
    public void Show()
    {
        sr.enabled = true;
        collider.enabled = true;
    }
}
