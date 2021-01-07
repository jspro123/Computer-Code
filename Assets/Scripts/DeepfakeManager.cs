using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.Progress;
using TMPro;
using UnityEngine.UI;
using Doozy.Engine;
using PixelCrushers.DialogueSystem;

public class DeepfakeManager : MonoBehaviour
{
	public CreatePopup popup;
	[Range(2, 10)]
	public float minGenerateDuration = 5;
	[Range(2, 20)]
	public float maxGenerateDuration = 10;
	public RectTransform maskingPanel;
	public Progressor progress;
	public GameObject generateButton;
	public GameObject saveButton;
	public Image generatedImage;
	public TMPro.TMP_Dropdown imageDropdown;
	public List<ImageInfo> deepfakes;
	
	private TwitterManager twitterManager;
	private string imageName;
	private bool addedImage = false;
	private const int minLoads = 20;
	private const int maxLoads = 30;
	
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	protected void Start()
	{
		twitterManager = FindObjectOfType<TwitterManager>();
		progress.gameObject.SetActive(false);
		saveButton.SetActive(false);
	}
	
	private void OnValidate()
	{
		if(minGenerateDuration > maxGenerateDuration) { minGenerateDuration = maxGenerateDuration; }
	}

	public void AddImage(string imageName)
	{
		if(!addedImage)
		{
			addedImage = true;
			imageDropdown.options.Clear();
		}
		
		imageDropdown.options.Add(new TMP_Dropdown.OptionData() {text = imageName});
	    imageDropdown.value = 1; //For refreshing apparently
	    imageDropdown.value = 0;
	}

    private void GenerateRandomSplit(float target, float[] output)
    {
        float total = 0;
        for(int i = 0; i < output.Length; i++)
        {
            output[i] = UnityEngine.Random.Range(0.0f, 1.0f);
            total += output[i];
        }
				
        //Scaling
        for (int i = 0; i < output.Length; i++)
        {
            output[i] = (output[i] / total) * target;
            if(i != 0) { output[i] += output[i - 1]; }
        }
    }
    
    private bool SetDeepfake()
    {
    	imageName = imageDropdown.options[imageDropdown.value].text;
    	
    	if(!deepfakes.Exists(x => (x.name == imageName)))
    	{
    		Debug.LogError("Deepfake not in dictionary!");
    		return false;
    	}
    	
    	Sprite deepfake = deepfakes.Find(x => x.name == imageName).sprite;
    	generatedImage.sprite = deepfake;
    	return true;
    }
    
    private IEnumerator GenerateImage()
    {   
    	float loadTime = UnityEngine.Random.Range(minGenerateDuration, maxGenerateDuration);
    	float totalTime = 0;
    	int loadSteps = UnityEngine.Random.Range(minLoads, maxLoads);
    	float[] waitArr = new float[loadSteps];
    	float[] posArr = new float[loadSteps];
    	GenerateRandomSplit(loadTime, waitArr);
    	GenerateRandomSplit(1, posArr);
    	int currentIndexWait = 0;
    	int currentIndexPos = posArr.Length - 1;

        while (totalTime < loadTime)
        {
            totalTime += Time.deltaTime;
            if(totalTime > waitArr[currentIndexWait])
            {
            	maskingPanel.anchorMax = new Vector2(maskingPanel.anchorMax.x, posArr[currentIndexPos]);
            	progress.SetValue(1 - maskingPanel.anchorMax.y);
                currentIndexWait++;
                currentIndexPos--;
            }
            yield return null;
        }
        
        maskingPanel.anchorMax = new Vector2(maskingPanel.anchorMax.x, 0);
        progress.SetValue(1);
    }
    
    public void SaveImage()
    {
    	if(imageName == null) { return; } 
    	if(imageName.Trim() == "") { return; }
    	
    	string toTweet = "Tweet modified " + imageName + ".";
    	if(!twitterManager.thingsToTweet.Exists(x => x == toTweet))
    	{
    		twitterManager.thingsToTweet.Add(toTweet);
    	}
    	
    	saveButton.SetActive(false);
    	generateButton.SetActive(true);
    	progress.gameObject.SetActive(false);
    	GameEventMessage.SendEvent("SAVEFAKE");
    }
    
    public void StartGeneratingImage()
    {
    	if(SetDeepfake())
    	{
    		maskingPanel.anchorMax = new Vector2(maskingPanel.anchorMax.x, 1);
			GameEventMessage.SendEvent("GENERATE");
			DialogueManager.PlaySequence("SetVariable(Deepfaked, true)");
    		popup.disablePopup = true;
	    	saveButton.SetActive(false);
	    	generateButton.SetActive(false);
	    	progress.gameObject.SetActive(true);
	    	StartCoroutine(GenerateImage());
    	}
    	
    }
    
    // This function is called when the behaviour becomes disabled () or inactive.
    protected void OnDisable()
    {
    	SaveImage();
    }

    public void CheckProgress()
    {
    	if(progress.Value == 1)
    	{
    		saveButton.SetActive(true);
    		popup.disablePopup = false;
    	}
    }
  
}
