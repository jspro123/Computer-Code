using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using PixelCrushers.DialogueSystem;

[Serializable]
public class ImageInfo
{
	public string name;
	public Sprite sprite;
}

public class AppManager : MonoBehaviour
{
    public GameObject twitterAlert;
    public GameObject chatAlert;
    public GameObject chatboxPrefab;
    
	private List<string> knownImages;
	
	private bool bishopAttack = false;
	private bool attackingNext = false;
	private string botTarget;
	private Dictionary<string, GameObject> openMap;
    private BotsManager botsManager;
    private DeepfakeManager deepFake;
    private GameObject chatboxCopy;
    private ScrollerController twitterData;
    
    // Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
    protected void Awake()
    {
    	openMap = new Dictionary<string, GameObject>();
    	knownImages = new List<string>();
    	botsManager = FindObjectOfType<BotsManager>();
    	deepFake = FindObjectOfType<DeepfakeManager>();
    	twitterData = FindObjectOfType<ScrollerController>();
    }

    public void ProcessBotAttack()
    {
    	botTarget = botsManager.curTarget;
    	attackingNext = true;
    	twitterAlert.SetActive(true);
    }
    
    public void ProcessBotAttack(string hiddenTarget)
    {
    	botTarget = hiddenTarget;
    	attackingNext = true;
    	twitterAlert.SetActive(true);
    	bishopAttack = true;
    }
    
    public void CheckForAttack()
    {
    	if(attackingNext) 
    	{
    		if(bishopAttack) { DialogueManager.PlaySequence("SetVariable(Checked Bishop Attack, true)"); }
    		attackingNext = false;
    		StartCoroutine(twitterData.StartAttack(botTarget));
    	}
    }
    
    public void SetDeepfakeList()
    {    	
    	for(int i = 0; i < knownImages.Count; i++) { deepFake.AddImage(knownImages[i]); }
    }
    
    public void AddToKnown(string name)
    {
    	if(!knownImages.Exists(x => x == name)) { knownImages.Add(name); }
    }
    
    public void OpenChat()
    {
    	chatboxCopy = Instantiate(chatboxPrefab);
    }
    
    public void CloseChat()
    {
    	Destroy(chatboxCopy);
    }
    
    
    
    
    
    
    
    
    
    
    
}
