﻿using UnityEngine;
using System.Collections;
using SLua;
using UnityEngine.UI;
using System.Collections.Generic;

public class Main : MonoBehaviour
{

	LuaSvr l;
	public Text logText;
	int progress=0;
	// Use this for initialization
	void Start()
	{
#if UNITY_5
		Application.logMessageReceived += this.log;
#else
		Application.RegisterLogCallback(this.log);
#endif

		l = new LuaSvr();
		l.init(tick,complete,LuaSvrFlag.LSF_DEBUG);
	}

	void log(string cond, string trace, LogType lt)
	{
		logText.text += (cond + "\n");

	}

	void tick(int p)
	{
		progress = p;
	}

	void complete()
	{
		l.start("main");
		object o = l.luaState.getFunction("foo").call(1, 2, 3);
		object[] array = (object[])o;
		for (int n = 0; n < array.Length; n++)
			Debug.Log(array[n]);

		string s = (string)l.luaState.getFunction("str").call(new object[0]);
		Debug.Log(s);
	}

	void OnGUI()
	{
		if(progress!=100)
			GUI.Label(new Rect(0, 0, 100, 50), string.Format("Loading {0}%", progress));
	}

}
