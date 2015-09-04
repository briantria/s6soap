/*
 * Brian Tria
 * 09/04/2015
 * 
 */

using UnityEngine;
using System.Collections;

public class BGMManager : MonoBehaviour 
{
	[SerializeField] private AudioClip m_audioMenu;
	[SerializeField] private AudioClip m_audioGame;

	private AudioSource m_audioSource;

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
		m_audioSource = this.GetComponent<AudioSource> ();
	}

	private void ChangeGameState (GameState p_gameState)
	{
		switch (p_gameState){
		case GameState.Restart:
		{
			if(m_audioSource.isPlaying)
			{
				m_audioSource.Stop ();
			}

			m_audioSource.clip = m_audioGame;
			m_audioSource.Play ();

			break;
		}
		case GameState.Idle:
		{
			if(m_audioSource.isPlaying)
			{
				m_audioSource.Stop ();
			}
			
			m_audioSource.clip = m_audioMenu;
			m_audioSource.Play ();

			break;
		}}
	}
}
