/*
 * Brian Tria
 * 08/31/2015
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrglObstacle : LevelElementManager 
{
	private const string PREFAB_SOURCE_PATH = "Prefabs/TriangleObstacle";

	public void Setup ()
	{	
		this.transform.localPosition = new Vector3 (0.0f, 0.4f, 0.0f);
	
//		Debug.Log("seed: " + Random.seed);
//		string strRandValues = "";
	
		for (int idx = 0; idx < LevelPatternManager.MAX_COLUMN; ++idx)
		{
			GameObject obj = AddElement(PREFAB_SOURCE_PATH, idx);
			float randValue = Random.value;
//			strRandValues += randValue.ToString("F2") + " ";
			obj.SetActive (randValue < 0.3f);
			m_listSpriteRenderer.Add (obj.GetComponent<SpriteRenderer>());
		}
		
//		Debug.Log(strRandValues);
	}
}
