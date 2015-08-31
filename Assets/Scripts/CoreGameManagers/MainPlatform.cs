/*
 * Brian Tria
 * 08/31/2015
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainPlatform : MonoBehaviour 
{
	private const string TILE_SOURCE_PATH = "Prefabs/MainPlatformTile";
	private const string GROUND_EDGE_SPRITE_PATH = "Images/Ground/ground_edge";
	private const string GROUND_MAIN_SPRITE_PATH = "Images/Ground/ground_main";
	
	private SpriteRenderer m_spriteRenderer;
	private List<SpriteRenderer> m_listSpriteRenderer = new List<SpriteRenderer> ();

	private GameObject AddTile (int p_idx)
	{
		GameObject objTile = Instantiate (Resources.Load(TILE_SOURCE_PATH)) as GameObject;
		objTile.name = "Tile" + p_idx;
		
		Transform tTile = objTile.transform;
		tTile.SetParent (this.transform);
		tTile.localScale = Vector3.one;
		tTile.localPosition = new Vector3 (p_idx * LevelPatternManager.COLUMN_WIDTH, 0.0f, 0.0f);
		
		return objTile;
	}

	public void Setup ()
	{
		Vector3 spriteScale = Vector3.one;
		
		for (int idx = 0; idx < LevelPatternManager.MAX_COLUMN; ++idx)
		{
			SpriteRenderer sr = AddTile(idx).GetComponent<SpriteRenderer> ();
			spriteScale.x = LevelPatternManager.COLUMN_WIDTH / sr.bounds.size.x;
			sr.transform.localScale = spriteScale;
			m_listSpriteRenderer.Add (sr);
		}
	}
}
