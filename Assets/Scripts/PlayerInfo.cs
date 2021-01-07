using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine;
using PixelCrushers.DialogueSystem;

public class PlayerInfo : MonoBehaviour
{
	public string playerName;
	public TMPro.TMP_InputField input;
	private bool submitted = false;
	
	public void RememberName()
	{
		if(input.text != "" && !submitted)
		{
			submitted = true;
			playerName = input.text;
			Debug.Log(playerName);
			GameEventMessage.SendEvent("LOGIN");
			DialogueManager.PlaySequence("SetVariable(playerName," + playerName + ")");
		}
	}
}
