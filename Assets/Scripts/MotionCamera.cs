using UnityEngine;
using System.Collections;

public class MotionCamera : MonoBehaviour 
{
    [SerializeField] private Transform m_objOnFocus;
    
    private const float MIN_ORTHOSIZE        = 2.0f;
    private const float MAX_ORTHOSIZE        = 4.2f;
    private const float DEFAULT_ORTHOSIZE = 3.2f;
    
    private const float FOLLOW_LIMIT_Y = -6.0f;
    private const float DEFAULT_POSX = 3.5f;
    
    private bool m_bFollowMainObject;
    private bool m_bLerpToMainObject;
    private bool m_bLerpToMax;
//    private bool m_bLerpToDefault;

    private Camera m_attachedCamera;
    private Transform m_transform;

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
        m_transform = this.transform;
        m_attachedCamera = this.GetComponent<Camera> ();
    }
    
    protected void Update ()
    {
        Vector3 newPos = m_transform.position;
    
        if (m_bFollowMainObject)
        {
            newPos.y = m_objOnFocus.position.y;
            m_attachedCamera.orthographicSize = Mathf.Lerp (MIN_ORTHOSIZE, DEFAULT_ORTHOSIZE, newPos.y / FOLLOW_LIMIT_Y);
            
            if (newPos.y <= FOLLOW_LIMIT_Y)
            {
                newPos.y = FOLLOW_LIMIT_Y;
                m_attachedCamera.orthographicSize = DEFAULT_ORTHOSIZE;
                m_bFollowMainObject = false;
                m_bLerpToMax = true;
                GameStateManager.Instance.ChangeGameState (GameState.Running);
            }
        }
        
        if (m_bLerpToMax)
        {
            newPos.x += GamePlayManager.LEVEL_SPEED * GamePlayManager.Instance.SpeedMultiplier * 0.1f * Time.deltaTime;
            newPos.y += GamePlayManager.LEVEL_SPEED * GamePlayManager.Instance.SpeedMultiplier * 0.025f * Time.deltaTime;
            m_attachedCamera.orthographicSize = Mathf.Lerp (DEFAULT_ORTHOSIZE, MAX_ORTHOSIZE, newPos.x / DEFAULT_POSX);
            
            if (newPos.x >= DEFAULT_POSX)
            {
                newPos.x = DEFAULT_POSX;
                m_attachedCamera.orthographicSize = MAX_ORTHOSIZE;
                m_bLerpToMax = false;
            }
        }
        
        m_transform.position = newPos;
    }
    
    private void ChangeGameState (GameState p_gameState)
    {
        switch (p_gameState){
        case GameState.Idle:
        {
            m_bFollowMainObject = false;
            m_bLerpToMainObject = false;
//            m_bLerpToDefault = false;
            m_bLerpToMax = false;
//            GameStateManager.Instance.IsPaused = true;
            
            break;
        }
        case GameState.Restart:
        {
            m_bFollowMainObject = true;
            m_bLerpToMainObject = false;
//            m_bLerpToDefault = true;
            
            break;
        }
        case GameState.GameOver:
        {
            m_bLerpToMainObject = true;
            m_bFollowMainObject = false;
//            m_bLerpToDefault = false;
            m_bLerpToMax = false;
            
            break;
        }
        case GameState.Running:
        {
//            m_bFollowMainObject = false;
//            m_bLerpToMainObject = false;
//            m_bLerpToDefault = true;
            
            break;
        }}
    }
}
