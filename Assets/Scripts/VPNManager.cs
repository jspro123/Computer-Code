using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VPNManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI textbox;
    public Color32 red;
    public Color32 green;
    
    private bool current = false;
    
    public void ToggleVPN()
    {
    	current = !current;

    	if(!current) { textbox.text = "OFF"; textbox.color = red; }
    	else { textbox.text = "ON"; textbox.color = green; }
    }
    
    // This function is called when the behaviour becomes disabled () or inactive.
    protected void OnDisable()
    {
    	if(current) { ToggleVPN(); }
    }
}
