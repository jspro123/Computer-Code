using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine;
using PixelCrushers.DialogueSystem;

public class TerminalManager : MonoBehaviour
{
	public CreatePopup popup;
	public TMPro.TMP_InputField inputField;
	public TMPro.TextMeshProUGUI outputField;
	private const string IPADRRESS = "123.4.5.6";
	private const int HACKING_TIME = 10;
	private bool hacking = false;
	private List<string> history = new List<string>();
	private int history_index = 0;
	
    // Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
    protected void Start()
    {
    	inputField.text = "";
    }
        
    public void ProcessCommand()
    {
    	if(hacking) { return; }
    	string command = inputField.text.Trim();
    	history.Add(command);
    	history_index = history.Count;
    	inputField.text = "";
    	string[] split = command.Split(' ');

    	if(split.Length == 1)
    	{
    		outputField.text = "> Unknown command: " + command;
    	} else if(!command.StartsWith("./hack"))
    	{
    		outputField.text = "> Unknown command: " + split[0];
    	} else if(split[1] != IPADRRESS)
    	{
    		outputField.text = "> Unknown IP: " + split[1];
    	} else 
    	{
    		outputField.text = "> Success! You have ten seconds. ";
    		StartCoroutine(StartHacking());
    		hacking = true;
    		popup.disablePopup = true;
    	}
    }
    
	public void previousCommand () {
		history_index--;
		inputField.text = history[history_index];
	}
	
	public void nextCommand () {
		history_index++;
		inputField.text = history[history_index];
	}
    
	void Update() {
		if (Input.GetKeyDown(KeyCode.UpArrow) && history_index > 0 && history_index <= history.Count) {
			previousCommand();
		}
		
		if (Input.GetKeyDown(KeyCode.DownArrow) && history_index >= 0 && history_index < history.Count - 1) {
			nextCommand ();
		}
	}
	
    public IEnumerator StartHacking()
    {
    	yield return new WaitForSeconds(1.5f);
    	GameEventMessage.SendEvent("BEGINHACKING");
    	
    	float totalElapsed = 0;
    	float elapsed = 0;
    	
    	while(totalElapsed < HACKING_TIME)
    	{
    		elapsed += Time.deltaTime;
    		if(elapsed >= 1)
    		{
    			totalElapsed += elapsed;
    			elapsed = 0;
    			outputField.text = "> " + (HACKING_TIME - Mathf.RoundToInt(totalElapsed)) + " seconds left. ";
    		}
    		yield return null;
    	}
    	
    	GameEventMessage.SendEvent("QUITHACKING");
    	outputField.text = "";
		hacking = false;
		popup.disablePopup = false;
    }
}
