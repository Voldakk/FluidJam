using UnityEngine;

public class PlayerTools : MonoBehaviour
{
    enum Tool
    {
        Hands, Welder
    }

    Tool currentTool;

    PlayerToolsUI ui;

    void Start()
    {
        ui = FindObjectOfType<PlayerToolsUI>();

        // Set the default active
        ui.SetActive(0);
        currentTool = Tool.Hands;
        hands.SetActive(true);
        welder.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        int index = (int)currentTool;

        // Scroll
        //int scroll = Mathf.RoundToInt(Input.GetAxis("MouseScroll"));
        //index += scroll;

        for (int i = 0; i < 2; i++)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                index = i;
            }
        }

        if(index != (int)currentTool)
        {
            ui.SetActive(index);

            // Disable the old tool
            switch (currentTool)
            {
                case Tool.Hands:
                    hands.SetActive(false);
                    break;
                case Tool.Welder:
                    welder.SetActive(false);
                    break;
                default:
                    break;
            }

            currentTool = (Tool)index;

            // Enable the new
            switch (currentTool)
            {
                case Tool.Hands:
                    hands.SetActive(true);
                    break;
                case Tool.Welder:
                    welder.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }

    // Hands
    [Header("Hands")]
    [SerializeField] PlayerPickup hands;

    // Welder
    [Header("Welder")]
    [SerializeField] PlayerWelder welder;
}
