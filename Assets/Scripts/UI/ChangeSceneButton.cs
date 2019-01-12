using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class ChangeSceneButton : MonoBehaviour
{
    public string sceneName;

    void Awake()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(sceneName);
        });
    }
}
