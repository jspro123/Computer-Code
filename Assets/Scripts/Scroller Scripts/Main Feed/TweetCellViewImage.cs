using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;

public class TweetCellViewImage : EnhancedScrollerCellView
{
	[Header("Top Stuff")]
	public Image pic;
    public Image tweetContent;
    public TMPro.TextMeshProUGUI displayName;
    public TMPro.TextMeshProUGUI userName;
    
    [Header("Bottom Stuff")]
    public TMPro.TextMeshProUGUI numReplies;
    public TMPro.TextMeshProUGUI numLikes;
    public TMPro.TextMeshProUGUI numRetweets;
    
    private TweetCellViewImage tweetToReplyTo;
    private ReplyScrollerController replyScrollerController;
    private List<ReplyData> replies; 
    
    protected void Awake()
    {
    	replies = new List<ReplyData>();
    	GameObject tmp = FindInActiveObjectByName("Replied Tweet Image");
    	if(tmp == null) { Debug.LogError("Can't find reply Tweet!"); }
    	else { tweetToReplyTo = tmp.GetComponent<TweetCellViewImage>(); }
    	
    	replyScrollerController = GameObject.Find("Reply Scroller Controller Image").GetComponent<ReplyScrollerController>();
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
    	tweetToReplyTo.tweetContent.sprite = tweetContent.sprite;
    	tweetToReplyTo.userName.text = userName.text;
    	tweetToReplyTo.displayName.text = displayName.text;
    	replyScrollerController.CreateReplies(replies);
    }
    
    public void SetData(TweetData data)
    {
		tweetContent.sprite = data.tweet.tweetPic;
    	displayName.text = data.displayName;
    	userName.text = data.userName;
    	pic.sprite = data.userPic;
    	replies = data.replies;
    	
    	numReplies.text = data.replies.Count.ToString();
    	numLikes.text = UnityEngine.Random.Range(1, 100).ToString();
    	numRetweets.text = UnityEngine.Random.Range(1, 100).ToString();
    	
    }
}
