﻿/*
 * Brian Tria
 * 09/01/2015
 * 
 */

using UnityEngine;
using System.Collections;

public class MainObject : MonoBehaviour
{
	private Rigidbody2D m_rigidbody;
	private bool        m_bDidPause;
	private Vector2     m_v2PrevVelocity;
	private float       m_fPrevAngularVelocity;
	
	protected void Awake ()
	{
		m_rigidbody = this.GetComponent<Rigidbody2D> ();
		m_bDidPause = false;
	}
	
	protected void OnCollisionEnter2D (Collision2D p_collision2D)
	{
		Debug.Log("collision: " + p_collision2D.gameObject.name);
		m_rigidbody.velocity = Vector2.zero;
		m_rigidbody.angularVelocity = 0.0f;
		m_rigidbody.AddForce (Vector2.one * 7, ForceMode2D.Impulse);
		m_rigidbody.AddTorque (-0.4f, ForceMode2D.Impulse);
	}
	
	protected void Update ()
	{
		if (GameStateManager.Instance.IsPaused)
		{
			if (m_bDidPause) { return; }
			
			m_v2PrevVelocity = m_rigidbody.velocity;
			m_fPrevAngularVelocity = m_rigidbody.angularVelocity;
			
			m_bDidPause = true;
			m_rigidbody.isKinematic = true;
		}
		else // resume
		{
			if (!m_bDidPause) { return; }
			
			m_bDidPause = false;
			m_rigidbody.isKinematic = false;
			
			m_rigidbody.velocity = m_v2PrevVelocity;
			m_rigidbody.angularVelocity = m_fPrevAngularVelocity;
		}
	}
}
