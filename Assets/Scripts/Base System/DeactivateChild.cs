using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateChild : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> disableCloseSChild = new List<GameObject>();

    [SerializeField]
    private List<GameObject> enableOpenSChild = new List<GameObject>();

    public void CloseThisMenu() 
    {
        foreach (GameObject child in disableCloseSChild)
        {
            child.SetActive(false);
        }
        this.gameObject.SetActive(false);
    }

    private void Start() 
    {
        foreach (GameObject child in enableOpenSChild)
        {
            child.SetActive(true);
        }
    }
}
