using System.Collections;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class MQTTMessageEvent : UnityEvent<string> { }

[UnitTitle("Subscribe Message")]
[UnitCategory("TrainAR/MQTT")]
[IncludeInSettings(true)]
public class Coroutine_Subscribe : Unit
{
    [DoNotSerialize]
    public ValueInput messageInput { get; private set; }

    [DoNotSerialize]
    public ControlInput inputFlow { get; private set; }

    [DoNotSerialize]
    public ControlOutput outputFlow { get; private set; }

    public MQTTMessageEvent onMessageReceived;

    public Text fieldOfText;

    protected override void Definition()
    {
        messageInput = ValueInput<string>("message");
        inputFlow = ControlInput("inputFlow", Trigger);
        outputFlow = ControlOutput("outputFlow");

        Succession(inputFlow, outputFlow);
        Requirement(messageInput, inputFlow);
    }

    public ControlOutput Trigger(Flow flow)
    {
        string message = flow.GetValue<string>(messageInput);
        OnMessageReceived(message);
        return outputFlow;
    }

    private void Awake()
    {
        onMessageReceived.AddListener(HandleMessageChanged);
    }

    void OnMessageReceived(string newMessage)
    {
        onMessageReceived.Invoke(newMessage);
    }

    public void HandleMessageChanged(string newMessage)
    {
        Debug.Log("Neue Nachricht: " + newMessage);
        fieldOfText.text = newMessage;
    }
}
