/*
 * Brian Tria
 * 08/31/2015
 * 
 */

using UnityEngine;
using System.Collections;

public class LevelPatternManager : MonoBehaviour 
{
	public static readonly int   MAX_COLUMN = 10;
	public static readonly float LOWER_OFFSET_Y = -2.0f;
	public static readonly float UPPER_OFFSET_Y =  2.0f;

	private Transform m_transform;

	private float m_fWidth = 0;
	public  float Width {get{return m_fWidth;}}

	private GameObject CreateLevelPatternElement (string p_strName)
	{
		GameObject obj = new GameObject (p_strName);

		Transform tObj = obj.transform;
		tObj.SetParent (m_transform);
		tObj.position = Vector3.zero;
		tObj.localScale = Vector3.one;

		return obj;
	}
	
	protected void Update ()
	{
		if (GameStateManager.Instance.IsPaused &&
		    GameStateManager.Instance.CurrentState != GameState.Running)
	    {
	    	return;
	    }
	    
		Vector3 v3NewPos = m_transform.position;
		v3NewPos.x -= GamePlayManager.LEVEL_SPEED * Time.deltaTime;
		
		if (v3NewPos.x < GameHudManager.MinScreenToWorldBounds.x - m_fWidth)
		{
			v3NewPos.x += m_fWidth * GamePlayManager.MAX_LEVEL_PATTERN_COUNT;
		}
		
		m_transform.position = v3NewPos;
	}

	public void Setup (int p_idx)
	{
		m_transform = this.transform;
	
		MainPlatform mainPlatform = CreateLevelPatternElement("MainPlatform").AddComponent<MainPlatform> ();
//		RectPlatform rectPlatform = CreateLevelPatternElement("RectPlatform").AddComponent<RectPlatform> ();
//		RectObstacle rectObstacle = CreateLevelPatternElement("RectObstacle").AddComponent<RectObstacle> ();
		TrglObstacle trglObstacle = CreateLevelPatternElement("TrglObstacle").AddComponent<TrglObstacle> ();

		mainPlatform.Setup ();
		trglObstacle.Setup ();

		m_fWidth = MAX_COLUMN * mainPlatform.TileWidth;
		m_transform.position = new Vector3 (m_fWidth * p_idx, 0.0f, 0.0f);
	}
	
	public void OffsetPosition (Vector3 p_v3Offset)
	{
		m_transform.position += p_v3Offset;
	}
}
