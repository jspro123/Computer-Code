using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;

public class TweetCellViewText : EnhancedScrollerCellView
{
	[Header("Top Stuff")]
	public Image pic;
    public TMPro.TextMeshProUGUI tweetContent;
    public TMPro.TextMeshProUGUI displayName;
    public TMPro.TextMeshProUGUI userName;
    
    [Header("Bottom Stuff")]
    public TMPro.TextMeshProUGUI numReplies;
    public TMPro.TextMeshProUGUI numLikes;
    public TMPro.TextMeshProUGUI numRetweets;    
    
    private TweetCellViewText tweetToReplyTo;
    private ReplyScrollerController replyScrollerController;
    private List<ReplyData> replies; 
    
    protected void Awake()
    {
    	replies = new List<ReplyData>();
    	GameObject tmp = FindInActiveObjectByName("Replied Tweet Text");
    	if(tmp == null) { Debug.LogError("Can't find reply Tweet!"); }
    	else { tweetToReplyTo = tmp.GetComponent<TweetCellViewText>(); }

    	replyScrollerController = GameObject.Find("Reply Scroller Controller Text").GetComponent<ReplyScrollerController>();
    }
    
    private GameObject FindInActiveObjectByName(string name)
	{
	    Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
	    for (int i = 0; i < objs.Length; i++)
	    {
	        if (objs[i].hideFlags == HideFlags.None)
	        {
	            if (objs[i].name == name)
	            {
	                return objs[i].gameObject;
	            }
	        }
	    }
	    return null;
	}
    
    public void SetRepliedTweet()
    {
    	tweetToReplyTo.pic.sprite = pic.sprite;
    	tweetToReplyTo.tweetContent.text = tweetContent.text;
    	tweetToReplyTo.userName.text = userName.text;
    	tweetToReplyTo.displayName.text = displayName.text;

    	replyScrollerController.CreateReplies(replies);
    }
    
    public void SetData(TweetData data)
    {
    	tweetContent.text = data.tweet.tweetText;
    	displayName.text = data.displayName;
    	userName.text = data.userName;
    	pic.sprite = data.userPic;
    	replies = data.replies;
    	
    	numReplies.text = data.replies.Count.ToString();
    	numLikes.text = UnityEngine.Random.Range(1, 100).ToString();
    	numRetweets.text = UnityEngine.Random.Range(1, 100).ToString();
    }
}
