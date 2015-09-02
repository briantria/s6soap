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
	
		for (int idx = 0; idx < LevelPatternManager.MAX_COLUMN; ++idx)
		{
			GameObject obj = AddElement(PREFAB_SOURCE_PATH, idx);
			float randValue = Random.value;
			obj.SetActive (randValue < 0.3f);
			m_listElement.Add (obj);
		}
	}

	public void GenerateNextPattern ()
	{
		foreach (GameObject obj in m_listElement)
		{
			obj.transform.GetChild(0).transform.localPosition = Vector3.zero;
		}
	}
}
