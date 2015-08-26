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
	}
}
