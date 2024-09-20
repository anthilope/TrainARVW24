using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;
using UnityEngine.Events;
using TMPro;
using Unity.VisualScripting;
using MQTT_Messages;



public class MQTT_Client : M2MqttUnityClient
{
    [System.Serializable]
    public class MQTTMessageEvent : UnityEvent<string> { }
    public MQTTMessageEvent onMessageReceived;

    public GameObject debugObject;

    [Tooltip("Set the topic to subscribe. !!!ATTENTION!!! multi-level wildcard # subscribes to all topics")]
    public string topicSubscribe = "";

    public void TestPublish()
    {
        client.Publish("test/unity", System.Text.Encoding.UTF8.GetBytes("Test message"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        Debug.Log("Test message published");
    }

    public void PublishMessage(string topic, string message)
    {
        client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
    }

    void Start()
    {
        ChangeDebugText("Testing");
    }

    void Update()
    {
        base.Update();
    }

    public void TryConnection()
    {
        base.Connect();
    }

    protected override void SubscribeTopics()
    {
        client.Subscribe(new string[] { topicSubscribe }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
    }

    public void SubscribeToTopic(string topic)
    {
        client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
    }

    protected override void UnsubscribeTopics()
    {
        client.Unsubscribe(new string[] { topicSubscribe });
    }

    public void SetBrokerAddress(string brokerAddress)
    {
        this.brokerAddress = brokerAddress;
    }

    public void ChangeDebugText(string text)
    {
        if (debugObject != null)
        {
            TextMeshProUGUI tmp = debugObject.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
            {
                tmp.text = text;
            }
            else
            {
               Debug.LogError("TextMeshProUGUI component not found on the 'debug' GameObject.");
            }
        }
        else
        {
            Debug.LogError("No GameObject found with the 'debug' tag.");
        }
    }

    public void debugging(string message)
    {
        ChangeDebugText(message);
    }

    public void TrySubscribe(string topicToSubscribe)
    {
        topicSubscribe = topicToSubscribe;
        SubscribeTopics();
        ChangeDebugText(topicSubscribe);
    }

    protected override void DecodeMessage(string topic, byte[] message)
    {
        base.DecodeMessage(topic, message);

        // Convert the byte array to a string
        string receivedMessage = System.Text.Encoding.UTF8.GetString(message);

        

        // Update the debug text
        ChangeDebugText(receivedMessage);

        // Trigger the Visual Scripting event
        //EventBus.Trigger(Visual_Scripting.VisualScriptingEventNames_messages.MessageWasReceived, receivedMessage);
        EventBus.Trigger(EventNames.MessageWasReceived, receivedMessage);



        // Invoke any Unity Events if needed
        onMessageReceived?.Invoke(receivedMessage);
    }

}
