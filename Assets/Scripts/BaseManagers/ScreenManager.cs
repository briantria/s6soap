/*
 * Brian Tria
 * 08/27/2015
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenManager : MonoBehaviour
{
	public static readonly float DEFAULT_ORTHOSIZE = 3.2f;
	protected List<GameObject> m_listChildrenObj = new List<GameObject>();
	
	virtual protected void OnEnable ()
	{
        UIStateManager.changeUIState += ChangeUIState;
        GameStateManager.changeGameState += ChangeGameState;
	}
	
	virtual protected void OnDisable ()
	{
        UIStateManager.changeUIState -= ChangeUIState;
        GameStateManager.changeGameState -= ChangeGameState;
	}
	
	virtual protected void Awake ()
	{
		LoadScreenManager.Instance.TotalInitObjectLoadCount += this.transform.childCount;
	}

	virtual protected void Start ()
	{
		foreach (Transform child in this.transform)
		{
			m_listChildrenObj.Add (child.gameObject);
		}
	}
    
    virtual protected void ChangeUIState (UIState p_uiState) { }
    virtual protected void ChangeGameState (GameState p_gameState) { }
}
