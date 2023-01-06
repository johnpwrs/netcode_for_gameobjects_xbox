using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkHome : MonoBehaviour
{
    private NetworkManager netManager;
    private bool isInMatch = false;

    private InputActionMap actionMap;

    public InputActionAsset inputActionAsset;

    // Start is called before the first frame update
    void Start()
    {
        netManager = GetComponent<NetworkManager>();
        actionMap = inputActionAsset.FindActionMap("Player");
        actionMap.Enable();
    }

    private void OnGUI()
    {
        if (!isInMatch)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host press 1 or a"))
            {
                startHost();
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Join press 2 or b"))
            {
                startClient();
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!isInMatch)
        {
            if (actionMap.FindAction("Action1").WasPerformedThisFrame())
            {
                startHost();
            }
            else if (actionMap.FindAction("Action2").WasPerformedThisFrame())
            {
                startClient();
            }
        }
    }

    private void startClient()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
                "192.168.1.119",  // The IP address is a string
                (ushort)7777// The port number is an unsigned short
            );
        netManager.StartClient();
        isInMatch = true;
    }

    private void startHost()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
              "0.0.0.0",  // The IP address is a string
              (ushort)7777// The port number is an unsigned short
          );
        netManager.StartHost();
        isInMatch = true;
    }


}
