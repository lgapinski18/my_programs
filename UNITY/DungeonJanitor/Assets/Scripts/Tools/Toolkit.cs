using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Toolkit : MonoBehaviour
{

    #region SCRIPT_PROPERTIES

    [SerializeField]
    List<AbstractTool> tools = new List<AbstractTool>();

    #endregion


    #region FIELDS

    AbstractTool currentTool;
    int currentIndex = 0;
    bool toggledOnTool = false;

    #endregion

    void Awake()
    {
        if (tools.Count > 0)
        {
            currentTool = tools[currentIndex];
        }

        foreach (AbstractTool tool in tools)
        {
            tool.gameObject.SetActive(true);
            //tool.gameObject.SetActive(false);
        }
    }


    void Update()
    {
        
    }

    public void SelectTool(int toolIndex)
    {
        currentTool.gameObject.SetActive(false);
        currentTool = tools[currentIndex];
        currentTool.gameObject.SetActive(true);
    }
    public void NextTool(InputAction.CallbackContext context)
    {
        if (context.started || context.canceled)
        {
            return;
        }
        if (!currentTool.IsUsed)
        {
            currentTool.gameObject.SetActive(false);
            currentIndex = (currentIndex + 1) % tools.Count;
            currentTool = tools[currentIndex];
            currentTool.gameObject.SetActive(toggledOnTool);
        }
    }
    public void PrevTool(InputAction.CallbackContext context)
    {
        if (context.started || context.canceled)
        {
            return;
        }
        if (!currentTool.IsUsed)
        {
            currentTool.gameObject.SetActive(false);
            currentIndex = (currentIndex + tools.Count - 1) % tools.Count;
            currentTool = tools[currentIndex];
            currentTool.gameObject.SetActive(toggledOnTool);
        }
    }
    public void ToggleSelectedTool(InputAction.CallbackContext context)
    {
        if (context.started || context.canceled)
        {
            return;
        }

        if (currentTool.gameObject.activeSelf)
        {
            currentTool.EndUsing();
        }

        if (!currentTool.IsUsed)
        {
            currentTool.gameObject.SetActive(!currentTool.gameObject.activeSelf);
            toggledOnTool = !toggledOnTool;

        }

    }

    public void UseTool(InputAction.CallbackContext context)
    {
        //Debug.Log("Using Tool");
        if (context.performed || context.canceled)
        {
            return;
        }
        //Debug.Log("Used Tool");
        if (toggledOnTool)
        {
            currentTool.Use();
        }
    }
    public void EndUsingTool(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            return;
        }
        currentTool.EndUsing();
    }

}
