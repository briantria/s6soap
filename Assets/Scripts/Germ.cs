/*
 * Brian Tria
 * 09/04/2015
 * 
 */

using UnityEngine;
using System.Collections;

public class Germ : MonoBehaviour, ICollectible
{
    [SerializeField] private GameObject m_objContainer;

    public void Collect ()
    {
        // TODO: destroy animation
        m_objContainer.SetActive (false);
        GameHudManager.Instance.UpdateScore (1);
    }
}
