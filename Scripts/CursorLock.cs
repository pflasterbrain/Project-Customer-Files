using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLock : MonoBehaviour
{
        // Cursor Lock variables
    public bool isCursorLocked = true;
    public bool isNewScene;
        private void Awake()
    {
        // Unlock cursor if it's a new scene
        if (isNewScene)
        {
            Invoke("UnlockCursor", 1);
        }
    }

    void Start()
    {
        if (!isNewScene)
        {
            LockCursor();
        }
    }
    void Update()
    {
        // Handle Cursor Locking
        HandleCursorLock();
    }

    // Handle locking/unlocking of the cursor
    void HandleCursorLock()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isCursorLocked)
            {
                UnlockCursor();
            }
            else
            {
                LockCursor();
            }
        }
    }

public void LockCursor()
{
    //Debug.Log("Locking cursor");
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
    isCursorLocked = true;
}

public void UnlockCursor()
{
    //Debug.Log("Unlocking cursor");
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
    isCursorLocked = false;
}
}
