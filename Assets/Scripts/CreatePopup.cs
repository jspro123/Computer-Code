using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doozy.Engine.UI;

public class CreatePopup : MonoBehaviour
{
	[HideInInspector]
	public bool disablePopup = false;
    private UIPopup m_popup;

    public void ShowPopup(string popupName)
    {
        //get a clone of the UIPopup, with the given PopupName, from the UIPopup Database 
        if(disablePopup) { return; }
        UIPopup popup = UIPopup.GetPopup(popupName);

        //make sure that a popup clone was actually created
        if (popup == null)
            return;

        popup.Show(); //show the popup
    }

    private void ClickButton()
    {
        Debug.Log("Clicked button");
        ClosePopup();
    }
    
    private void ClosePopup()
    {
        if (m_popup != null) m_popup.Hide();
    }
}
