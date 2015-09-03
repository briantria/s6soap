/*
 * Brian Tria
 * 09/03/2015
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GermLayoutManager : MonoBehaviour 
{
	#region CONSTANTS
	private const    string  PREFAB_SOURCE_PATH = "Prefabs/Germ";

	private const    int     ROW_COUNT          = 3;
	private const    int     COL_COUNT           = 10;
	private const    int     MAX_GERM_COUNT = ROW_COUNT * COL_COUNT;

	private const    float   PARALLAX               = 0.3f;
    private const    float   BASE_SCALE           = 2.0f;
    private const    float   COL_INIT_POSY       = -1.0f;
    private const    float   COL_INIT_POSX       = 4.0f;

	private readonly Vector2 ACTUAL_SPRITE_SIZE = new Vector2 (166.0f, 144.0f) * Constants.PPU_MULTIPLIER;
	private readonly Vector2 PADDING                    = new Vector2 (0.9f, 1.2f);
	#endregion

	private Transform        m_transform;
	private Transform        m_tCurrGermFollowed;
	private Camera			  m_mainCamera;

	private int m_iCurrGermIdx;
	private int m_iSeed;

	private System.Random    m_random;
	private List<GameObject> m_listGerms = new List<GameObject> ();

	protected void Awake ()
	{
		m_mainCamera = Camera.main;
		m_transform = this.transform;
		m_iSeed = "br!4n".GetHashCode ();
		Reset ();
	}

	protected void Start ()
	{
		Setup ();
		m_tCurrGermFollowed = m_listGerms[0].transform;
	}

	protected void Update ()
	{
		if (//GameStateManager.Instance.IsPaused ||
		    GameStateManager.Instance.CurrentState != GameState.Running)
		{
			return;
		}

		Vector3 pos = m_transform.position;
		pos.x -= PARALLAX * GamePlayManager.LEVEL_SPEED * GamePlayManager.Instance.SpeedMultiplier * Time.deltaTime;
		m_transform.position = pos;

		// start from 0, check if every 3 object is offscreen
		// if yes, reposition (posx only) those objects and generate next pattern
		if (m_tCurrGermFollowed.position.x < (GameHudManager.MinScreenToWorldBounds.x - ACTUAL_SPRITE_SIZE.x - m_mainCamera.orthographicSize))
		{
			Vector3 newPos;
			int iUpperLimit = m_iCurrGermIdx + ROW_COUNT;

			for (int idx = m_iCurrGermIdx; idx < iUpperLimit; ++idx)
			{
				newPos    = m_listGerms[idx].transform.position;
				newPos.x += ACTUAL_SPRITE_SIZE.x * BASE_SCALE * PADDING.x * COL_COUNT;

				m_listGerms[idx].transform.position = newPos;
				GenerateNextGerm (idx);
			}

			m_iCurrGermIdx = iUpperLimit % MAX_GERM_COUNT;
			m_tCurrGermFollowed = m_listGerms[m_iCurrGermIdx].transform;
		}
	}

	private void Setup ()
	{
		for (int idx = 0; idx < MAX_GERM_COUNT; ++idx)
		{
			GameObject obj = Instantiate (Resources.Load(PREFAB_SOURCE_PATH)) as GameObject;
			m_listGerms.Add (obj);
			ResetGermPosition (obj, idx);
		}
	}

	private void ResetGermPosition (GameObject p_obj, int p_idx)
	{
		int colIdx = p_idx / ROW_COUNT;
		int rowIdx = p_idx % ROW_COUNT;
		
		float colPosY    = ACTUAL_SPRITE_SIZE.y * BASE_SCALE * PADDING.y;
        float colOffsetX = ACTUAL_SPRITE_SIZE.x * BASE_SCALE * PADDING.x * colIdx;
        float colOffsetY = (colPosY * rowIdx) - (colPosY * 0.5f * (colIdx % 2));
		
		Transform tObj = p_obj.transform;
		tObj.SetParent (this.transform);
		tObj.localScale *= BASE_SCALE;
        tObj.position = new Vector3 (  COL_INIT_POSX + colOffsetX,
		                                            COL_INIT_POSY - colOffsetY,
		                                            0.0f);
		GenerateNextGerm (p_idx);
	}

	public void Reset ()
	{
		m_random = new System.Random (m_iSeed);
		m_iCurrGermIdx = 0;
		m_transform.position = new Vector3 (0.0f, 0.0f, 0.0f);

		if (m_listGerms.Count == MAX_GERM_COUNT)
		{
			m_tCurrGermFollowed = m_listGerms[0].transform;

			int idx = 0;
			int jdx = MAX_GERM_COUNT - 1;

			do
			{
				ResetGermPosition (m_listGerms[idx], idx++);
				if (idx > jdx) { break; }

				ResetGermPosition (m_listGerms[jdx], jdx--);
			}
			while (jdx >= idx);
		}
	}

	// TODO: consider current map layout
	public void GenerateNextGerm (int p_idx)
	{
		bool bIsMid = (p_idx % ROW_COUNT) == (ROW_COUNT / 2);
		bool bIsVisible = bIsMid || m_random.NextDouble() < 0.5f;
		m_listGerms[p_idx].SetActive (bIsVisible);

		if (bIsVisible)
		{
			float multiplier = (float)(m_random.Next (50, 100)) * 0.01f;
			m_listGerms[p_idx].transform.localScale = Vector3.one * BASE_SCALE * multiplier;
		}
	}
}
