/*
 * Brian Tria
 * 09/01/2015
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelElementManager : MonoBehaviour 
{
	protected List<GameObject> m_listElement = new List<GameObject> ();
	
	protected GameObject AddElement (string p_prefabPath, int p_idx, string name = "LevelElement")
	{
		GameObject objElement = Instantiate (Resources.Load(p_prefabPath)) as GameObject;
		objElement.name = name + p_idx;
		
		Transform tElement = objElement.transform;
		tElement.SetParent (this.transform);
		tElement.localScale = Vector3.one;
		tElement.localPosition = new Vector3 (p_idx * LevelPatternManager.COLUMN_WIDTH, 0.0f, 0.0f);
		
		return objElement;
	}
}
