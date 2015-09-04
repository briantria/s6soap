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
	public static readonly float LEVEL_TOP_SPEED = 4.0f;
	
	[SerializeField] private MainObject m_mainObject;
	[SerializeField] private GermLayoutManager m_germLayoutManager;

	private List<LevelPatternManager> m_listLevelPatterns = new List<LevelPatternManager>();

	private static GamePlayManager m_instance = null;
	public static GamePlayManager Instance {get{return m_instance;}}

	public float SpeedMultiplier { get; set; }

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
		if (//GameStateManager.Instance.IsPaused ||
		    GameStateManager.Instance.CurrentState != GameState.Running)
		{
			return;
		}

		if (LEVEL_SPEED * SpeedMultiplier < LEVEL_TOP_SPEED)
		{
			SpeedMultiplier += 0.05f * Time.deltaTime;
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
			m_listLevelPatterns.Add (levelPatternManager);
		}
	}

	private void ChangeGameState (GameState p_gameState)
	{
		switch (p_gameState){
		case GameState.Start:
		{
			MapGenerator.Instance.Reset ();
			m_mainObject.gameObject.SetActive (true);
			m_mainObject.RBody2D.isKinematic = false;
			m_mainObject.Reset ();
			m_germLayoutManager.Reset ();

			foreach (LevelPatternManager lpm in m_listLevelPatterns)
            {
                lpm.gameObject.SetActive (true);
				lpm.Reset ();
			}

			SpeedMultiplier = 1.0f;

			break;
		}
		case GameState.Inactive:
		{
			m_mainObject.Reset ();
			m_mainObject.gameObject.SetActive (false);
			m_germLayoutManager.Reset ();

			foreach (LevelPatternManager lpm in m_listLevelPatterns)
			{
				lpm.gameObject.SetActive (false);
			}

			break;
		}
        case GameState.Idle:
        {
            MapGenerator.Instance.Reset ();
            
            for (int idx = 0; idx < MAX_LEVEL_PATTERN_COUNT; ++idx)
            {
                m_listLevelPatterns[idx].Reset ();
            }
            
            m_germLayoutManager.Reset ();
            
            break;
        }
		case GameState.Restart:
		{
			m_mainObject.RBody2D.WakeUp ();
            m_mainObject.RBody2D.isKinematic = false;

			SpeedMultiplier = 1.0f;

			break;
		}
		case GameState.Running:
		{
			break;
		}}
	}
}
