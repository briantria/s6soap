/*
 * Brian Tria
 * 08/27/2015
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameHudManager : ScreenManager 
{
	private static Vector2 m_v2MinScreenToWorldBounds;
	public  static Vector2 MinScreenToWorldBounds {get{return m_v2MinScreenToWorldBounds;}}
	
//	private static Vector2 m_v2MaxScreenToWorldBounds;
//	public  static Vector2 MaxScreenToWorldBounds {get{return m_v2MinScreenToWorldBounds;}}

	protected override void Awake ()
	{
		base.Awake ();
		
		m_v2MinScreenToWorldBounds = Camera.main.ScreenToWorldPoint (Vector2.zero);
//		m_v2MaxScreenToWorldBounds = Camera.main.ScreenToWorldPoint (Vector2.one);
	}

	override protected void ChangeUIState (UIState p_uiState)
	{
		// first, be sure we're not previously on title screen
		bool bShowChild = (UIState.OnTitleScreen & UIStateManager.Instance.ActiveScreens) <= 0;

		p_uiState  &= (UIState.OnGameScreen | UIState.OnSettingsScreen | UIState.OnResultScreen);
		bShowChild &= p_uiState > 0;
		
		foreach (GameObject childObj in m_listChildrenObj)
		{
			childObj.SetActive (bShowChild);
		}
	}

	public void OnClickPause ()
	{
		UIStateManager.Instance.ChangeUIState (UIState.OnSettingsScreen);
		GameStateManager.Instance.IsPaused = true;
	}
}
