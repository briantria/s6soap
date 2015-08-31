/*
 * Brian Tria
 * 08/27/2015
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TitleScreenManager : ScreenManager
{
	override protected void ChangeUIState (UIState p_uiState)
	{
		// first, be sure we're not previously on game screen
		bool bShowChild = (UIState.OnGameScreen & UIStateManager.Instance.ActiveScreens) <= 0;

		p_uiState  &= (UIState.OnTitleScreen | UIState.OnSettingsScreen);
		bShowChild &= p_uiState > 0;

		foreach (GameObject childObj in m_listChildrenObj)
		{
			childObj.SetActive (bShowChild);
		}
	}
	
	public void OnClickDayMode ()
	{
		UIStateManager.Instance.ActiveScreens = UIState.Reset;
		UIStateManager.Instance.ChangeUIState (UIState.OnGameScreen);
		GameStateManager.Instance.ChangeGameState (GameState.Start);
	}
	
	public void OnClickNightMode ()
	{
		UIStateManager.Instance.ActiveScreens = UIState.Reset;
		UIStateManager.Instance.ChangeUIState (UIState.OnGameScreen);
		GameStateManager.Instance.ChangeGameState (GameState.Start);
	}
}
