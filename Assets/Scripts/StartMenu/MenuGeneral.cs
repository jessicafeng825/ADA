using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGeneral : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Animator>().Play("FadeIn");
    }
}
