using UnityEngine;
using UnityEngine.Events;

public class Button : Trigger
{
    public enum ButtonState
    {
        Standard, Activated
    }

    [SerializeField]
    ButtonState state;

    [SerializeField]
    Transform button;

    [SerializeField]
    Vector2 standardPosition;

    [SerializeField]
    Vector2 activatePosition;

    [SerializeField]
    LayerMask triggerMask;

    public void SetState(ButtonState newState)
    {
        state = newState;

        if (newState == ButtonState.Standard)
            button.transform.position = transform.TransformPoint(standardPosition);
        else
            button.transform.position = transform.TransformPoint(activatePosition);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.TransformPoint(standardPosition), transform.TransformPoint(activatePosition));
    }

    private void OnValidate()
    {
        SetState(state);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(triggerMask.Contains(collision.gameObject.layer) && state != ButtonState.Activated)
        {
            SetState(ButtonState.Activated);
            Activate();
        }
    }
}
