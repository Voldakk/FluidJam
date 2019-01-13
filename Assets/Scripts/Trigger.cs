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
        Gizmos.color = Color.green;
        if (triggers != null)
        {
            foreach (var t in triggers)
            {
                if(t != null)
                    Gizmos.DrawLine(transform.position, t.transform.position);
            }
        }
    }
}
