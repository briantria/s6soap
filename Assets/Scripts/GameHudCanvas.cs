/*
 * Brian Tria
 * 08/27/2015
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameHudCanvas : MonoBehaviour 
{
	private List<GameObject> m_listChildrenObj = new List<GameObject>();

	protected void OnEnable ()
	{
		UIStateManager.changeUIState += ChangeUIState;
	}
	
	protected void OnDisable ()
	{
		UIStateManager.changeUIState -= ChangeUIState;
	}

	protected void Awake ()
	{
		LoadScreenManager.Instance.TotalInitObjectLoadCount += this.transform.childCount;
		
		foreach (Transform child in this.transform)
		{
			m_listChildrenObj.Add (child.gameObject);
		}
	}

	private void ChangeUIState (UIState p_uiState)
	{
		bool bShowChild = false;
		
		p_uiState  &= UIState.OnGameScreen;
		//p_uiState  &= (UIState.OnGameScreen | UIState.OnResultScreen);
		bShowChild  = p_uiState > 0;
		
		foreach (GameObject childObj in m_listChildrenObj)
		{
			childObj.SetActive (bShowChild);
		}
	}

	public void OnClickPause ()
	{

	}
}
