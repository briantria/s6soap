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
			GameObject obj = AddElement (PREFAB_SOURCE_PATH, idx);
			obj.SetActive (MapGenerator.Instance.GroundedObjectCode[idx] == 0);
			m_listElement.Add (obj);
		}
	}

	public void GenerateNextPattern ()
	{
		int iDraggableInstanceID = this.GetInstanceID ();

		if (Draggable.CurrentDragObject != null)
		{
			iDraggableInstanceID = Draggable.CurrentDragObject.transform.GetInstanceID ();
		}

		for (int idx = 0; idx < LevelPatternManager.MAX_COLUMN; ++idx)
		{
			Transform draggable = m_listElement[idx].transform.GetChild(0).transform;

			if (draggable.GetInstanceID() == iDraggableInstanceID)
			{
				Draggable.CurrentDragObject = null;
			}

			draggable.localPosition = Vector3.zero;
			m_listElement[idx].SetActive (MapGenerator.Instance.GroundedObjectCode[idx] == 0);
		}
	}
}
