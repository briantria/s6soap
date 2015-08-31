/*
 * Brian Tria
 * 08/27/2015
 * 
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadScreenManager : MonoBehaviour
{
	private static LoadScreenManager m_instance = null;
	public  static LoadScreenManager Instance {get{return m_instance;}}

	[SerializeField] private GameObject m_objMainBG;
	[SerializeField] private Text       m_txtLoading;

	public int TotalInitObjectLoadCount { get; set; }

	protected void Awake ()
	{
		m_instance = this;
		TotalInitObjectLoadCount = 0;
	}

	protected void Start ()
	{
		StartCoroutine (InitLoading());
		StartCoroutine ("Animate");
	}

	private IEnumerator Animate ()
	{
		int iMaxDotCount = 3;
		float fAnimSpeed = 0.3f;
		
		while (true)
		{
			m_txtLoading.text = "Loading";
			yield return new WaitForSeconds (fAnimSpeed);
			
			for (int idx = 0; idx < iMaxDotCount; ++idx)
			{
				m_txtLoading.text += ".";
				yield return new WaitForSeconds (fAnimSpeed);
			}
		}
	}

	private IEnumerator InitLoading ()
	{
		yield return new WaitForSeconds (TotalInitObjectLoadCount * 0.03f);

		UIStateManager.Instance.ActiveScreens = UIState.Reset;
		//UIStateManager.Instance.ChangeUIState (UIState.OnTitleScreen);
		//GameStateManager.Instance.ChangeGameState (GameState.Inactive);
		UIStateManager.Instance.ChangeUIState (UIState.OnGameScreen);
		GameStateManager.Instance.ChangeGameState (GameState.Start);

		m_objMainBG.SetActive (false);
		m_txtLoading.gameObject.SetActive (false);
		StopCoroutine ("Animate");
	}
}
