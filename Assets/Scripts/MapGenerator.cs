﻿/*
 * Brian Tria
 * 09/01/2015
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour 
{
	private static MapGenerator m_instance = null;
	public  static MapGenerator Instance {get{return m_instance;}}

	public static readonly string TAG_OBSTACLE = "Obstacle";
	public static readonly string TAG_MAINPLATFORM = "MainPlatform";

	#region properties
	private List<int> m_listGroundCode = new List<int> ();
	public List<int> GroundCode {get{return m_listGroundCode;}}
	
	private List<int> m_listDraggableGroundCode = new List<int> ();
	public List<int> DraggableGroundCode {get{return m_listDraggableGroundCode;}}
	
	private List<int> m_listTriangleObstacleCode = new List<int> ();
	public List<int> TriangleObstacleCode {get{return m_listTriangleObstacleCode;}}

	private List<int> m_listRectObstacleCode = new List<int> ();
	public List<int> RectObstacleCode {get{return m_listRectObstacleCode;}}
	#endregion
	
	private int m_iPrevGroundCode;
	private int m_iPrevDraggableGroundCode;
	private int m_seed;
	System.Random m_random;

	protected void Awake ()
	{
		m_instance = this;
		m_seed = "adrimsim".GetHashCode();
		Reset ();
		//m_random = new System.Random (m_seed);
	}

	public void Reset ()
	{
		m_random = new System.Random (m_seed);
		m_iPrevGroundCode = 1;
		m_iPrevDraggableGroundCode = -1;
	}

	public void GenerateRandomMap ()
	{
		// seed here. use System.Random
		// ref: https://www.reddit.com/r/Unity3D/comments/2w6v69/is_randomseed_specific_to_the_class_it_is/
		
		/*
			plan:
			
			generate random map code here. then let other
			element container access the data generated here.
			this way, we can relate the ground data to obstacle data.
			
			the map generator rule:
				- randomize ground (platform tag) or ravine (obstacle tag)
				- if ground, randomize if obstacle will be visible. say a triangle obstacle.
				- if ravine, add a draggable platform.
				- randomize if draggable platform will have another obstacle.
				  (consider previous platform if it already had an obstacle)
		*/

		// TODO: percentage change overtime to increase game difficulty
		CreateGround ();
		AddDraggableGround ();
		AddObstacles ();
	}

	private void CreateGround ()
	{
		/*
			0 -> ravine
			1 -> basic ground
			2 -> right edge
			3 -> left edge
		 */

		m_listGroundCode.Clear ();

		// start with random ground
		for (int idx = 0; idx < LevelPatternManager.MAX_COLUMN; ++idx)
		{
			bool bGround = m_random.NextDouble () > 0.3f;
			m_listGroundCode.Add (bGround ? 1 : 0);
		}
		
		// change ground_0 based on m_iPrevGroundCode
		if (m_iPrevGroundCode > 1)
		{
			m_listGroundCode[0] = m_iPrevGroundCode - 2;
		}
		else
		{
			m_listGroundCode[0] = m_iPrevGroundCode;
		}

		// plug tiny holes & remove 'ilands'
		for (int idx = m_listGroundCode.Count - 2; idx > 0; --idx)
		{
			if (m_listGroundCode[idx-1] == m_listGroundCode[idx+1])
			{
				m_listGroundCode[idx] = m_listGroundCode[idx-1];
			}
		}

		// set edges
		for (int idx = m_listGroundCode.Count - 2; idx > 0; --idx)
		{
			if(m_listGroundCode[idx] == 0)
			{
				m_listGroundCode[idx-1] = (m_listGroundCode[idx-1] > 0) ? 2 : 0;
				m_listGroundCode[idx+1] = (m_listGroundCode[idx+1] > 0) ? 3 : 0;
			}
			else
			{
				m_listGroundCode[idx] = (m_listGroundCode[idx-1] == 0) ? 3 : m_listGroundCode[idx];
				m_listGroundCode[idx] = (m_listGroundCode[idx+1] == 0) ? 2 : m_listGroundCode[idx];
			}
		}

		// set m_iPrevGroundCode with this last ground element
		m_iPrevGroundCode = m_listGroundCode [m_listGroundCode.Count - 1];
	}

	private void AddDraggableGround ()
	{
		m_listDraggableGroundCode.Clear ();

		foreach (int groundIdx in m_listGroundCode)
		{
			if (groundIdx > 0)
			{
				// if main ground is present, dont add draggable ground
				m_iPrevDraggableGroundCode = -1;
			}
			else
			{
				int nextCode = m_random.Next (m_iPrevDraggableGroundCode - 1, m_iPrevDraggableGroundCode + 3);
				nextCode = Mathf.Max (1, nextCode);
				
				m_iPrevDraggableGroundCode = nextCode;
			}

			m_listDraggableGroundCode.Add (m_iPrevDraggableGroundCode);
		}
	}

	private void AddObstacles ()
	{

	}
}
