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
	private const int LEVEL_PATTERN_COUNT = 3;
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
		LoadScreenManager.Instance.TotalInitObjectLoadCount += LEVEL_PATTERN_COUNT * 4; // 4 level pattern elements
	}

	protected void Start ()
	{
		SetupLevelPatterns ();
	}

	private void SetupLevelPatterns ()
	{
		for (int idx = 0; idx < LEVEL_PATTERN_COUNT; ++idx)
		{
			GameObject objLevelPattern = new GameObject ("LevelPattern" + idx);
			
			Transform tLevelPattern = objLevelPattern.transform;
			tLevelPattern.SetParent (this.transform);
			tLevelPattern.position = Vector3.zero;
			tLevelPattern.localScale = Vector3.one;
			
			LevelPatternManager levelPatternManager = objLevelPattern.AddComponent<LevelPatternManager> ();
			levelPatternManager.Setup ();
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
		}
	}
}
