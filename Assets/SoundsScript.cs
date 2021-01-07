using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundsScript : MonoBehaviour
{
    public AudioSource source;
    public AudioClip door; 
    
    public void Ramp()
	{
		source.volume = source.volume;
    	DOTween.To(() => source.volume, x => source.volume = x, 0.6f, 15f);
    }
    
    public void EndGame()
    {
    	StartCoroutine(ReallyEnd());
    }
    
    public IEnumerator ReallyEnd()
    {
    	source.PlayOneShot(door, 2.0f);
    	yield return new WaitForSeconds(1.0f);
    	Application.Quit();
    }
}
