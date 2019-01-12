using UnityEngine;

public class Door : Triggerable
{
    public enum DoorState
    {
        Closed, Open
    }

    [SerializeField]
    DoorState state;

    [SerializeField]
    Transform door;

    [SerializeField]
    Vector2 openPosition;

    [SerializeField]
    Vector2 closedPosition;

    private void Start()
    {
        SetState(state);
    }

    public void SetState(DoorState newState)
    {
        state = newState;

        if (newState == DoorState.Closed)
            door.transform.position = transform.TransformPoint(closedPosition);
        else
            door.transform.position = transform.TransformPoint(openPosition);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.TransformPoint(closedPosition), transform.TransformPoint(openPosition));
    }

    private void OnValidate()
    {
        SetState(state);
    }

    public override void OnTrigger()
    {
        SetState(DoorState.Open);
    }
}
