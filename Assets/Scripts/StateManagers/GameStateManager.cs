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

	protected void Awake ()
	{
		m_instance = this;
	}

	public void ChangeGameState (GameState p_gameState)
	{
		if (changeGameState != null)
		{
			changeGameState (p_gameState);
		}
	}
}

[Flags]
public enum GameState
{
	Inactive = 0,
	Idle     = 1 << 0, // 0x01
	Running  = 1 << 1, // 0x02
	GameOver = 1 << 2, // 0x04
	Restart  = 1 << 3  // 0x08
}