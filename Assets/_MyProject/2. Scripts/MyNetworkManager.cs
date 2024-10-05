using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        Debug.Log("A client has connected: " + conn.address);        
    }
    public override void OnClientConnect()
    {
        Debug.Log("Connected to server:");        
    }    

    public override void OnClientDisconnect()
    {
        Debug.Log("Disconnected from server: ");
    }
}
