using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[Serializable]
public class User
{
	public string userName;
	public string displayName;
	public Sprite pic;
	
	public User(string _userName, string _displayName, Sprite _pic)
	{
		userName = _userName;
		displayName = _displayName;
		pic = _pic;
	}
}

[Serializable]
public class TweetContent
{
	public bool isText = true;
	public string tweetText;
	public Sprite tweetPic;
	
	public TweetContent(string tweet)
	{
		isText = true;
		tweetText = tweet;
	}
	
	public TweetContent(Sprite tweet)
	{
		isText = false;
		tweetPic = tweet;
	}
}

[Serializable]
public class SourceFiles
{
	public TextAsset namesFile;
	public TextAsset usernamesFile;
	public TextAsset textTweetsFile;
	public string imageTweetsFile; //directory
	public string profilesFile; //directory
	public TextAsset repliesFile;
	
	public bool botFiles = false;
	public bool playerFiles = true;
	public bool deepfake = false;
	public string triggerName = "";
}

[Serializable]
public class ProcessedFiles
{
    public List<string> names, usernames, replies;
    public List<TweetContent> tweets;
	public List<Sprite> profiles;
	public List<User> users;
	
	public bool playerFiles = true;
	public bool botFiles = false;
	public bool deepfake = false;
	public string triggerName = "";
	
	public ProcessedFiles()
	{
		names = new List<string>();
		usernames = new List<string>();
		replies = new List<string>();
		tweets = new List<TweetContent>();
		profiles = new List<Sprite>();
		users = new List<User>();
	}
}

public class TwitterManager : MonoBehaviour
{
	[Header("Tweets and Settings")]
	[Range(0,10)]
	public int minReplies;
	[Range(0,10)]
	public int maxReplies;
	public List<SourceFiles> sourceFiles;
	
	[Header("Profile Fields")]
	public Image profile;
	public TMPro.TextMeshProUGUI displayName;
	public TMPro.TextMeshProUGUI userName;
	
	[Header("Misc. settings")]
	public Color32 hashtagColor;
	public List<String> thingsToTweet;

	private List<ProcessedFiles> processedFiles;
	private ProcessedFiles generic;
	
    void Awake()
    {
    	processedFiles = new List<ProcessedFiles>();

		for(int i = 0; i < sourceFiles.Count; i++)
		{
			ProcessSourceFiles(sourceFiles[i]);
			CreateUsers(processedFiles[i]);
		}

		generic = processedFiles[0];
    }
    
    private void OnValidate()
    {
        if(minReplies > maxReplies) { minReplies = maxReplies; }
    }


    private void ProcessSourceFiles(SourceFiles source)
    {
    	ProcessedFiles processed = new ProcessedFiles();
    	
		processed.names.AddRange(source.namesFile.text.Split('\n'));
		processed.usernames.AddRange(source.usernamesFile.text.Split('\n'));
		processed.replies.AddRange(source.repliesFile.text.Split('\n'));
		
		Sprite[] tmpProfilePics = Resources.LoadAll<Sprite>(source.profilesFile);
		processed.profiles.AddRange(tmpProfilePics);

		
		string[] tmpTextTweets = source.textTweetsFile.text.Split('\n');
		Sprite[] tmpTweetPics = Resources.LoadAll<Sprite>(source.imageTweetsFile);
		for(int i = 0; i < tmpTextTweets.Length; i++)
		{
			if(tmpTextTweets.Length == 1) { break; }
			processed.tweets.Add(new TweetContent(tmpTextTweets[i]));
		}
		for(int i = 0; i < tmpTweetPics.Length; i++)
		{
			processed.tweets.Add(new TweetContent(tmpTweetPics[i]));
		}
				
		RandomizeList(processed.names);
		RandomizeList(processed.usernames);
		RandomizeList(processed.replies);
		RandomizeList(processed.tweets);
		RandomizeList(processed.profiles);
		
		processed.botFiles = source.botFiles;
		processed.playerFiles = source.playerFiles;
		processed.deepfake = source.deepfake;
		processed.triggerName = source.triggerName;
		
		if(processed.playerFiles)
		{
			profile.sprite = processed.profiles[0];
			displayName.text = processed.names[0];
			userName.text = "@" + processed.usernames[0];
		}
		
		processedFiles.Add(processed);
    }
   	
	private static void RandomizeList<T>(List<T> list)
	{
         for (int i = 0; i < list.Count; i++) {
             T temp = list[i];
             int randomIndex = UnityEngine.Random.Range(i, list.Count);
             list[i] = list[randomIndex];
             list[randomIndex] = temp;
         }
	}
	
	private void CreateUsers(ProcessedFiles processed)
	{		
		for(int i = 0; i < processed.usernames.Count; i++)
		{
			string curUsername = processed.usernames[i];
			if(i >= processed.names.Count || i >= processed.profiles.Count) { Debug.Log("Out of Material!"); return; }
			string curDisplay = processed.names[i];
			Sprite curPic = processed.profiles[i];
			
			User user = new User(curUsername, curDisplay, curPic);
			processed.users.Add(user);
		}
	}
	
	private string ColorHashtags(string tweet)
	{
		string[] split = tweet.Split(' ');
		string final = "";
		for(int i = 0; i < split.Length; i++)
		{
			if(split[i].StartsWith("#"))
			{
				split[i] = "<color=#" + ColorUtility.ToHtmlStringRGBA(hashtagColor) + ">" + split[i] + "</color>";
			}
			
			final += split[i] + " ";
		}
		
		return final;
	} 
	
	public TweetData CreateGenericTweet()
	{
		TweetData data = new TweetData();
		int userIndex = UnityEngine.Random.Range(0, generic.users.Count);
		int tweetIndex = UnityEngine.Random.Range(0, generic.tweets.Count);
		User user = generic.users[userIndex];
		string username = "@" + user.userName;
		string display = user.displayName;
		Sprite pic = user.pic;
		TweetContent tweet = generic.tweets[tweetIndex];
		if(tweet.isText) { tweet.tweetText = ColorHashtags(tweet.tweetText); }
		
		data.tweet = tweet;
		data.userName = username;
		data.displayName = display;
		data.userPic = pic;
		
		int numReplies = UnityEngine.Random.Range(minReplies, maxReplies);
		for(int i = 0; i < numReplies; i++)
		{
			ReplyData genReply = CreateGenericReply();
			data.replies.Add(genReply);
		}
			
		return data;
	}
	
	public ReplyData CreateGenericReply()
	{
		ReplyData data = new ReplyData();
		int userIndex = UnityEngine.Random.Range(0, generic.users.Count);
		int replyIndex = UnityEngine.Random.Range(0, generic.replies.Count);
		User user = generic.users[userIndex];
		string username = "@" + user.userName;
		string display = user.displayName;
		Sprite pic = user.pic;
		string tweet = generic.replies[replyIndex];
		
		data.tweetContent = ColorHashtags(tweet);
		data.userName = username;
		data.displayName = display;
		data.userPic = pic;
		
		return data;
	}
	
	public TweetData CreateTweet(string target)
	{
		ProcessedFiles processed = processedFiles.Find(x => (x.triggerName == target));
		TweetData data = new TweetData();
		int userIndex = UnityEngine.Random.Range(0, processed.users.Count);
		int tweetIndex = UnityEngine.Random.Range(0, processed.tweets.Count);
		User user = processed.users[userIndex];
		string username = "@" + user.userName;
		string display = user.displayName;
		Sprite pic = user.pic;
		TweetContent tweet = processed.tweets[tweetIndex];
		if(tweet.isText) { tweet.tweetText = ColorHashtags(tweet.tweetText); }
		data.tweet = tweet;
		data.userName = username;
		data.displayName = display;
		data.userPic = pic;
		int numReplies = UnityEngine.Random.Range(minReplies, maxReplies);
		for(int i = 0; i < numReplies; i++)
		{
			ReplyData genReply = CreateReply(target);
			data.replies.Add(genReply);
		}
		
		if(processed.deepfake) { data.deepfake = true; }
		return data;
	}
	
	public ReplyData CreateReply(string target)
	{
		ProcessedFiles processed = processedFiles.Find(x => (x.triggerName == target));
		ReplyData data = new ReplyData();
		int userIndex = UnityEngine.Random.Range(0, generic.users.Count);
		int replyIndex = UnityEngine.Random.Range(0, processed.replies.Count);
		User user = generic.users[userIndex];
		string username = "@" + user.userName;
		string display = user.displayName;
		Sprite pic = user.pic;
		string tweet = processed.replies[replyIndex];
		
		data.tweetContent = ColorHashtags(tweet);
		data.userName = username;
		data.displayName = display;
		data.userPic = pic;
		
		return data;
	}
}