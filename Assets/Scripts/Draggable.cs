/*
 * Brian Tria
 * 09/01/2015
 * 
 */

using UnityEngine;
using System.Collections;

public class Draggable : MonoBehaviour
{
	public static GameObject CurrentDragObject = null;

	public bool m_bHDrag = false;
	public bool m_bVDrag = false;

	private float m_fClickRange;
	private bool m_bWillDrag;
	private Vector3 m_v3PrevCursorPos;
	private Vector3 m_v3CurrCursorPos;
	private Transform m_transform;

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
		m_bWillDrag = false;
		m_transform = this.transform;
		m_fClickRange = this.GetComponent<SpriteRenderer>().bounds.size.x * this.transform.localScale.x;// * 0.5f;
	}

	protected void Update ()
	{
		if (//GameStateManager.Instance.IsPaused ||
		    GameStateManager.Instance.CurrentState != GameState.Running)
		{
			return;
		}

		if (Input.GetMouseButtonDown(0) &&
		    CurrentDragObject == null)
		{
			m_v3PrevCursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			m_bWillDrag = Vector2.Distance (m_v3PrevCursorPos, m_transform.position) < m_fClickRange;
		}

		if (m_bWillDrag == false) { return; }

		CurrentDragObject = this.gameObject;
		m_v3CurrCursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 v3NewPos = m_transform.position;

		if (m_bHDrag)
		{
			v3NewPos.x = m_v3CurrCursorPos.x;// += m_v3CurrCursorPos.x - m_v3PrevCursorPos.x;
		}

		if (m_bVDrag)
		{
			v3NewPos.y = m_v3CurrCursorPos.y;// += m_v3CurrCursorPos.y - m_v3PrevCursorPos.y;
		}

		m_transform.position = v3NewPos;
		m_v3PrevCursorPos = m_v3CurrCursorPos;

		//Debug.Log ("update drag: " + v3NewPos);

		if (Input.GetMouseButtonUp(0))
		{
			CurrentDragObject = null;
			m_bWillDrag = false;
		}
	}

	private void ChangeGameState (GameState p_gameState)
	{
		switch (p_gameState){
		case GameState.GameOver:
		{
			CurrentDragObject = null;
			m_bWillDrag = false;
			break;
		}}
	}
}
