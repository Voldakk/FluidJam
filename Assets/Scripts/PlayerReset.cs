using UnityEngine;

public class PlayerReset : MonoBehaviour
{
    void Update()
    {
        if(Input.GetButtonDown("Reset"))
        {
            Checkpoint.lastCheckpoint.Reset(transform);
        }
    }
}
