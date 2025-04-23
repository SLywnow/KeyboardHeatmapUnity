using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityRawInput;
using SLywnow;

public class KB_Main : MonoBehaviour
{
	[Header("Main")]
	public List<KB_Button> buttons;
	public KB_Button mousewheel;
	public Gradient col;
	public ulong maxpress;
	public float maxtime;

	[Header("UI")]
	public Toggle enableWheel;
	public Toggle enableMouse;
	public Toggle timerMode;

	[Header("Saves")]
	public KB_Save def;
	public KB_Save loaded;

	private void Awake()
	{
		if (FilesSet.CheckFile(Application.dataPath + "/save.dat",false))
		{
			loaded = JsonUtility.FromJson<KB_Save>(FilesSet.LoadStream(Application.dataPath + "/save.dat", false, false));
			Load();
		}
		else
		{
			RestoreDefault();
		}
	}

	void Start()
	{
		RawInput.Start();
		RawInput.WorkInBackground = true;
		RawInput.OnKeyDown += HandleKeyDown;
		RawInput.OnKeyUp += HandleKeyUp;

		timerMode.onValueChanged.AddListener((bool b) => Clear());
	}

	private void HandleKeyDown(RawKey key)
	{
		string k = key.ToString();

		if (timerMode.isOn)
		{
			KB_Button b = buttons.Find(f => f.button.ToLower() == k.ToLower());

			if (b != null)
			{
				b.press = true;
			}
		}
		else
		{
			if (enableWheel.isOn && enableMouse.isOn && (k == "WheelUp" || k == "WheelDown"))
			{
				ProcessButton(mousewheel);
			}
			else
			{
				if (!enableMouse.isOn && (k == "LeftButton" || k == "RightButton" || k == "MiddleButton"))
				{
					return;
				}

				KB_Button b = buttons.Find(f => f.button.ToLower() == k.ToLower());

				ProcessButton(b);
			}
		}
		//Debug.Log(key.ToString());
	}

	private void HandleKeyUp(RawKey key)
	{
		string k = key.ToString();
		if (timerMode.isOn)
		{
			KB_Button b = buttons.Find(f => f.button.ToLower() == k.ToLower());

			if (b != null)
			{
				b.press = false;
			}
		}
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
		maxtime = 0;
		foreach (KB_Button bt in buttons)
		{
			bt.pressCount = 0;
			bt.time = 0;
			bt.UpdateUI();
		}
	}

	public void SelectGrad()
	{
		GradientPicker.Create(col, "Select button's gradient", GradUpdate, GradDone);
	}

	void GradUpdate(Gradient g)
	{

	}

	void GradDone(Gradient g)
	{
		col = g;

		foreach (KB_Button bt in buttons)
		{
			bt.UpdateUI();
		}
	}

	public void RestoreDefault()
	{
		loaded = def;
		Load();
	}
	
	void Load()
	{
		enableWheel.isOn = loaded.enableWheel;
		enableMouse.isOn = loaded.enableMouse;
		timerMode.isOn = loaded.timerMode;
		col = loaded.col;
	}

	public void Save()
	{
		loaded = new KB_Save();
		loaded.enableWheel = enableWheel.isOn;
		loaded.enableMouse = enableMouse.isOn;
		loaded.timerMode = timerMode.isOn;
		loaded.col = col;

		FilesSet.SaveStream(Application.dataPath + "/save.dat", JsonUtility.ToJson(loaded, true), false, false);
	}

	private void OnDisable()
	{
		RawInput.Stop();
		RawInput.OnKeyDown -= HandleKeyDown;
		RawInput.OnKeyUp -= HandleKeyUp;
	}
}

[System.Serializable]
public class KB_Save
{
	public bool enableWheel;
	public bool enableMouse;
	public bool timerMode;
	public Gradient col;
}