using UnityEngine;

public class PlayerToolsUI : MonoBehaviour
{
    ToolPanel[] panels;

    private void Awake()
    {
        panels = new ToolPanel[transform.childCount];
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i] = transform.GetChild(i).GetComponent<ToolPanel>();
            panels[i].SetActive(false);
        }
    }

    public void SetActive(int index)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == index);
        }
    }
}
