using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    LineRenderer lr;

    [SerializeField]
    int maxJumps = 0;

    [SerializeField]
    float maxDist = 100f;

    [SerializeField]
    LayerMask mask;

    List<Vector3> points;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        points = new List<Vector3>();
    }

    void Update()
    {
        Vector2 start = transform.position;
        Vector2 direction = transform.rotation * Vector2.up;

        points.Clear();
        points.Add(start);

        RaycastHit2D hit;

        for (int i = 0; i <= maxJumps; i++)
        {
            hit = Physics2D.Raycast(start, direction, maxDist, mask);
            if(hit.collider)
            {
                start = hit.point - direction * 0.05f;
                direction = direction - 2f * Vector2.Dot(direction, hit.normal) * hit.normal;

                points.Add(start);

                LaserReciever lr = hit.transform.GetComponent<LaserReciever>();
                if (lr)
                {
                    lr.Trigger();
                    break;
                }
            }
            else
            {
                points.Add(start + direction * maxDist);
                break;
            }
        }

        lr.positionCount = points.Count;
        lr.SetPositions(points.ToArray());
    }
}
