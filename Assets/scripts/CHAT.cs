using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CHAT : NetworkBehaviour
{

    public InputField inputField;
    public Text textHistory;

    // Update is called once per frame
    void Update()
    {
        if (inputField.text != "" && Input.GetKey(KeyCode.Return))
        {
            //Debug.Log(inputField.text);
            CmdSendMessage(inputField.text);
            inputField.text = "";
        }
    }


    [Command]
    void CmdSendMessage(string text)
    {
        RpcSendMessage(text);
    }
    [ClientRpc]
    void RpcSendMessage(string text)
    {
        textHistory.text = text + "\n" + textHistory.text;
    }
}
