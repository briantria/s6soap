﻿/*
 * Brian Tria
 * 09/03/2015
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GermLayoutManager : MonoBehaviour 
{
	private const    string  PREFAB_SOURCE_PATH = "Prefabs/Germ";
	private const    int     MAX_GERM_COUNT     = 9;//60;
	private const    float   PARALLAX           = 0.5f;
	private const    float   COL_INIT_POSY      = 6.0f;
	private readonly Vector2 ACTUAL_SPRITE_SIZE = new Vector2 (166.0f, 144.0f) * Constants.PPU_MULTIPLIER;
	private readonly Vector2 PADDING            = new Vector2 (0.9f, 1.2f);

	private Transform        m_transform;
	private float            m_fWidth;
	private int              m_iCurrColumn;
	private int              m_iSeed;
	private System.Random    m_random;
	private List<GameObject> m_listGerms = new List<GameObject> ();

	protected void Awake ()
	{
		m_transform = this.transform;
		m_iSeed = "papabao".GetHashCode ();
		Reset ();
	}

	protected void Start ()
	{
		Setup ();
	}

	protected void Update ()
	{
		if (GameStateManager.Instance.IsPaused ||
		    GameStateManager.Instance.CurrentState != GameState.Running)
		{
			return;
		}

		Vector3 pos = m_transform.position;
		pos.x -= PARALLAX * GamePlayManager.LEVEL_SPEED * GamePlayManager.Instance.SpeedMultiplier * Time.deltaTime;

		// start from 0, check if every 3 object is offscreen
		// if yes, reposition those objects and generate next pattern
	}

	private void Setup ()
	{
		float fBaseScale = 2.0f;

		for (int idx = 0; idx < MAX_GERM_COUNT; ++idx)
		{
			GameObject obj = Instantiate (Resources.Load(PREFAB_SOURCE_PATH)) as GameObject;

			int col = idx / 3;
			int row = idx % 3;

			float colPosY    = ACTUAL_SPRITE_SIZE.y * fBaseScale * PADDING.y;
			float colOffsetY = (colPosY * row) - (colPosY * 0.5f * (col % 2));

			Transform tObj = obj.transform;
			tObj.SetParent (this.transform);
			tObj.localScale *= fBaseScale;
			tObj.position = new Vector3 (ACTUAL_SPRITE_SIZE.x * fBaseScale * PADDING.x * col,
			                             COL_INIT_POSY - colOffsetY,
			                             0.0f);

			m_listGerms.Add (obj);
		}
	}

	public void Reset ()
	{
		m_random = new System.Random (m_iSeed);
		m_transform.position = new Vector3 (0.0f, 0.0f, 0.0f);
	}

	public void GenerateNextPattern ()
	{
		// offset pos.x of germ set
		// randomize which are visible
		// middle (or atleast 2?) should always be visible 
	}
}
