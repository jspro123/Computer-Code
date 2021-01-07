using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem.Extras;
using PixelCrushers.DialogueSystem;

public class ModifiedTextline : TextlineDialogueUI
{

    public override void Open()
    {
        base.Open();
		//OnApplyPersistentData();
    }

	public override void Close()
	{
		OnRecordPersistentData();
	    base.Close(); // Then allow it to close.
	}
	
	public void Record()
	{
		//OnRecordPersistentData();
	}

}
