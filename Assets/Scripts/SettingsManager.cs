/*
 * Brian Tria
 * 08/27/2015
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SettingsManager : ScreenManager 
{
	override protected void ChangeUIState (UIState p_uiState)
	{
		bool bShowChild = false;

		p_uiState  &= UIState.OnSettingsScreen;
		bShowChild  = p_uiState > 0;
		
		foreach (GameObject childObj in m_listChildrenObj)
		{
			childObj.SetActive (bShowChild);
		}
	}

	public void OnClickClose ()
	{
		// remove settings from active screens
		UIStateManager.Instance.ActiveScreens &= ~UIState.OnSettingsScreen;
		UIStateManager.Instance.ChangeUIState (UIStateManager.Instance.ActiveScreens);
	}
}
