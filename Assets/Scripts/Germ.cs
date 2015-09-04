/*
 * Brian Tria
 * 09/04/2015
 * 
 */

using UnityEngine;
using System.Collections;

public class Germ : MonoBehaviour, ICollectible
{
	[SerializeField] private GameObject m_objSprite;
	[SerializeField] private ParticleSystem m_particleSystem;

//	protected void Start ()
//	{
//		m_objParticle.enableEmission = false;
//	}

	private void StopEmission ()
	{
		if (m_particleSystem.isPlaying)
		{
			m_particleSystem.Stop ();
		}
	}

    public void Collect ()
    {
        // TODO: destroy animation
		m_objSprite.SetActive (false);
		this.GetComponent<PolygonCollider2D>().enabled = false;
		GameHudManager.Instance.UpdateScore (1);

		if (m_particleSystem.isStopped)
		{
			m_particleSystem.Play ();
		}

		CancelInvoke ("StopEmission");
		Invoke ("StopEmission", 0.5f);
    }

	public void Activate (bool p_bool)
	{
		m_objSprite.SetActive (p_bool);
		this.GetComponent<PolygonCollider2D>().enabled = p_bool;
	}
}
