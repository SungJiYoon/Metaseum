using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using ExitGames.Client.Photon;
using TMPro;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    public GameObject textPrefab; // �ؽ�Ʈ ������
    public ScrollRect scrollRect; // ��ũ�� ���� ScrollRect ������Ʈ
    public RectTransform content; // ��ũ�� ���� Content ������Ʈ
    public TMP_InputField chatInputField; // ä�� �Է�â

    private ChatClient chatClient;
    private string chatAppId = "4d4974fd-a026-41c7-b892-892f860f3007";
    private string chatAppVersion = "1.0";
    private string userName = "null";
    private string roomName = "null";
    private string chatChannel = "global"; // ���� ä�� �̸�
    void Start()
    {
        roomName = DataManager.instance.inputRoomName.text;
        userName = DataManager.instance.inputNickName.text;
        chatChannel = roomName + "_General";

        // ä�� �Է�â�� �̺�Ʈ ������ ���
        chatInputField.onSubmit.AddListener(SendChatMessage);
        Connect();
    }
    void Update()
    {
        if (chatClient != null)
        {
            chatClient.Service();
        }
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendChatMessage(chatInputField.text);
            // ��ȭ �Է�â�� ��Ŀ�� �Ǿ����� ���� �� EnterŰ�� ������
            if (chatInputField.isFocused == false)
                chatInputField.ActivateInputField(); // �Է�â�� ���� ���߱�
        }
    }
    public void SendChatMessage(string message)
    {
        SendMessage(message);
        chatInputField.text = ""; // �Է�â �ʱ�ȭ
    }
    void AddMessage(string message)
    {
        GameObject newText = Instantiate(textPrefab, content);
        newText.GetComponent<TMP_Text>().text = message;

        // ���� ������ �ؽ�Ʈ ������Ʈ�� ũ�⸦ Content ������Ʈ�� ũ�⿡ �°� �����մϴ�.
        content.sizeDelta += new Vector2(0, newText.GetComponent<RectTransform>().rect.height);

        // ��ũ�Ѻ並 ���ϴ����� �̵��մϴ�.
        content.parent.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;

        // Content�� ���̰� ScrollRect�� ���̺��� ũ��, ��ũ�ѹٸ� �� �Ʒ��� �����ϴ�.
        if (content.sizeDelta.y > scrollRect.GetComponent<RectTransform>().rect.height)
        {
            scrollRect.normalizedPosition = new Vector2(0, 0);
        }
    }
    private void UpdateChatUIForRoom(string sender, string message)
    {
        AddMessage(sender + ": " + message);
    }
    public void Connect()
    {
        chatClient = new ChatClient(this);
        chatClient.Connect(chatAppId, chatAppVersion, new AuthenticationValues(userName));
    }
    public void OnConnected()
    {
        Debug.Log("Connected to chat server.");
        chatClient.Subscribe(new string[] { chatChannel });
    }
    public void OnDisconnected()
    {
        Debug.Log("Disconnected from chat server.");
    }
    public void OnChatStateChange(ChatState state) { }
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        int msgCount = messages.Length;

        if (channelName.StartsWith(roomName)) // ä�� �̸��� �� �̸����� �����ϴ��� Ȯ��
        {
            for (int i = 0; i < msgCount; i++)
            {
                // Ư�� �濡 ���� ä�� UI�� ���� �޽����� ������Ʈ�մϴ�.
                UpdateChatUIForRoom(senders[i], messages[i].ToString());
            }
        }
    }
    public void OnPrivateMessage(string sender, object message, string channelName) { }
    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log("Subscribed to channel: " + channels[0]);
    }
    public void OnUnsubscribed(string[] channels) { }
    public void OnStatusUpdate(string user, int status, bool gotMessage, object message) { }
    public void OnUserSubscribed(string channel, string user) { }
    public void OnUserUnsubscribed(string channel, string user) { }
    public new void SendMessage(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            return;
        }
        if (chatClient != null && chatClient.CanChat)
        {
            chatClient.PublishMessage(chatChannel, message);
        }

        chatInputField.text = "";
    }
    public void OnSendButtonClick()
    {
        SendChatMessage(chatInputField.text);
    }
    public void DebugReturn(DebugLevel level, string message)
    {
        // Handle debug messages here
        // For example, you can log the debug messages to the Unity console
        Debug.Log("Chat debug return: " + message);
    }
}
