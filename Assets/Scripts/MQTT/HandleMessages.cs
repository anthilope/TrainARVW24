using Unity.VisualScripting;
using UnityEngine;

namespace MQTT_Messages
{
    public static class EventNames
    {
        public static string MessageWasReceived = "OnMessageReceived";
    }

    [UnitTitle("MQTT: MessageReceived")]
    [UnitCategory("Events")]
    public class MessageReceived : EventUnit<string>
    {
        [Inspectable]
        [UnitHeaderInspectable("Compare String")]
        public string compareString;

        [DoNotSerialize]
        public ValueOutput message { get; private set; }

        [DoNotSerialize]
        public ValueOutput isMatch { get; private set; }

        protected override bool register => true;

        public override EventHook GetHook(GraphReference reference)
        {
            Debug.Log("Event registered: " + EventNames.MessageWasReceived);
            return new EventHook(EventNames.MessageWasReceived);
        }

        protected override void Definition()
        {
            base.Definition();

            message = ValueOutput<string>("Message");
            isMatch = ValueOutput<bool>("Is Match");
        }

        protected override void AssignArguments(Flow flow, string messageArg)
        {
            flow.SetValue(message, messageArg);
            bool match = messageArg == compareString;
            flow.SetValue(isMatch, match);
        }
    }
}
