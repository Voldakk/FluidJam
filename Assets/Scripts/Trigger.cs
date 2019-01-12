using UnityEngine;

public abstract class Trigger : LevelObject
{
    [SerializeField]
    protected Triggerable[] triggers;

    protected virtual void Activate()
    {
        foreach (var t in triggers)
        {
            t.OnTrigger();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (var t in triggers)
        {
            Gizmos.DrawLine(transform.position, t.transform.position);
        }
    }
}
