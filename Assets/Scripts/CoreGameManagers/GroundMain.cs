/*
 * Brian Tria
 * 08/31/2015
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundMain : LevelElementManager 
{
	private const string PREFAB_SOURCE_PATH = "Prefabs/GroundMainTile";
	private const string GROUND_EDGE_SPRITE_PATH = "Images/Ground/ground_edge";
	private const string GROUND_MAIN_SPRITE_PATH = "Images/Ground/ground_main";

	/*
		0 -> ravine
		1 -> basic ground
		2 -> right edge
		3 -> left edge
	 */

	private void ApplyMapCode (SpriteRenderer p_sr, int p_idx)
	{
		Vector3 spriteScale = Vector3.one;
		p_sr.gameObject.SetActive (false);

		if (MapGenerator.Instance.GroundCode[p_idx] > 1)
		{
			p_sr.tag = MapGenerator.TAG_PLATFORM;
			p_sr.gameObject.SetActive (true);
			p_sr.sprite = Resources.Load (GROUND_EDGE_SPRITE_PATH, typeof(Sprite)) as Sprite;
			spriteScale.x = ((MapGenerator.Instance.GroundCode[p_idx] * 2) - 5);
		}
		else if (MapGenerator.Instance.GroundCode[p_idx] > 0)
		{
			p_sr.tag = MapGenerator.TAG_MAINPLATFORM;
			p_sr.gameObject.SetActive (true);
			p_sr.sprite = Resources.Load (GROUND_MAIN_SPRITE_PATH, typeof(Sprite)) as Sprite;
		}
		else
		{
			return;
		}

		spriteScale.x *= (LevelPatternManager.COLUMN_WIDTH / p_sr.sprite.bounds.size.x);
		p_sr.transform.localScale = spriteScale;
	}

	public void Setup ()
	{
		for (int idx = 0; idx < LevelPatternManager.MAX_COLUMN; ++idx)
		{
			GameObject obj = AddElement(PREFAB_SOURCE_PATH, idx);
			SpriteRenderer sr = obj.GetComponent<SpriteRenderer> ();
			ApplyMapCode (sr, idx);
			m_listElement.Add (obj);
		}
	}

	public void GenerateNextPattern ()
	{
		for (int idx = 0; idx < LevelPatternManager.MAX_COLUMN; ++idx)
		{
			ApplyMapCode (m_listElement[idx].GetComponent<SpriteRenderer>(), idx);
		}
	}
}
