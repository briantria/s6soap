/*
 * Brian Tria
 * 09/01/2015
 * 
 */

using UnityEngine;
using System.Collections;

public class MainObject : MonoBehaviour
{
	[SerializeField] private SpriteRenderer m_groundGlow; 

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
		if (p_collision2D.gameObject.CompareTag(MapGenerator.TAG_OBSTACLE))
		{
			m_rigidbody.isKinematic = true;
			GameStateManager.Instance.ChangeGameState (GameState.GameOver);
			return;
		}
	
		m_rigidbody.velocity = Vector2.zero;
		m_rigidbody.angularVelocity = 0.0f;
		m_rigidbody.AddForce (Vector2.one * 7, ForceMode2D.Impulse);
		m_rigidbody.AddTorque (-0.4f, ForceMode2D.Impulse);

		if (p_collision2D.gameObject.CompareTag(MapGenerator.TAG_MAINPLATFORM))
		{
			m_groundGlow.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
			StopCoroutine ("GlowFadeOut");
			StartCoroutine ("GlowFadeOut");
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

	private IEnumerator GlowFadeOut ()
	{
		float fAlpha = 1.0f;

		while (true)
		{
			yield return new WaitForSeconds (0.01f);
			fAlpha -= 0.05f;
			fAlpha = Mathf.Max (fAlpha, 0.0f);
			m_groundGlow.color = new Color (1.0f, 1.0f, 1.0f, fAlpha);

			if (fAlpha <= 0.0f)
			{
				StopCoroutine ("GlowFadeOut");
				break;
			}
		}
	}
}
