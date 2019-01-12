using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Transform main;
    [SerializeField]
    Transform levels;

    [SerializeField]
    GameObject buttonPrefab;

    private void Awake()
    {
        main.gameObject.SetActive(true);
        levels.gameObject.SetActive(false);

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings - 1; i++)
        {
            GameObject button = Instantiate(buttonPrefab, levels);
            var b = button.GetComponent<UnityEngine.UI.Button>();

            int level = i;

            button.GetComponentInChildren<TMP_Text>().text = "Level " + (level + 1);

            b.onClick.AddListener(() => { OnButtonClicked(level); });
        }
    }

    private void OnButtonClicked(int level)
    {
        SceneManager.LoadScene(level + 1);
    }
}
