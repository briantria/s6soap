/*
 * Brian Tria
 * 08/31/2015
 * 
 */

using UnityEngine;
using System.Collections;

public class LevelPatternManager : MonoBehaviour 
{
	private int   m_iColumn = 10;
	private float m_fWidth = 0;
	public  float Width {get{return m_fWidth;}}

	
	//	[SerializeField] private Transform m_groundTiles;
	//	private List<SpriteRenderer> m_listGroundTileSprites = new List<SpriteRenderer> ();
	
	//		foreach (Transform groundTile in m_groundTiles)
	//		{
	//			m_listGroundTileSprites.Add (groundTile.GetComponent<SpriteRenderer>());
	//		}
	//
	//		float tileWidth = m_listGroundTileSprites[0].bounds.size.x * m_listGroundTileSprites[0].transform.localScale.x;
	//		for (int idx = 0; idx < m_listGroundTileSprites.Count; ++idx)
	//		{
	//			m_listGroundTileSprites[idx].transform.position = new Vector3 (-5.0f + (tileWidth * idx), -2.0f, 0.0f);
	//		}


	private GameObject CreateLevelPatternElement (string p_strName)
	{
		GameObject obj = new GameObject (p_strName);

		Transform tObj = obj.transform;
		tObj.SetParent (this.transform);
		tObj.position = Vector3.zero;
		tObj.localScale = Vector3.one;

		return obj;
	}

	public void Setup ()
	{
		MainPlatform mainPlatform = CreateLevelPatternElement("MainPlatform").AddComponent<MainPlatform> ();
		RectPlatform rectPlatform = CreateLevelPatternElement("RectPlatform").AddComponent<RectPlatform> ();
		RectObstacle rectObstacle = CreateLevelPatternElement("RectObstacle").AddComponent<RectObstacle> ();
		TrglObstacle trglObstacle = CreateLevelPatternElement("TrglObstacle").AddComponent<TrglObstacle> ();

		//m_fWidth = m_iColumn * 
	}
}
