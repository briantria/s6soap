﻿/*
 * Brian Tria
 * 09/01/2015
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainObject : MonoBehaviour
{
	[SerializeField] private MainObjectGroundGlow m_groundGlow; 
    [SerializeField] private GameObject m_objPlayButton; 
    [SerializeField] private PolygonCollider2D m_collider;
	[SerializeField] private float m_torque;
	[SerializeField] private float m_jumpVelocity;
	[SerializeField] private float m_jumpDelay;
	
	[SerializeField] private AudioClip m_audioBoost;
	[SerializeField] private AudioClip m_audioJump;
	[SerializeField] private AudioClip m_audioHit;
	[SerializeField] private AudioClip m_audioPlayBtn;
	[SerializeField] private AudioClip m_audioGameOver;

	[SerializeField] private List<AudioSource> m_sfxSources = new List<AudioSource> ();

//	private bool    m_bDidPause;
	private Vector2 m_v2PrevVelocity;
	private float   m_fPrevAngularVelocity;

	private Rigidbody2D m_rigidbody;
	public  Rigidbody2D RBody2D {get{return m_rigidbody;}}
    public bool ResetReady { get; set; }

    protected void OnEnable ()
    {
        GameStateManager.changeGameState += ChangeGameState;
    }
    
    protected void OnDisable ()
    {
        GameStateManager.changeGameState -= ChangeGameState;
    }

	protected void Awake ()
	{
		m_rigidbody = this.GetComponent<Rigidbody2D> ();
//		m_bDidPause = false;
	}

	protected void OnTriggerEnter2D (Collider2D p_collider2D)
	{
		if (p_collider2D.CompareTag(MapGenerator.TAG_COLLECTIBLE))
		{
            p_collider2D.gameObject.GetComponent<ICollectible>().Collect ();

			if (m_sfxSources[1].isPlaying)
			{
				m_sfxSources[1].Stop ();
			}
			
			m_sfxSources[1].clip = m_audioHit;
			m_sfxSources[1].Play ();
		}
        
        if (p_collider2D.CompareTag(MapGenerator.TAG_RESET_AREA))
        {
            m_collider.isTrigger = false;
            ResetReady = true;
            m_rigidbody.Sleep ();
        }
	}
	
	protected void OnCollisionEnter2D (Collision2D p_collision2D)
	{
		if (p_collision2D.gameObject.CompareTag(MapGenerator.TAG_OBSTACLE)  ||
            p_collision2D.gameObject.CompareTag(MapGenerator.TAG_DEATH_AREA) )
		{
            if (GamePlayManager.Instance.SpeedMultiplier < 1.0f)
            {
                GamePlayManager.Instance.SpeedMultiplier = 1.0f;
            }
            
            m_collider.isTrigger = true;
            m_rigidbody.velocity = Vector2.zero;
			m_rigidbody.AddForce (Vector2.one * m_jumpVelocity, ForceMode2D.Impulse);

			if (m_sfxSources[1].isPlaying)
			{
				m_sfxSources[1].Stop ();
			}

			m_sfxSources[1].clip = m_audioGameOver;
			m_sfxSources[1].Play ();
			
			GameStateManager.Instance.ChangeGameState (GameState.GameOver);
			return;
		}

		Vector3 relativePosition = p_collision2D.transform.InverseTransformPoint (p_collision2D.contacts[0].point);
		m_sfxSources[0].clip = m_audioJump;

		if (p_collision2D.gameObject.CompareTag(MapGenerator.TAG_DRAGGABLE_PLATFORM))
		{
			m_sfxSources[0].clip = m_audioBoost;
		}

		if (relativePosition.y <= 0)
		{
			// Debug.Log ("main object is not above");
            GamePlayManager.Instance.SpeedMultiplier = 0.0f;
			return;
		}
        else if (GamePlayManager.Instance.SpeedMultiplier < 1.0f)
        {
            GamePlayManager.Instance.SpeedMultiplier = 1.0f;
        }

		m_rigidbody.velocity = Vector2.zero;
		CancelInvoke ("Jump");
		Invoke ("Jump", m_jumpDelay);
		
		if (p_collision2D.gameObject.CompareTag(MapGenerator.TAG_MAINPLATFORM))
		{
			m_groundGlow.Activate (m_jumpDelay);
		}
	}
	
//	protected void Update ()
//	{
//		if (GameStateManager.Instance.IsPaused)
//		{
//			if (m_bDidPause) { return; }
//			
//			m_v2PrevVelocity = m_rigidbody.velocity;
//			m_fPrevAngularVelocity = m_rigidbody.angularVelocity;
//			
//			m_bDidPause = true;
//			m_rigidbody.isKinematic = true;
//		}
//		else // resume
//		{
//			if (!m_bDidPause) { return; }
//			
//			m_bDidPause = false;
//			m_rigidbody.isKinematic = false;
//			
//			m_rigidbody.velocity = m_v2PrevVelocity;
//			m_rigidbody.angularVelocity = m_fPrevAngularVelocity;
//		}
//	}

    private void ChangeGameState (GameState p_gameState)
    {
        switch (p_gameState){
        case GameState.Idle:
        {
            Reset ();
            ResetReady = false;
            m_objPlayButton.SetActive (true);
            break;
        }}
    }

	private void Jump ()
	{
		if (m_sfxSources[0].isPlaying)
		{
			m_sfxSources[0].Stop ();
		}

		m_sfxSources[0].Play ();
		m_rigidbody.AddForce (Vector2.one * m_jumpVelocity, ForceMode2D.Impulse);

		StopCoroutine ("Turn60");
		StartCoroutine ("Turn60");
	}

	private IEnumerator Turn60 ()
	{
		Vector3 v3Euler = this.transform.localEulerAngles;

		float rotateSpeed = 120.0f;
		float prevEulerZ  = v3Euler.z;
		float nextEulerZ  = prevEulerZ - rotateSpeed;
		//float deltaAngle  = 0;

		while (true)
		{
			//deltaAngle += 10.0f;
			//v3Euler.z = Mathf.Lerp (prevEulerZ, nextEulerZ, deltaAngle / rotateSpeed);
			v3Euler.z -= 5.0f;
			this.transform.localEulerAngles = v3Euler;

			if (v3Euler.z - nextEulerZ < 0.2f)
			{
				v3Euler.z = nextEulerZ;
				break;
			}

			yield return new WaitForSeconds (0.008f);
		}

		this.transform.localEulerAngles = v3Euler;
	}

	public void OnClickPlay ()
	{
		GameStateManager.Instance.ChangeGameState (GameState.Restart);

		if (m_sfxSources[1].isPlaying)
		{
			m_sfxSources[1].Stop ();
		}
		
		m_sfxSources[1].clip = m_audioPlayBtn;
		m_sfxSources[1].Play ();
	}

	public void Reset ()
	{
//		Vector3 pos   = this.transform.position;
//				pos.y = 2;
		this.transform.position = Vector3.zero;
		this.transform.localEulerAngles = Vector3.zero;

        m_objPlayButton.SetActive (false);
		m_rigidbody.Sleep ();
		m_rigidbody.velocity = Vector2.zero;
		m_rigidbody.angularVelocity = 0.0f;
	}
}
