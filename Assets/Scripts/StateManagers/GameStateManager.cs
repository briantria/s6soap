/*
 * Brian Tria
 * 08/27/2015
 * 
 */

using UnityEngine;
using System;
using System.Collections;

public class GameStateManager : MonoBehaviour 
{
	private static GameStateManager m_instance = null;
	public  static GameStateManager Instance { get { return m_instance; }}

	public delegate void GameStateDelegate (GameState p_gameState);
	public static event GameStateDelegate changeGameState;

	public GameState CurrentState { get; set; }
	public bool      IsPaused     { get; set; }

	protected void Awake ()
	{
		m_instance = this;
		IsPaused = false;
	}

	public void ChangeGameState (GameState p_gameState)
	{
		if (changeGameState != null)
		{
			changeGameState (p_gameState);
		}

		CurrentState = p_gameState;
	}
}

[Flags]
public enum GameState
{
	Inactive = 0,
	Idle     = 1 << 0, // 0x01
	Start    = 1 << 1, // 0x02
	Running  = 1 << 2, // 0x04
	GameOver = 1 << 3, // 0x08
	Restart  = 1 << 4  // 0x16
}