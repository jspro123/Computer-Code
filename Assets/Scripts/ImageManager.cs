using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
	public List<ImageInfo> images;
	public Image displayedImage;

	private string currentImage;
	private AppManager appManager;
	
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	protected void Start()
	{
		appManager = FindObjectOfType<AppManager>();
	}
	
    public void SetImage(string name)
    {
    	currentImage = name;
    	
    	if(!images.Exists(x => (x.name == name)))
    	{
    		Debug.LogError("Image not in dictionary!");
    		return;
    	}
    	
    	Sprite sprite = images.Find(x => x.name == name).sprite;
    	displayedImage.sprite = sprite;
    }
    
    public void AddToKnown()
    {
    	appManager.AddToKnown(currentImage);
    }
}
