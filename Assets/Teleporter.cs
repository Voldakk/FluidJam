using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField]
    Teleporter connected;

    [SerializeField]
    Vector3 offset = Vector3.up;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (connected == null)
            return;

        if(collision.transform.CompareTag("Player"))
        {
            collision.transform.position = connected.transform.position + offset;
        }
    }

    private void OnDrawGizmos()
    {
        if (connected != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, connected.transform.position);
        }
    }
}
