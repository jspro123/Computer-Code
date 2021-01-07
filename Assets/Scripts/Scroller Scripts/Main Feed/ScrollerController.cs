using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using System;
using DG.Tweening;
using PixelCrushers.DialogueSystem;

public class ScrollerController : MonoBehaviour, IEnhancedScrollerDelegate
{
	[Header("Prefabs and Scroller")]
	public EnhancedScroller myScroller;
	public TweetCellViewText tweetTextCellViewPrefab;
	public TweetCellViewImage tweetImageCellViewPrefab;
	
	[Header("Misc. Settings")]
	[Tooltip("Size of each text tweet, in pixels. ")]
	public int textCellSize;
	[Tooltip("Size of each image tweet, in pixels. ")]
	public int imageCellSize;
	[Tooltip("How many tweets to generate. ")]
	public int numTweets;
	[Range(20, 100)]
	public int minBotTweets = 20;
	[Range(20, 100)]
	public int maxBotTweets = 20;
	[Range(5, 30)]
	public float minAttackDuration = 5;
	[Range(5, 30)]
	public float maxAttackDuration = 5;
	public float preAttackDelay = 0.75f;
	public EnhancedScroller.TweenType jumpTween;
	public float tweenModifer;
	
	private List<TweetData> _tweets;
	private TwitterManager twitterManager;
	
	// This function is called when the script is loaded or a value is changed in the inspector (Called in the editor only).
	protected void OnValidate()
	{
		if(minBotTweets > maxBotTweets) { minBotTweets = maxBotTweets; }
		if(minAttackDuration > maxAttackDuration) { minAttackDuration = maxAttackDuration; }
	}
	
	void Start ()
	{
		_tweets = new List<TweetData>();
		twitterManager = FindObjectOfType<TwitterManager>();
		
		for(int i = 0; i < numTweets; i++)
		{
			_tweets.Add(twitterManager.CreateGenericTweet());
		}

		twitterManager.gameObject.SetActive(false);
		myScroller.Delegate = this;
		myScroller.ReloadData();
		this.gameObject.SetActive(true);
	}
	
	public void SubmitTweet(TweetData data)
	{
		_tweets.Insert(0, data);
		int currentCellIndex = myScroller.GetCellViewIndexAtPosition(myScroller.ScrollPosition) + 1;
		float tweenTime = 0;
		myScroller.ReloadData();
		
		if(currentCellIndex != 0)
		{
			tweenTime = currentCellIndex * tweenModifer;
			myScroller.JumpToDataIndex(currentCellIndex,0,0, true, EnhancedScroller.TweenType.immediate, 0, null, EnhancedScroller.LoopJumpDirectionEnum.Up);
			myScroller.JumpToDataIndex(0,0,0, true, jumpTween, tweenTime, null, EnhancedScroller.LoopJumpDirectionEnum.Up);		
		} else 
		{
			myScroller.JumpToDataIndex(1,0,0, true, jumpTween, 0.1f, null, EnhancedScroller.LoopJumpDirectionEnum.Up);
			myScroller.JumpToDataIndex(0,0,0, true, jumpTween, 0.1f, null, EnhancedScroller.LoopJumpDirectionEnum.Up);
		}
		
		if(data.deepfake) { DialogueManager.PlaySequence("SetVariable(Submitted Deepfake, true)"); }
	}
	
	public void jumpToTop () {
		int currentCellIndex = myScroller.GetCellViewIndexAtPosition(myScroller.ScrollPosition) + 1;
		float tweenTime = currentCellIndex * tweenModifer;
		myScroller.JumpToDataIndex(0, 0, 0, true, jumpTween, tweenTime, null, EnhancedScroller.LoopJumpDirectionEnum.Up);
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
	
	public IEnumerator StartAttack(string target)
	{
		yield return new WaitForSeconds(preAttackDelay);
		int currentIndex = 0;
		int numTweets = UnityEngine.Random.Range(minBotTweets, maxBotTweets);
		float attackDuration = UnityEngine.Random.Range(minAttackDuration, maxAttackDuration);
		float totalTime = 0;
		float[] waitArr = new float[numTweets];
		GenerateRandomSplit(attackDuration, waitArr);
		
        while (totalTime < attackDuration)
        {
            totalTime += Time.deltaTime;
            if(totalTime > waitArr[currentIndex])
            {
            	SubmitTweet(twitterManager.CreateTweet(target));
                currentIndex++;
            }
            yield return null;
        }
        
        yield break;
	}
	
	public int GetNumberOfCells(EnhancedScroller scroller)
	{
		return _tweets.Count;
	}
	
	public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		return (_tweets[dataIndex].tweet.isText) ? textCellSize : imageCellSize;
	}
	
	public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		if(_tweets[dataIndex].tweet.isText)
		{
			TweetCellViewText cellView = scroller.GetCellView(tweetTextCellViewPrefab) as TweetCellViewText;
			cellView.SetData(_tweets[dataIndex]);
			return cellView;
		} else 
		{
			TweetCellViewImage cellView = scroller.GetCellView(tweetImageCellViewPrefab) as TweetCellViewImage;
			cellView.SetData(_tweets[dataIndex]);
			return cellView;		
		}
		
		Debug.LogError("???");
		return null;
	}
}