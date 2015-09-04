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
    [SerializeField] private GameObject m_objPlayButton; 
    [SerializeField] private PolygonCollider2D m_collider;
	[SerializeField] private float m_torque;
	[SerializeField] private float m_jumpVelocity;
	[SerializeField] private float m_jumpDelay;
	
	[SerializeField] AudioClip m_audioDeath;
	[SerializeField] AudioClip m_audioJump;
	[SerializeField] AudioClip m_audioHit;
	[SerializeField] AudioClip m_audioPlayBtn;
	[SerializeField] AudioClip m_audioGameOver;

//	private bool    m_bDidPause;
	private Vector2 m_v2PrevVelocity;
	private float   m_fPrevAngularVelocity;

	private AudioSource m_audioSource;
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
		m_audioSource = this.GetComponent<AudioSource> ();
//		m_bDidPause = false;
	}

	protected void OnTriggerEnter2D (Collider2D p_collider2D)
	{
		if (p_collider2D.CompareTag(MapGenerator.TAG_COLLECTIBLE))
		{
            p_collider2D.gameObject.GetComponent<ICollectible>().Collect ();

			if (m_audioSource.isPlaying)
			{
				m_audioSource.Stop ();
			}
			
			m_audioSource.clip = m_audioHit;
			m_audioSource.Play ();
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

			if (m_audioSource.isPlaying)
			{
				m_audioSource.Stop ();
			}

			m_audioSource.clip = m_audioGameOver;
			m_audioSource.Play ();
			
			GameStateManager.Instance.ChangeGameState (GameState.GameOver);
			return;
		}

		Vector3 relativePosition = p_collision2D.transform.InverseTransformPoint (p_collision2D.contacts[0].point);

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
		if (m_audioSource.isPlaying)
		{
			m_audioSource.Stop ();
		}

//		m_audioSource.clip = m_audioJump;
		m_audioSource.Play ();
//		float angularVel = Mathf.Clamp (m_torque - m_rigidbody.angularVelocity, m_torque, m_torque * 0.8f);
		m_rigidbody.AddForce (Vector2.one * m_jumpVelocity, ForceMode2D.Impulse);
		//m_rigidbody.AddTorque (angularVel, ForceMode2D.Impulse);
	}

	public void OnClickPlay ()
	{
		GameStateManager.Instance.ChangeGameState (GameState.Restart);

		if (m_audioSource.isPlaying)
		{
			m_audioSource.Stop ();
		}
		
		m_audioSource.clip = m_audioPlayBtn;
		m_audioSource.Play ();
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
