using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine;

public class SendTweetEvent : MonoBehaviour
{
    public TMPro.TMP_Dropdown dropdown;
    public TMPro.TMP_InputField textbox;
    
    private ScrollerController scrollerController;
    private TwitterManager twitterManager;
    private TweetData currentTweet;

	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	protected void Start()
	{
		FillDropdown();
		FillTweet();
	}

	public void FillDropdown()
	{
		dropdown.options.Clear();
		twitterManager = FindObjectOfType<TwitterManager>();
		
		for(int i = 0; i < twitterManager.thingsToTweet.Count; i++)
		{
			dropdown.options.Add(new TMPro.TMP_Dropdown.OptionData() {text = twitterManager.thingsToTweet[i]});
		}
	}

    public void FillTweet()
    {
    	string tweet = dropdown.options[dropdown.value].text.Trim();
    	twitterManager = FindObjectOfType<TwitterManager>();
    	currentTweet = twitterManager.CreateTweet(tweet);
    	if(currentTweet.tweet.isText) { textbox.text = currentTweet.tweet.tweetText; }
    }
    
    public void SubmitTweet()
    {
    	scrollerController = FindObjectOfType<ScrollerController>();
    	scrollerController.SubmitTweet(currentTweet);
    }
}
