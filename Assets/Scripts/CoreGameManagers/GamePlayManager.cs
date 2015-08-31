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
	
	private List<LevelPatternManager> m_listLevelPatterns = new List<LevelPatternManager>();

	protected void OnEnable ()
	{
		GameStateManager.changeGameState += ChangeGameState;
	}

	protected void OnDisable ()
	{
		GameStateManager.changeGameState -= ChangeGameState;
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
			
			GameStateManager.Instance.IsPaused = false;
			GameStateManager.Instance.ChangeGameState (GameState.Running);
		}
	}
}
