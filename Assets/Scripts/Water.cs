using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Water : MonoBehaviour
{
    [SerializeField]
    private float linearDrag = 0.1f;

    [SerializeField]
    private float density = 1;

    private new BoxCollider2D collider;
    private float waterLevel;

    private Vector2[] points = new Vector2[4];
    private List<int> bellow = new List<int>(4);

    Vector2 a, b, c, d, e, f;

    float width, height;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        waterLevel = collider.bounds.max.y;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Transform t = collision.transform;
        Rigidbody2D rb = collision.attachedRigidbody;
        float objectDensity = collision.density;

        // Calcualte volume
        float objectVolume = rb.mass / objectDensity;

        if (collision is BoxCollider2D)
        {
            BoxCollider2D boxCollider = collision as BoxCollider2D;
            Vector2 size = boxCollider.size * t.lossyScale;
            Vector2 halfSize = size * 0.5f;

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
                submergedArea += TriangleArea(a, f, e);
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
                submergedArea += TriangleArea(a, f, e);
                submergedArea += TriangleArea(a, b, f);
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

                // The ABC + ACF + AFE triangle
                submergedArea += TriangleArea(a, b, c);
                submergedArea += TriangleArea(a, c, f);
                submergedArea += TriangleArea(a, f, e);
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

        // Apply force
        Vector2 force = density * objectVolume * -Physics2D.gravity;
        rb.AddForce(force, ForceMode2D.Force);

        // Drag
        rb.velocity -= rb.velocity * linearDrag;

        Debug.LogFormat("Density: {0}, volume: {1}, force: {2}", objectDensity, objectVolume, force);
    }

    public Vector2 GetPoint(int index)
    {
        if (index >= points.Length)
            return points[index - points.Length];

        return points[index];
    }

    private float TriangleArea(Vector2 pointA, Vector2 pointB, Vector2 pointC)
    {
        width = Vector2.Distance(pointA, pointB);
        height = Vector2.Distance(pointA, pointC) * Mathf.Sin(Mathf.Deg2Rad * Vector2.Angle(pointB - pointA, pointC - pointA));
        return 0.5f * width * height;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(a, 0.1f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(b, 0.1f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(c, 0.1f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(d, 0.1f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(e, 0.1f);

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(f, 0.1f);
    }
}
