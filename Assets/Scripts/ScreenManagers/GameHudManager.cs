/*
 * Brian Tria
 * 08/27/2015
 * 
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameHudManager : ScreenManager 
{
    private static GameHudManager m_instance = null;
    public   static GameHudManager Instance {get{return m_instance;}}

    [SerializeField] private Text m_textScore;

	private static Vector2 m_v2MinScreenToWorldBounds;
	public  static Vector2 MinScreenToWorldBounds {get{return m_v2MinScreenToWorldBounds;}}
	
//	private static Vector2 m_v2MaxScreenToWorldBounds;
//	public  static Vector2 MaxScreenToWorldBounds {get{return m_v2MinScreenToWorldBounds;}}

    public int Score { get; set; }

	protected override void Awake ()
	{
		base.Awake ();
        m_instance = this;
		
		m_v2MinScreenToWorldBounds = Camera.main.ScreenToWorldPoint (Vector2.zero);
//		m_v2MaxScreenToWorldBounds = Camera.main.ScreenToWorldPoint (Vector2.one);
	}

	override protected void ChangeUIState (UIState p_uiState)
	{
		// be sure we're not previously on title screen
		bool bShowChild = (UIState.OnTitleScreen & UIStateManager.Instance.ActiveScreens) <= 0;
		p_uiState  &= (UIState.OnGameScreen | UIState.OnSettingsScreen | UIState.OnResultScreen);
		bShowChild &= p_uiState > 0;
		
		foreach (GameObject childObj in m_listChildrenObj)
		{
			childObj.SetActive (bShowChild);
		}

        m_textScore.gameObject.SetActive (true);

		// reset camera
		if (p_uiState == UIState.OnTitleScreen)
		{
			Camera.main.orthographicSize = DEFAULT_ORTHOSIZE;
		}
	}
    
    protected override void ChangeGameState (GameState p_gameState)
    {
        switch (p_gameState){
        case GameState.Start:
        case GameState.Restart:
        {
            UpdateScore (0, true);
            break;
        }}
    }

    public void OnClickPlay ()
    {
        GameStateManager.Instance.ChangeGameState (GameState.Restart);
    }

	public void OnClickPause ()
	{
		UIStateManager.Instance.ChangeUIState (UIState.OnSettingsScreen);
		GameStateManager.Instance.IsPaused = true;
	}
    
    public void UpdateScore (int p_iDeltaScore, bool p_bOverWrite = false)
    {
        Score = p_bOverWrite ? p_iDeltaScore : Mathf.Max (0, Score + p_iDeltaScore);
        m_textScore .text = "" + Score;
    }
}
