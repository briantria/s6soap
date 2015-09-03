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
        m_objContainer.SetActive (false);
    }
}
