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
        public ValueOutput isMatchTopic { get; private set; }

        [DoNotSerialize]
        public ValueOutput isMatchMessage { get; private set; }

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
            isMatchTopic = ValueOutput<bool>("Is Match Topic");
            isMatchMessage = ValueOutput<bool>("Is Match Message");
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


            if (compareTopic.Equals(receivedTopic))
            {
                flow.SetValue(isMatchTopic, true);
                if (compareMessage.Equals(receivedMessage))
                {
                    flow.SetValue(isMatchMessage, true);
                }
                else
                {
                    flow.SetValue(isMatchMessage, false);
                }

            }
            else
            {
                flow.SetValue(isMatchTopic, false);
				flow.SetValue(isMatchMessage, false);
			}
        }
    }
}


