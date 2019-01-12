using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    LayerMask playerLayer;

    [SerializeField]
    SpriteRenderer flag;

    [SerializeField]
    Color lockedColor;

    [SerializeField]
    Color activeColor;

    [SerializeField]
    Color activatedColor;

    [SerializeField]
    Checkpoint next;

    [SerializeField]
    LevelObject[] objects;

    enum State
    {
        Locked, Active, Activated
    }

    State state;

    public static Checkpoint lastCheckpoint { get; private set; }

    private void Awake()
    {
        SetState(State.Active);
    }

    private void Start()
    {
        if (next)
            next.SetState(State.Locked);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(playerLayer.Contains(collision.gameObject.layer) && state == State.Active)
        {
            SetState(State.Activated);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if(next != null)
            Gizmos.DrawLine(flag.transform.position, next.flag.transform.position);
    }

    private void SetState(State newState)
    {
        state = newState;

        switch (state)
        {
            case State.Locked:
                flag.color = lockedColor;
                break;

            case State.Active:
                flag.color = activeColor;
                break;

            case State.Activated:
                lastCheckpoint = this;
                flag.color = activatedColor;
                if (next)
                    next.SetState(State.Active);
                else
                    LevelComplete();
                break;

            default:
                break;
        }
    }

    private void LevelComplete()
    {

    }

    public void Reset(Transform player)
    {
        player.position = transform.position + Vector3.up;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        foreach (var o in objects)
        {
            o.Reset();
        }
    }
}
