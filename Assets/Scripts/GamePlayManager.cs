/*
 * Brian Tria
 * 08/27/2015
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GamePlayManager : MonoBehaviour 
{
	[SerializeField] private Transform m_groundTiles;
	private List<SpriteRenderer> m_listGroundTileSprites = new List<SpriteRenderer> ();

	protected void Awake ()
	{
		foreach (Transform groundTile in m_groundTiles)
		{
			m_listGroundTileSprites.Add (groundTile.GetComponent<SpriteRenderer>());
		}

		float tileWidth = m_listGroundTileSprites[0].bounds.size.x * m_listGroundTileSprites[0].transform.localScale.x;
		for (int idx = 0; idx < m_listGroundTileSprites.Count; ++idx)
		{
			m_listGroundTileSprites[idx].transform.position = new Vector3 (-5.0f + (tileWidth * idx), -2.0f, 0.0f);
		}
	}
}
