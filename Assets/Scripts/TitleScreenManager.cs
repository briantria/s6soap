/*
 * Brian Tria
 * 08/27/2015
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TitleScreenManager : MonoBehaviour
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

	public void OnClickDayMode ()
	{
		UIStateManager.Instance.ChangeUIState (UIState.OnGameScreen);
	}

	public void OnClickNightMode ()
	{
		UIStateManager.Instance.ChangeUIState (UIState.OnGameScreen);
	}
	
	private void ChangeUIState (UIState p_uiState)
	{
		bool bShowChild = false;

		p_uiState  &= UIState.OnTitleScreen;
		bShowChild  = p_uiState > 0;

		foreach (GameObject childObj in m_listChildrenObj)
		{
			childObj.SetActive (bShowChild);
		}
	}
}
