/*
 * Brian Tria
 * 09/03/2015
 * 
 */

using UnityEngine;
using System.Collections;

public class MainObjectGroundGlow : MonoBehaviour 
{
	private SpriteRenderer m_spriteRenderer;
	private Transform m_transform;
	private float m_fGlowFadeOutStartDelay;
	private bool m_bBeganFadingOut;

	protected void Awake ()
	{
		m_spriteRenderer = this.GetComponent<SpriteRenderer> ();
		m_transform = this.transform;
		m_fGlowFadeOutStartDelay = 0.0f;
		m_bBeganFadingOut = false;
	}

	protected void Update ()
	{
		if (!m_bBeganFadingOut) { return; }

		Vector3 pos    = m_transform.position;
				pos.x -= GamePlayManager.LEVEL_SPEED * GamePlayManager.Instance.SpeedMultiplier * Time.deltaTime;

		m_transform.position = pos;
	}
	
	private IEnumerator GlowFadeOut ()
	{
		yield return new WaitForSeconds (m_fGlowFadeOutStartDelay);
		float fAlpha = 1.0f;
		m_bBeganFadingOut = true;
		
		while (true)
		{
			yield return new WaitForSeconds (0.01f);
			fAlpha -= 0.05f;
			fAlpha = Mathf.Max (fAlpha, 0.0f);
			m_spriteRenderer.color = new Color (1.0f, 1.0f, 1.0f, fAlpha);
			
			if (fAlpha <= 0.0f)
			{
				Vector3 pos   = m_transform.position;
						pos.x = -3.5f;

				m_transform.position = pos;
				m_bBeganFadingOut = false;
				StopCoroutine ("GlowFadeOut");

				break;
			}
		}
	}

	public void Activate (float p_fadeoutDelay)
	{
		m_spriteRenderer.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		m_fGlowFadeOutStartDelay = p_fadeoutDelay;
		StopCoroutine ("GlowFadeOut");
		StartCoroutine ("GlowFadeOut");
	}
}
