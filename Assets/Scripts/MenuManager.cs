using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField] Menu[] menus;

    private void Awake()
    {
        if(Instance!=null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            
        }
    }
    public void OpenMenu(string menuName)
    {
        foreach (Menu menu in menus)
        {
            if(menu.menuName==menuName)
            {
                OpenMenu(menu);
            }
            else if (menu.isOpen)
            {
                CloseMenu(menu);
            }
        }
    }

    private void CloseMenu(Menu menu)
    {
        menu.Close();
    }

    public void OpenMenu(Menu menu)
    {
        menu.Open();
    }
}
