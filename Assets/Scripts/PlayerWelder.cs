using System.Collections.Generic;
using UnityEngine;

public class PlayerWelder : MonoBehaviour
{
    [SerializeField]
    LayerMask objectLayer;

    [SerializeField]
    LayerMask connectionLayer;

    [SerializeField]
    float maxDistance;

    ConnectionPoint currentPoint;

    new Camera camera;

    public static List<ConnectionPoint> allPoints = new List<ConnectionPoint>();

    void Awake()
    {
        camera = Camera.main;
        allPoints.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }

    private void Update()
    {
        Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, float.MaxValue, connectionLayer);

        Cursors cursor = Cursors.Pointer;
        if (currentPoint)
            cursor = Cursors.Welder;

        if (hit.collider)
        {
            ConnectionPoint point = hit.collider.GetComponent<ConnectionPoint>();
            if (point)
            {
                if (Vector2.Distance(hit.point, transform.position) <= maxDistance)
                {
                    if (point.connected)
                    {
                        // Occupied
                        // Should never happen
                    }
                    else
                    {
                        // If we're conenction 2 points
                        if (currentPoint)
                        {
                            // If it's the same point / object
                            if (point == currentPoint || point.physicsObject == currentPoint.physicsObject)
                            {
                                cursor = Cursors.WelderOutOfRange;
                                if (Input.GetButtonDown("Weld"))
                                {
                                    // Cancel
                                    currentPoint.UpdateVissibility();
                                    currentPoint = null;
                                }
                            }
                            else
                            {
                                // If it's in range
                                if (Vector3.Distance(point.transform.position, currentPoint.transform.position) <= maxDistance)
                                {
                                    cursor = Cursors.Welder;
                                    if (Input.GetButtonDown("Weld"))
                                    {
                                        currentPoint.connected = point;
                                        point.connected = currentPoint;

                                        currentPoint.UpdateVissibility(false);
                                        point.UpdateVissibility(false);

                                        Transform a = currentPoint.physicsObject.transform;
                                        Transform b = point.physicsObject.transform;

                                        float rotZ = point.transform.rotation.eulerAngles.z + 180f - currentPoint.transform.rotation.eulerAngles.z;
                                        a.transform.rotation = Quaternion.Euler(0, 0, rotZ);

                                        a.transform.position = point.transform.position - (currentPoint.transform.position - a.transform.position);

                                        var joint = a.gameObject.AddComponent<HingeJoint2D>();
                                        joint.connectedBody = b.GetComponent<Rigidbody2D>();
                                        joint.enableCollision = true;
                                        joint.autoConfigureConnectedAnchor = false;
                                        joint.anchor = a.InverseTransformPoint(currentPoint.transform.position);
                                        joint.connectedAnchor = b.InverseTransformPoint(point.transform.position);

                                        /*joint.useLimits = true;
                                        JointAngleLimits2D limits = new JointAngleLimits2D
                                        {
                                            min = 0f,
                                            max = 0f
                                        };
                                        joint.limits = limits;*/

                                        currentPoint = null;
                                    }
                                }
                                else
                                {
                                    cursor = Cursors.WelderOutOfRange;
                                    if (Input.GetButtonDown("Weld"))
                                    {
                                        // Out of range
                                        currentPoint.UpdateVissibility();
                                        currentPoint = null;
                                    }
                                }
                            }
                        }
                        // Selecting the first point
                        else
                        {
                            cursor = Cursors.Welder;
                            if (Input.GetButtonDown("Weld"))
                            {
                                currentPoint = point;
                                currentPoint.UpdateVissibility(false);
                            }
                        }
                    }
                }
                else
                {
                    cursor = Cursors.WelderOutOfRange;
                }
            }
        }
        else if(currentPoint && Vector2.Distance(hit.point, transform.position) > maxDistance)
        {
            cursor = Cursors.WelderOutOfRange;
        }

        // Click to cancel
        if(Input.GetButtonDown("CancelWeld") && currentPoint)
        {
            currentPoint.UpdateVissibility();
            currentPoint = null;
        }

        CursorManager.SetCursor(cursor);
    }

    public void SetActive(bool active)
    {
        enabled = active;
        ConnectionPoint.vissible = active;

        foreach (var c in allPoints)
        {
            c.UpdateVissibility();
        }
    }
}
