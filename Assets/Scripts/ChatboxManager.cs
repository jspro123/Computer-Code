using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class ChatboxManager : MonoBehaviour
{
	public CreatePopup popup;
	
    public void StartConvo()
    {
    	popup.disablePopup = true;
    	Sequencer.Message("Hold");
    }
    
    public void EndConvo()
    {
    	popup.disablePopup = false;
    }
}