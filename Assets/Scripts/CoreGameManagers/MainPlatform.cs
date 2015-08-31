/*
 * Brian Tria
 * 08/31/2015
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainPlatform : LevelElementManager 
{
	private const string PREFAB_SOURCE_PATH = "Prefabs/MainPlatformTile";
	private const string GROUND_EDGE_SPRITE_PATH = "Images/Ground/ground_edge";
	private const string GROUND_MAIN_SPRITE_PATH = "Images/Ground/ground_main";
	
	public void Setup ()
	{
		Vector3 spriteScale = Vector3.one;
		
		for (int idx = 0; idx < LevelPatternManager.MAX_COLUMN; ++idx)
		{
			SpriteRenderer sr = AddElement(PREFAB_SOURCE_PATH, idx).GetComponent<SpriteRenderer> ();
			spriteScale.x = LevelPatternManager.COLUMN_WIDTH / sr.bounds.size.x;
			sr.transform.localScale = spriteScale;
			m_listSpriteRenderer.Add (sr);
		}
	}
}
