using UnityEngine;
using System.Collections;

public class MainPlatform : MonoBehaviour 
{
	private const string TILE_SOURCE_PATH = "Prefabs/MainPlatformTile";

	private float m_fTileWidth;
	public  float TileWidth {get{return m_fTileWidth;}}

	private GameObject AddTile (int p_idx)
	{
		GameObject objTile = Instantiate(Resources.Load (TILE_SOURCE_PATH)) as GameObject;
		objTile.name = "Tile" + p_idx;
		
		Transform tTile = objTile.transform;
		tTile.SetParent (this.transform);
		tTile.localScale = Vector3.one;
		tTile.localPosition = new Vector3 (p_idx * m_fTileWidth, 0.0f, 0.0f);
		
		return objTile;
	}

	public void Setup ()
	{
		m_fTileWidth = 0;
		SpriteRenderer spriteRenderer = AddTile(0).GetComponent<SpriteRenderer> ();
		m_fTileWidth = spriteRenderer.bounds.size.x * spriteRenderer.transform.localScale.x;
		
		for (int idx = 1; idx < LevelPatternManager.MAX_COLUMN; ++idx)
		{
			AddTile (idx);
		}
	}
}
