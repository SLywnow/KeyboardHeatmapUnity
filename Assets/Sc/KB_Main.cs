using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityRawInput;

public class KB_Main : MonoBehaviour
{
	[Header("Main")]
	public List<KB_Button> buttons;
	public KB_Button mousewheel;
	public Gradient col;
	public ulong maxpress;

	[Header("UI")]
	public Toggle enableWheel;
	public Toggle enableMouse;

	void Start()
	{
		RawInput.Start();
		RawInput.WorkInBackground = true;
		RawInput.OnKeyDown += HandleKeyDown;
	}

	private void HandleKeyDown(RawKey key)
	{
		string k = key.ToString();
		if (enableWheel.isOn && enableMouse.isOn && (k == "WheelUp" || k == "WheelDown"))
		{
			ProcessButton(mousewheel);
		}
		else
		{
			if (!enableMouse.isOn && (k == "LeftButton" || k== "RightButton" || k== "MiddleButton"))
			{
				return;
			}

			KB_Button b = buttons.Find(f => f.button.ToLower() == k.ToLower());

			ProcessButton(b);
		}

		//Debug.Log(key.ToString());
	}

	void ProcessButton(KB_Button b)
	{
		if (b != null)
		{
			b.pressCount++;
			if (b.pressCount > maxpress)
				maxpress = b.pressCount;

			foreach (KB_Button bt in buttons)
				bt.UpdateUI();
		}
	}

	public void Clear()
	{
		maxpress = 0;
		foreach (KB_Button bt in buttons)
		{
			bt.pressCount = 0;
			bt.UpdateUI();
		}
	}

	private void OnDisable()
	{
		RawInput.Stop();
		RawInput.OnKeyDown -= HandleKeyDown;
	}
}
