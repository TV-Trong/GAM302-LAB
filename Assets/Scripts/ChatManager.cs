using ExitGames.Client.Photon;
using Fusion;
using Photon.Chat;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class ChatManager : NetworkBehaviour, IChatClientListener
{
    OnlineChat onlineChat => GetComponent<OnlineChat>();
    ChatClient chatClient;

    public override void Spawned()
    {
        PhotonNetwork.LocalPlayer.NickName = PlayerPrefs.GetString("LocalName");
        PlayerPrefs.DeleteKey("LocalName");

        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues($"User{PhotonNetwork.LocalPlayer.NickName}"));
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void Rpc_ReceivedChatMessage(string sender, string message)
    {
        string formattedMessage = $"{sender}: {message}";
        var chatContent = Instantiate(onlineChat.textContentPrefab, onlineChat.contentObject.transform);
        chatContent.GetComponent<TextMeshProUGUI>().text = formattedMessage;
    }

    public void SendChatMessage(string message)
    {
        string playerName = PhotonNetwork.LocalPlayer.NickName;
        Rpc_ReceivedChatMessage(playerName, message);
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnChatStateChange(ChatState state)
    {
        throw new System.NotImplementedException();
    }

    public void OnConnected()
    {
        //chatClient.Subscribe(new string[] { "General" });
    }

    public void OnDisconnected()
    {
        throw new System.NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        //string msg = string.Empty;
        //for (int i = 0; i < senders.Length; i++)
        //{
        //    msg += $"{senders[i]}: {messages[i]}\n";
        //    onlineChat.
        //}
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnsubscribed(string[] channels)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }
}
