/*
 * Brian Tria
 * 08/27/2015
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GamePlayManager : MonoBehaviour 
{
	public static readonly int MAX_LEVEL_PATTERN_COUNT = 3;
	public static readonly float LEVEL_SPEED = 2.0f;
	public static readonly string TAG_OBSTACLE = "Obstacle";
	public static readonly string TAG_PLATFORM = "Platform";
	
	[SerializeField] private GameObject m_mainObject;
	
	private List<LevelPatternManager> m_listLevelPatterns = new List<LevelPatternManager>();

	protected void OnEnable ()
	{
		GameStateManager.changeGameState += ChangeGameState;
		UIStateManager.changeUIState += ChangeUIState;
	}

	protected void OnDisable ()
	{
		GameStateManager.changeGameState -= ChangeGameState;
		UIStateManager.changeUIState -= ChangeUIState;
	}

	protected void Awake ()
	{
		int iMultiplier  = 4;  // 4 level pattern elements
		    iMultiplier *= LevelPatternManager.MAX_COLUMN;
		LoadScreenManager.Instance.TotalInitObjectLoadCount += MAX_LEVEL_PATTERN_COUNT * iMultiplier;
	}

	protected void Start ()
	{
		SetupLevelPatterns ();
	}

	private void SetupLevelPatterns ()
	{
		for (int idx = 0; idx < MAX_LEVEL_PATTERN_COUNT; ++idx)
		{
			GameObject objLevelPattern = new GameObject ("LevelPattern" + idx);
			
			Transform tLevelPattern = objLevelPattern.transform;
			tLevelPattern.SetParent (this.transform);
			tLevelPattern.localScale = Vector3.one;
			
			LevelPatternManager levelPatternManager = objLevelPattern.AddComponent<LevelPatternManager> ();
			levelPatternManager.Setup (idx);
			levelPatternManager.OffsetPosition (new Vector3 (-5.0f, LevelPatternManager.LOWER_OFFSET_Y, 0.0f));
			m_listLevelPatterns.Add (levelPatternManager);
		}
	}

	private void ChangeUIState (UIState p_uiState)
	{
		// first, be sure we're not previously on title screen
		bool bShowChild = (UIState.OnTitleScreen & UIStateManager.Instance.ActiveScreens) <= 0;
		
		p_uiState  &= (UIState.OnGameScreen | UIState.OnSettingsScreen | UIState.OnResultScreen);
		bShowChild &= p_uiState > 0;
		m_mainObject.SetActive (bShowChild);
	}

	private void ChangeGameState (GameState p_gameState)
	{
		if (p_gameState == GameState.Inactive)
		{
			foreach (LevelPatternManager lpm in m_listLevelPatterns)
			{
				lpm.gameObject.SetActive (false);
			}

			return;
		}
		else if (p_gameState == GameState.Start)
		{
			foreach (LevelPatternManager lpm in m_listLevelPatterns)
			{
				lpm.gameObject.SetActive (true);
			}
			
			Invoke ("DelayRunningState", 0.02f);
		}
	}
	
	private void DelayRunningState ()
	{
		GameStateManager.Instance.IsPaused = false;
		GameStateManager.Instance.ChangeGameState (GameState.Running);
	}
}
