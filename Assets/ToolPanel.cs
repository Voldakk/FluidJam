using UnityEngine;
using UnityEngine.UI;

public class ToolPanel : MonoBehaviour
{
    public Image image;
    public GameObject selected;

    public void SetActive(bool active)
    {
        selected.SetActive(active);
    }
}
