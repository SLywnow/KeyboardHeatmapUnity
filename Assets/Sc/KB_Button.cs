using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KB_Button : MonoBehaviour
{
	public KB_Main main;
   public string button;
   public ulong pressCount;

	Image img;

	private void Awake()
	{
		//button = button.ToUpper();
		if (main == null)
		{
			main = FindAnyObjectByType<KB_Main>();
		}
		img=GetComponent<Image>();
		img.color = main.col.Evaluate(0);
	}

	public void UpdateUI()
	{
		if (main.maxpress > 0)
			img.color = main.col.Evaluate((float)pressCount / (float)main.maxpress);
		else
			img.color = main.col.Evaluate(0);
	}

}
