using UnityEngine;

public enum Cursors
{
    None, Pointer, Pickup
}

public class CursorManager : MonoBehaviour
{
    public Texture2D[] cursorTextures;
    public Vector2[] cursorHotspots;
    private Cursors currentCursor;

    #region Singleton

    static CursorManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("There's more than one CursorManager in the scene!", gameObject);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    #endregion

    private void Start()
    {
        SetCursor(Cursors.Pointer);
    }

    public static void SetCursor(Cursors cursor)
    {
        if (cursor != instance.currentCursor)
        {
            instance.currentCursor = cursor;
            Cursor.SetCursor(instance.cursorTextures[(int)cursor], instance.cursorHotspots[(int)cursor], CursorMode.Auto);
        }
    }
}
