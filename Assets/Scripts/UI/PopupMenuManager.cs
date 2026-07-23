using System.Collections.Generic;
using UnityEngine;

public class PopupMenuManager : MonoBehaviour
{
    public static List<PopupMenu> popupMenus = new List<PopupMenu>();

    public static void MenuOpened(PopupMenu popupMenu)
    {
        foreach (PopupMenu menu in popupMenus)
        {
            if (menu != popupMenu)
            {
                if (menu.isActive)
                {
                    menu.ToggleMenu();
                }
            }
        }
    }
}
