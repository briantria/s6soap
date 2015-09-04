/*
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

	public static readonly string TAG_OBSTACLE     = "Obstacle";
	public static readonly string TAG_MAINPLATFORM = "MainPlatform";
	public static readonly string TAG_PLATFORM     = "Platform";
    public static readonly string TAG_DEATH_AREA    = "DeathArea";
    public static readonly string TAG_RESET_AREA    = "ResetArea";
	public static readonly string TAG_COLLECTIBLE  = "Collectible";

	#region properties
	private List<int> m_listGroundCode = new List<int> ();
	public List<int> GroundCode {get{return m_listGroundCode;}}
	
	private List<int> m_listDraggableGroundCode = new List<int> ();
	public List<int> DraggableGroundCode {get{return m_listDraggableGroundCode;}}
	
	private List<int> m_listGroundedObjectCode = new List<int> ();
	/// <summary>
	/// The list of object-codes that determines which objects should be visible on the main platform.
	/// </summary>
	public List<int> GroundedObjectCode {get{return m_listGroundedObjectCode;}}

//	private List<int> m_listRectObstacleCode = new List<int> ();
//	public List<int> RectObstacleCode {get{return m_listRectObstacleCode;}}
	#endregion

	private float m_fObstacleRarity;
	private int m_iPrevGroundCode;
	private int m_iPrevDraggableGroundCode;
	private int m_seed;
	private System.Random m_random;

	protected void Awake ()
	{
		m_instance = this;
		m_seed = "angelyka".GetHashCode();
		Reset ();
	}

	public void Reset ()
	{
		m_random = new System.Random (m_seed);
		m_iPrevGroundCode = 1;
		m_iPrevDraggableGroundCode = -1;
		m_fObstacleRarity = 0.15f;
	}

	public void GenerateRandomMap ()
	{
		// seed here. use System.Random
		// ref: https://www.reddit.com/r/Unity3D/comments/2w6v69/is_randomseed_specific_to_the_class_it_is/
		
		/*
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

		if (m_fObstacleRarity < 0.8f)
		{
			m_fObstacleRarity += 0.008f;
		}

		//Debug.Log ("Obstacle rarity: " + m_fObstacleRarity);

		CreateGround ();
		AddDraggableGround ();
		AddGroundedObjects ();
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

		for (int idx = 0; idx < m_listGroundCode.Count; ++idx)
		{
			// avoid ground
			if (m_listGroundCode[idx] > 0)
			{
				m_iPrevDraggableGroundCode = -1;
			}
			// avoid consecutive draggable ground
			else if ((idx > 0 && m_listDraggableGroundCode[idx-1] > 0) ||
			         (idx == 0 && m_iPrevDraggableGroundCode > 0))
			{
				m_listDraggableGroundCode.Add (-1);
				continue;
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

	private void AddGroundedObjects ()
	{
		/*
			-1 : none
			 0 : triangle obstacle
			 1 : booster / spring
		 */

		m_listGroundedObjectCode.Clear ();

		foreach (int groundIdx in m_listGroundCode)
		{
			int iCode = -1;
			
			// avoid holes and edges
			if (groundIdx == 1)
			{
				iCode += (m_random.NextDouble () < m_fObstacleRarity) ? 1 : 0;
			}

			m_listGroundedObjectCode.Add (iCode);
		}
	}
}
