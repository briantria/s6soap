/*
 * Brian Tria
 * 09/01/2015
 * 
 */

using UnityEngine;
using System.Collections;

public class MainObject : MonoBehaviour
{
	[SerializeField] private MainObjectGroundGlow m_groundGlow; 
	[SerializeField] private float m_torque;
	[SerializeField] private float m_jumpVelocity;
	[SerializeField] private float m_jumpDelay;

	private bool    m_bDidPause;
	private Vector2 m_v2PrevVelocity;
	private float   m_fPrevAngularVelocity;
	
	private Rigidbody2D m_rigidbody;
	public  Rigidbody2D RBody2D {get{return m_rigidbody;}}

	protected void Awake ()
	{
		m_rigidbody = this.GetComponent<Rigidbody2D> ();
		m_bDidPause = false;
	}

	protected void OnTriggerEnter2D (Collider2D p_collider2D)
	{
		if (p_collider2D.CompareTag(MapGenerator.TAG_COLLECTIBLE))
		{

		}
	}
	
	protected void OnCollisionEnter2D (Collision2D p_collision2D)
	{
//		if (p_collision2D.gameObject.CompareTag(MapGenerator.TAG_OBSTACLE))
//		{
//			m_rigidbody.Sleep ();
//			UIStateManager.Instance.ChangeUIState (UIState.OnResultScreen);
//			GameStateManager.Instance.ChangeGameState (GameState.GameOver);
//			return;
//		}

		if (p_collision2D.gameObject.CompareTag(MapGenerator.TAG_DEATHAREA))
		{
			m_rigidbody.Sleep ();
			UIStateManager.Instance.ChangeUIState (UIState.OnResultScreen);
			GameStateManager.Instance.ChangeGameState (GameState.GameOver);
			return;
		}

		Vector3 relativePosition = p_collision2D.transform.InverseTransformPoint (p_collision2D.contacts[0].point);

		if (relativePosition.y <= 0)
		{
			// Debug.Log ("main object is not above");
			return;
		}

		m_rigidbody.velocity = Vector2.zero;
		CancelInvoke ("DelayedJump");
		Invoke ("DelayedJump", m_jumpDelay);
		
		if (p_collision2D.gameObject.CompareTag(MapGenerator.TAG_MAINPLATFORM))
		{
			m_groundGlow.Activate (m_jumpDelay);
		}
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

	private void DelayedJump ()
	{
		float angularVel = Mathf.Clamp (m_torque - m_rigidbody.angularVelocity, m_torque, m_torque * 0.8f);
		m_rigidbody.AddForce (Vector2.one * m_jumpVelocity, ForceMode2D.Impulse);
		m_rigidbody.AddTorque (angularVel, ForceMode2D.Impulse);
	}

	public void Reset ()
	{
		Vector3 pos   = this.transform.position;
				pos.y = 2;
		this.transform.position = pos;
		this.transform.localEulerAngles = Vector3.zero;

		m_rigidbody.Sleep ();
		m_rigidbody.velocity = Vector2.zero;
		m_rigidbody.angularVelocity = 0.0f;
	}
}
