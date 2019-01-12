using UnityEngine;

//[RequireComponent(typeof(DistanceJoint2D))]
public class PlayerPickup : MonoBehaviour
{
    [SerializeField]
    LayerMask objectLayer;

    [SerializeField]
    float maxDistance;

    FixedJoint2D joint;

    new Camera camera;

    [SerializeField]
    string objectLayerName;
    [SerializeField]
    string playerObjectLayerName;

    void Awake()
    {
        camera = Camera.main;
        joint = GetComponent<FixedJoint2D>();
        joint.enabled = false;
    }

    bool mouseClick;
    Vector2 mousePos;

    void Update()
    {
        mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        mouseClick = Input.GetButtonDown("Pickup") || mouseClick;
    }

    private void LateUpdate()
    {
        Vector2 anchor = mousePos.Sub(transform.position);

        if (anchor.magnitude > maxDistance)
            anchor = anchor.normalized * maxDistance;

        if (transform.lossyScale.x < 0)
            anchor.x *= -1;
        joint.anchor = anchor;
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, float.MaxValue, objectLayer);

        if(hit.collider != null && joint.connectedBody == null)
            CursorManager.SetCursor(Cursors.Pickup);
        else
            CursorManager.SetCursor(Cursors.Pointer);

        if (mouseClick)
        {
            mouseClick = false;

            if (joint.connectedBody == null)
            {
                if (hit.collider != null)
                {
                    if (Vector2.Distance(hit.point, transform.position) <= maxDistance)
                    {
                        joint.enabled = true;
                        joint.connectedBody = hit.rigidbody;
                        hit.transform.gameObject.layer = LayerMask.NameToLayer(playerObjectLayerName);
                        //joint.connectedAnchor = hit.point.Sub(transform.position);
                    }
                }
            }
            else
            {
                joint.enabled = false;

                joint.connectedBody.gameObject.layer = LayerMask.NameToLayer(objectLayerName);
                joint.connectedBody = null;
            }
        }

        Vector2 anchor = mousePos.Sub(transform.position);

        if (anchor.magnitude > maxDistance)
            anchor = anchor.normalized * maxDistance;

        if (transform.lossyScale.x < 0)
            anchor.x *= -1;
        joint.anchor = anchor;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, maxDistance);

        if(joint != null)
        {
            if(joint.connectedBody != null)
            {
                Gizmos.color = Color.red;

                Vector2 anchor = joint.anchor;
                if (transform.lossyScale.x < 0)
                    anchor.x *= -1;
                Gizmos.DrawSphere(anchor.Add(transform.position), 0.05f);

                Gizmos.DrawLine(joint.connectedBody.transform.position, transform.position);
            }
        }
    }
}
