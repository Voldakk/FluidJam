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

        if(hit.collider)
        {
            Debug.Log("Hit " + hit.collider.gameObject.name);

            if (Vector2.Distance(hit.point, transform.position) <= maxDistance)
            {
                ConnectionPoint point = hit.collider.GetComponent<ConnectionPoint>();
                if (point)
                {
                    if (Input.GetButtonDown("Weld"))
                    {
                        if (point.connected)
                        {
                            // Occupied
                        }
                        else
                        {
                            // If we're conenction 2 points
                            if (currentPoint)
                            {
                                // If it's the same point / object
                                if (point == currentPoint || point.physicsObject == currentPoint.physicsObject)
                                {
                                    // Cancel
                                    currentPoint.SetVissible(true);
                                    currentPoint = null;
                                }
                                else
                                {
                                    // If it's in range
                                    if (Vector3.Distance(point.transform.position, currentPoint.transform.position) <= maxDistance)
                                    {
                                        currentPoint.connected = point;
                                        point.connected = currentPoint;

                                        currentPoint.SetVissible(false);
                                        point.SetVissible(false);

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

                                        currentPoint = null;
                                    }
                                    else
                                    {
                                        // Out of range
                                        currentPoint.SetVissible(true);
                                        currentPoint = null;
                                    }
                                }
                            }
                            // Selecting the first point
                            else
                            {
                                currentPoint = point;
                                currentPoint.SetVissible(false);
                            }
                        }
                    }
                }
            }
            else
            {
                // Out of range
            }
        }

        // Click to cancel
        if(Input.GetButtonDown("CancelWeld") && currentPoint)
        {
            currentPoint.SetVissible(true);
            currentPoint = null;
        }
    }

    public void SetActive(bool active)
    {
        enabled = active;

        foreach (var c in allPoints)
        {
            c.SetVissible(active);
        }
    }
}
