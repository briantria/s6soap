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
	public  static LoadScreenManager Instance { get { return m_instance; }}

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
		UIStateManager.Instance.ChangeUIState (UIState.OnTitleScreen);
		Debug.Log("TotalInitObjectLoadCount: " + TotalInitObjectLoadCount);
		StartCoroutine (InitLoading());
	}

	private IEnumerator InitLoading ()
	{
		yield return new WaitForSeconds (TotalInitObjectLoadCount * 0.03f);
		m_objMainBG.SetActive (false);
		m_txtLoading.gameObject.SetActive (false);
	}
}
