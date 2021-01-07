using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Doozy.Engine;
using PixelCrushers.DialogueSystem;

public class BotsManager : MonoBehaviour
{
	public CreatePopup popup;
    public TMPro.TMP_Dropdown dropdown;
    [SerializeField]
    public GameObject attackButton;
    public TMPro.TextMeshProUGUI attackText;
    [HideInInspector]
    public string curTarget; 
    
    private const float PERIODPAUSE = 1.0f;
    private const float FINALPAUSE = 2.0f;

    
    public void BeginAttack()
    {
    	string target = dropdown.options[dropdown.value].text; 
		popup.disablePopup = true;
    	curTarget = target;
    	attackButton.SetActive(false);
    	StartCoroutine(ShittyAnimation());
    }
    
    private IEnumerator ShittyAnimation()
    {
    	attackText.text = "Attack in progress";
    	yield return new WaitForSeconds(PERIODPAUSE);
    	attackText.text += ".";
    	yield return new WaitForSeconds(PERIODPAUSE);
    	attackText.text += ".";
    	yield return new WaitForSeconds(PERIODPAUSE);
    	attackText.text += ".";
    	yield return new WaitForSeconds(PERIODPAUSE);
    	attackText.text = "Attack complete!";
    	yield return new WaitForSeconds(FINALPAUSE);
    	MarkAttack();
    	yield break;
    }
    
    private void MarkAttack()
    {
    	popup.disablePopup = false;
    	DialogueManager.PlaySequence("SetVariable(Sent Bots, true)");
    	GameEventMessage.SendEvent("BOT");
    	GameEventMessage.SendEvent("CLOSE");
    }
}
