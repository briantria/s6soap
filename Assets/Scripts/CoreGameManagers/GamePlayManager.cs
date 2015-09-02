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
	public static readonly float LEVEL_TOP_SPEED = 5.0f;
	
	[SerializeField] private GameObject m_mainObject;
	private List<LevelPatternManager> m_listLevelPatterns = new List<LevelPatternManager>();

	private static GamePlayManager m_instance = null;
	public static GamePlayManager Instance {get{return m_instance;}}

	public float SpeedMultiplier { get; set; }

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
		m_instance = this;
		SpeedMultiplier = 1.0f;
		int iMultiplier  = 4;  // 4 level pattern elements
		    iMultiplier *= LevelPatternManager.MAX_COLUMN;
		LoadScreenManager.Instance.TotalInitObjectLoadCount += MAX_LEVEL_PATTERN_COUNT * iMultiplier;
	}

	protected void Start ()
	{
		SetupLevelPatterns ();
	}

	protected void Update ()
	{
		if (GameStateManager.Instance.IsPaused ||
		    GameStateManager.Instance.CurrentState != GameState.Running)
		{
			return;
		}

		if (LEVEL_SPEED * SpeedMultiplier < LEVEL_TOP_SPEED)
		{
			SpeedMultiplier += 0.002f;
		}
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
		m_mainObject.SetActive (false);//(bShowChild);
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

			SpeedMultiplier = 1.0f;
			Invoke ("DelayRunningState", 0.02f);
		}
	}
	
	private void DelayRunningState ()
	{
		GameStateManager.Instance.IsPaused = false;
		GameStateManager.Instance.ChangeGameState (GameState.Running);
	}
}
