﻿using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Water : MonoBehaviour
{
    [SerializeField]
    private float linearDrag = 0.1f;

    [SerializeField]
    private float angularDrag = 0.1f;

    [SerializeField]
    private float density = 1;

    private new BoxCollider2D collider;
    private float waterLevel;

    private Vector2[] points = new Vector2[4];
    private List<int> bellow = new List<int>(4);

    private Vector2 a, b, c, d, e, f, cof;

    private Vector3 startPosition;
    private Vector3 startScale;
    private float displacedVolume = 0f;
    private float extraHeight;
    [SerializeField]
    private float heightT = 1f;

    [SerializeField]
    private float enterModifier = 1f;
    [SerializeField]
    private float downModifier = 2f;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        waterLevel = collider.bounds.max.y;

        startPosition = transform.position;
        startScale = transform.localScale;
    }

    private void FixedUpdate()
    {
        extraHeight = Mathf.Lerp(extraHeight, displacedVolume / startScale.x, Time.fixedDeltaTime * heightT);

        transform.localScale = startScale + Vector3.up * extraHeight;
        transform.position = startPosition + Vector3.up * extraHeight / 2f;

        waterLevel = collider.bounds.max.y;

        displacedVolume = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.attachedRigidbody;
        rb.velocity *= enterModifier;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Transform t = collision.transform;
        Rigidbody2D rb = collision.attachedRigidbody;

        float objectVolume = 0;
        Vector2 centerOfForce = t.position;

        // Calcualte displaced volume
        if (collision is BoxCollider2D)
        {
            BoxCollider2D boxCollider = collision as BoxCollider2D;

            Vector2 halfSize = boxCollider.size * 0.5f;
            points[0] = t.TransformPoint(halfSize * new Vector2(1f, 1f) + boxCollider.offset);
            points[1] = t.TransformPoint(halfSize * new Vector2(1f, -1f) + boxCollider.offset);
            points[2] = t.TransformPoint(halfSize * new Vector2(-1f, -1f) + boxCollider.offset);
            points[3] = t.TransformPoint(halfSize * new Vector2(-1f, 1f) + boxCollider.offset);

            bellow.Clear();
            for (int i = 0; i < 4; i++)
            {
                if (points[i].y < waterLevel)
                    bellow.Add(i);
            }

            Vector2 size = boxCollider.size * t.lossyScale;
            halfSize = size / 2f;

            float submergedArea = 0f;

            // One corner submerged 
            if (bellow.Count == 1)
            {
                // Find a, b, c, d
                a = GetPoint(bellow[0]);
                b = GetPoint(bellow[0] + 3);
                c = GetPoint(bellow[0] + 2);
                d = GetPoint(bellow[0] + 1);

                // Find e
                Vector2 ad = d - a;
                float aey = waterLevel - a.y;
                e = a + ad * (aey / ad.y);

                // Find f
                Vector2 ab = b - a;
                float afy = waterLevel - a.y;
                f = a + ab * (afy / ab.y);

                // The AFE triangle
                submergedArea += Math.TriangleArea(a, f, e);

                // The center of force
                centerOfForce = Math.TriangeCenterOfMass(a, f, e);
            }
            // One side submerged
            else if (bellow.Count == 2)
            {
                // Find a, b, c, d
                int first = 0;
                if (bellow[0] == 0 && bellow[1] == 3)
                    first = 1;

                b = GetPoint(bellow[first] + 0);
                a = GetPoint(bellow[first] + 1);
                d = GetPoint(bellow[first] + 2);
                c = GetPoint(bellow[first] + 3);

                // Find e
                Vector2 ad = d - a;
                float aey = waterLevel - a.y;
                e = a + ad * (aey / ad.y);

                // Find f
                Vector2 bc = c - b;
                float bcy = waterLevel - b.y;
                f = b + bc * (bcy / bc.y);

                // The AFE + ABF triangle
                submergedArea += Math.TriangleArea(a, f, e);
                submergedArea += Math.TriangleArea(a, b, f);

                // The center of force
                centerOfForce = Math.QuadrilateralCenterOfMass(a, e, f, b);
            }
            // Everything but one corner submerged
            else if (bellow.Count == 3)
            {
                // The index thats above the water
                int sum = bellow.Sum();
                int above = 6 - sum;

                // Find a, b, c, d
                d = GetPoint(above + 0);
                c = GetPoint(above + 1);
                b = GetPoint(above + 2);
                a = GetPoint(above + 3);

                // Find e
                Vector2 ad = d - a;
                float aey = waterLevel - a.y;
                e = a + ad * (aey / ad.y);

                // Find f
                Vector2 cd = d - c;
                float cdy = waterLevel - c.y;
                f = c + cd * (cdy / cd.y);

                // The DEF
                submergedArea += size.x * size.y - Math.TriangleArea(d, e, f);

                // The center of force
                centerOfForce = Math.PentagonCenterOfMass(a, b, c, f, e);
            }
            else if(bellow.Count == 0)
            {
                submergedArea = 0f;
            }
            else
            {
                // Find a, b, c, d
                a = GetPoint(0);
                b = GetPoint(1);
                c = GetPoint(2);
                d = GetPoint(3);

                submergedArea = size.x * size.y;
            }

            objectVolume = submergedArea;
        }
        else if (collision is CircleCollider2D)
        {
            CircleCollider2D circleCollider = collision as CircleCollider2D;
            float radius = circleCollider.radius * Mathf.Max(t.lossyScale.x, t.lossyScale.y);
            Vector2 center = circleCollider.bounds.center;

            Vector2 mid = center;
            mid.y = waterLevel;

            depth = waterLevel - (center.y - radius);

            a = mid;
            c = center;

            // More than halfway out of the water
            if (depth > 0 && depth <= radius)
            {
                angle = Mathf.Rad2Deg * Mathf.Asin((radius - depth) / radius);
                angle = (90 - angle);

                // Find b
                b = mid;
                b.x += Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

                // The volume of the sector
                objectVolume = ((angle * 2f) / 360f) * Mathf.PI * radius * radius;

                // We only want the  volume of the segment
                objectVolume -= Math.TriangleArea(a, b, c) * 2;
            }
            else if (depth > radius && depth < radius * 2)
            {
                angle = Mathf.Rad2Deg * Mathf.Asin((depth - radius) / radius);
                angle = (90 - angle);

                // Find b
                b = mid;
                b.x += Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

                // The volume of everything but the sector
                objectVolume = ((360 - angle * 2f) / 360f) * Mathf.PI * radius * radius;

                // Add the triangle in the sector, but not the segment
                objectVolume += Math.TriangleArea(a, b, c) * 2;
            }
            else if (depth >= radius * 2)
            {
                objectVolume = Mathf.PI * radius * radius;
            }
        }

        objectVolume = Mathf.Abs(objectVolume);

        // Apply force
        Vector2 force = density * objectVolume * -Physics2D.gravity;
        rb.AddForceAtPosition(force, centerOfForce, ForceMode2D.Force);
        cof = centerOfForce;

        // Drag
        rb.velocity -= rb.velocity * Mathf.Min(rb.velocity.magnitude, Time.fixedDeltaTime * linearDrag * (rb.velocity.y < 0 ? downModifier : 1f));
        rb.angularVelocity -= rb.angularVelocity * angularDrag * Time.fixedDeltaTime;

        // Add to the total displaced amount this fixed update
        displacedVolume += objectVolume;

        Debug.LogFormat("Volume: {0}, force: {1}", objectVolume, force);
    }

    float angle;
    float depth;

    // Wraps the index
    public Vector2 GetPoint(int index)
    {
        if (index >= points.Length)
            return points[index - points.Length];

        return points[index];
    }

    private void OnDrawGizmos()
    {
        float radius = 0.05f;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(a, radius);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(b, radius);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(c, radius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(d, radius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(e, radius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(f, radius);

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(cof, radius);
    }
}
