using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class ExitGameButton : MonoBehaviour
{
    void Awake()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
