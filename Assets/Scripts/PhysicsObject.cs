using UnityEngine;

public class PhysicsObject : LevelObject
{
    Vector3 position;
    Quaternion rotation;

    Rigidbody2D rb;

    private void Awake()
    {
        position = transform.position;
        rotation = transform.rotation;

        rb = GetComponent<Rigidbody2D>();
    }

    public override void Reset()
    {
        transform.position = position;
        transform.rotation = rotation;

        if (rb)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
}
