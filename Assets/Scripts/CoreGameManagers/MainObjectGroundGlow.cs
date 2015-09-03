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
	private float m_fGlowFadeOutStartDelay;

	protected void Awake ()
	{
		m_spriteRenderer = this.GetComponent<SpriteRenderer> ();
		m_fGlowFadeOutStartDelay = 0.0f;
	}
	
	private IEnumerator GlowFadeOut ()
	{
		yield return new WaitForSeconds (m_fGlowFadeOutStartDelay);
		m_spriteRenderer.color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
	}

	public void Activate (float p_fadeoutDelay)
	{
		m_spriteRenderer.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		m_fGlowFadeOutStartDelay = p_fadeoutDelay;
		StopCoroutine ("GlowFadeOut");
		StartCoroutine ("GlowFadeOut");
	}
}
