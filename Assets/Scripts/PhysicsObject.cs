﻿using UnityEngine;

public class PhysicsObject : LevelObject
{
    Vector3 position;
    Quaternion rotation;

    Rigidbody2D rb;

    public ConnectionPoint[] connectionPoints;

    private void Awake()
    {
        position = transform.position;
        rotation = transform.rotation;

        rb = GetComponent<Rigidbody2D>();

        connectionPoints = new ConnectionPoint[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            connectionPoints[i] = transform.GetChild(i).GetComponent<ConnectionPoint>();
            connectionPoints[i].physicsObject = this;
        }
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

        foreach (var c in connectionPoints)
        {
            c.connected = null;
        }

        var joints = transform.GetComponents<HingeJoint2D>();
        foreach (var j in joints)
        {
            Destroy(j);
        }
    }
}
