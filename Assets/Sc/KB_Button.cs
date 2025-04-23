using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KB_Button : MonoBehaviour
{
	public KB_Main main;
   public string button;
   public ulong pressCount;
	public float time;
	public bool press;

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

	private void Update()
	{
		if (main.timerMode.isOn)
		{
			if (press)
			{
				time += Time.deltaTime;
				if (main.maxtime< time)
					main.maxtime = time;
			}

			if (main.maxtime > 0)
				img.color = main.col.Evaluate((float)time / (float)main.maxtime);
			else
				img.color = main.col.Evaluate(0);
		}
	}

	public void UpdateUI()
	{
		if (!main.timerMode.isOn)
		{
			if (main.maxpress > 0)
				img.color = main.col.Evaluate((float)pressCount / (float)main.maxpress);
			else
				img.color = main.col.Evaluate(0);
		}
	}

}
