using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagement : MonoBehaviour
{
    public static bool IsPause;

    [SerializeField] private GameObject go_BaseUI;

    void Start()
    {
        IsPause = false;
    }

    private void CallMenu()
    {
        IsPause = true;
        go_BaseUI.SetActive(true);
        Time.timeScale = 0;
    }

    private void CloseMenu()
    {
        IsPause = false;
        go_BaseUI.SetActive(false);
        Time.timeScale = 1;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (IsPause)
            {
                CallMenu();
            }
            else
            {
                CloseMenu();
            }
        }
    }

    

    
}
