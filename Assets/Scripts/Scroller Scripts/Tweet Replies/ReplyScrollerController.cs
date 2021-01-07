using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using System;

public class ReplyScrollerController : MonoBehaviour, IEnhancedScrollerDelegate
{
	[Header("Prefabs and Scroller")]
	public EnhancedScroller myScroller;
	public ReplyCellView replyCellViewPrefab;

	[Header("Misc. Settings")]
	public int replyCellSize;
	[Range(0,10)]
	public int minGenericReplies;
	[Range(0,10)]
	public int maxGenericReplies;
	
	private List<ReplyData> _replies;
	private TwitterManager twitterManager;

    private void OnValidate()
    {
        if(minGenericReplies > maxGenericReplies) { minGenericReplies = maxGenericReplies; }
    }

	void Awake ()
	{
		_replies = new List<ReplyData>();
		twitterManager = FindObjectOfType<TwitterManager>();
		myScroller.Delegate = this;
	}
	
	public void CreateReplies(List<ReplyData> replies)
	{
		_replies.Clear();
		_replies.AddRange(replies);
		myScroller.ReloadData();
	}
	
	
	public int GetNumberOfCells(EnhancedScroller scroller)
	{
		return _replies.Count;
	}
	
	public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		return replyCellSize;
	}
	
	public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		ReplyCellView cellView = scroller.GetCellView(replyCellViewPrefab) as ReplyCellView;
		cellView.SetData(_replies[dataIndex]);
		return cellView;
	}
}