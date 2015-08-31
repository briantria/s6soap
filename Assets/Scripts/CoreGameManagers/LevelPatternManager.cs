/*
 * Brian Tria
 * 08/31/2015
 * 
 */

using UnityEngine;
using System.Collections;

public class LevelPatternManager : MonoBehaviour 
{
	public static readonly int MAX_COLUMN = 10;

	private float m_fWidth = 0;
	public  float Width {get{return m_fWidth;}}

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
//		RectPlatform rectPlatform = CreateLevelPatternElement("RectPlatform").AddComponent<RectPlatform> ();
//		RectObstacle rectObstacle = CreateLevelPatternElement("RectObstacle").AddComponent<RectObstacle> ();
//		TrglObstacle trglObstacle = CreateLevelPatternElement("TrglObstacle").AddComponent<TrglObstacle> ();

		mainPlatform.Setup ();

		m_fWidth = MAX_COLUMN * mainPlatform.TileWidth;
	}
}
