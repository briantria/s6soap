/*
 * Brian Tria
 * 08/27/2015
 * 
 */

using UnityEngine;
using System;
using System.Collections;

public class UIStateManager : MonoBehaviour 
{
	private static UIStateManager m_instance = null;
	public  static UIStateManager Instance {get{return m_instance;}}

	public delegate void UIStateDelegate (UIState p_uiState);
	public static event UIStateDelegate changeUIState;

	protected void Awake ()
	{
		m_instance = this;
	}

	public void ChangeUIState (UIState p_uiState)
	{
		if (changeUIState != null)
		{
			changeUIState (p_uiState);
		}
	}
}

[Flags]
public enum UIState
{
	NoActiveScreen = 0,
	OnTitleScreen  = 1 << 0, // 0x01
	OnGameScreen   = 1 << 1, // 0x02
	OnResultScreen = 1 << 2  // 0x04
}