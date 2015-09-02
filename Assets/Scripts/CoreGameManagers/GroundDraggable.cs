/*
 * Brian Tria
 * 08/31/2015
 * 
 */

using UnityEngine;
using System.Collections;

public class GroundDraggable : LevelElementManager 
{
	private const string PREFAB_SOURCE_PATH = "Prefabs/GroundDraggableTile";
	private readonly Vector2 m_v2ActualSpriteSize = new Vector2 (58.0f, 58.0f) * Constants.PPU_MULTIPLIER;

	private void ApplyMapCode (GameObject p_obj, int p_idx)
	{
		int posMultiplierY = MapGenerator.Instance.DraggableGroundCode[p_idx];
		p_obj.SetActive (false);

		if (posMultiplierY < 0) { return; }

		Vector3 objPos = p_obj.transform.localPosition;
		objPos.y = m_v2ActualSpriteSize.y * posMultiplierY;
		
		p_obj.SetActive (true);
		p_obj.transform.localPosition = objPos;
	}

	public void Setup ()
	{
		for (int idx = 0; idx < LevelPatternManager.MAX_COLUMN; ++idx)
		{
			GameObject obj = AddElement(PREFAB_SOURCE_PATH, idx);
			ApplyMapCode (obj, idx);
			m_listElement.Add (obj);
		}
	}

	public void GenerateNextPattern ()
	{
		for (int idx = m_listElement.Count - 1; idx >= 0; --idx)
		{
			ApplyMapCode (m_listElement[idx], idx);
		}
	}
}
