using System.Collections.Generic;
using UnityEngine;

public class TweetData
{
	public string displayName;
    public string userName;
    public Sprite userPic;
    public TweetContent tweet;
    public string date;
    public int likes;
    public int retweets;
    public List<ReplyData> replies;
    public bool deepfake = false;
    
    public TweetData()
    {
    	replies = new List<ReplyData>();
    }
}
