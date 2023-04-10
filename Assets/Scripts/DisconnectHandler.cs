using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class DisconnectHandler : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void startKeepAlive();

    private void Start()
    {
        startKeepAlive();
    }
}
