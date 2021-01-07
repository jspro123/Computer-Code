using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnhancedUI.EnhancedScroller;

public class ReplyCellView : EnhancedScrollerCellView
{
	public Image pic;
    public TMPro.TextMeshProUGUI tweetContent;
    public TMPro.TextMeshProUGUI displayName;
    public TMPro.TextMeshProUGUI userName;
    
    public void SetData(ReplyData data)
    {
    	tweetContent.text = data.tweetContent;
    	displayName.text = data.displayName;
    	userName.text = data.userName;
    	pic.sprite = data.userPic;
    }
}
