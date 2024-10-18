using System;
using System.Diagnostics;
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
    public class MessageReceived : EventUnit<Tuple<string, string>>
    {
        [Inspectable]
        [UnitHeaderInspectable("Compare Topic")]
        public string compareTopic;

        [Inspectable]
        [UnitHeaderInspectable("Compare Message")]
        public string compareMessage;

        [DoNotSerialize]
        public ValueOutput message { get; private set; }

        [DoNotSerialize]
        public ValueOutput topic { get; private set; }

        [DoNotSerialize]
        public ValueOutput isMatch { get; private set; }

        protected override bool register => true;

        public override EventHook GetHook(GraphReference reference)
        {
            //Debug.Log("Event registered: " + EventNames.MessageWasReceived);
            return new EventHook(EventNames.MessageWasReceived);
        }

        protected override void Definition()
        {
            base.Definition();

            topic = ValueOutput<string>("Topic");
            message = ValueOutput<string>("Message");
            isMatch = ValueOutput<bool>("Is Match");
        }

        protected override void AssignArguments(Flow flow, Tuple<string, string> args)
        {
            string receivedTopic = args.Item1;
            string receivedMessage = args.Item2;
            flow.SetValue(topic, receivedTopic);
            flow.SetValue(message, receivedMessage);
            /*
            bool match = receivedMessage == compareString;
            flow.SetValue(isMatch, match);
            */

            if (receivedMessage.Equals(compareMessage) && receivedTopic.Equals(compareTopic))
            {
                bool match = true;
                flow.SetValue(isMatch, match);

            }
            else
            {
                bool match = false;
                flow.SetValue(isMatch, match);
            }
        }
    }
}


