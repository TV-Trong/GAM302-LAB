using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnlineChat : NetworkBehaviour
{
    public TMP_InputField chatInput;
    public Button sendButton;
    public GameObject contentObject;
    public GameObject textContentPrefab;

    ChatManager chatManager => GetComponent<ChatManager>();

    public void SendMessage()
    {
        string message = chatInput?.text;
        if (!string.IsNullOrEmpty(message))
        {
            chatManager.SendChatMessage(message);
            chatInput.text = null;
        }
    }
}
