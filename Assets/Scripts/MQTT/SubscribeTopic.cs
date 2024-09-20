using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine;
using static UI.QuestionnaireController;
using Static;
using TMPro;


[UnitTitle("Subscribe Topic")]
[UnitCategory("TrainAR/MQTT")]
[IncludeInSettings(true)]
    public class SubscribeTopic : Unit
    {
        [Inspectable]
        public int numberOfTopics = 4;

        [DoNotSerialize]
        public List<ValueInput> topicsToSubscribe { get; private set; } = new List<ValueInput>();

        [DoNotSerialize]
        public ControlInput inputFlow { get; private set; }

        [DoNotSerialize]
        public ControlOutput outputFlow { get; private set; }

        protected override void Definition()
        {
            for (int i = 0; i < numberOfTopics; i++)
            {
                //Output flow (this is only a paththrough for an output Unit)
                topicsToSubscribe.Add(ValueInput<string>(NumberToString(i + 1) + " Subscribe to topic:", string.Empty));
            }
            // = ValueInput<List<string>>("topicName", new List<string>());
            inputFlow = ControlInput("inputFlow", Trigger);
            outputFlow = ControlOutput("outputFlow");

            Succession(inputFlow, outputFlow);
            //Requirement(topicName, inputFlow);
        }

        public ControlOutput Trigger(Flow flow)
        {
        GameObject text = GameObject.FindWithTag("debug");
        TextMeshProUGUI textMeshPro = text.GetComponent<TextMeshProUGUI>();
       

        GameObject mqtt_Object = GameObject.FindWithTag("client");
            if (mqtt_Object != null)
            {
                MQTT_Client mqttClient = mqtt_Object.GetComponent<MQTT_Client>();

                if (mqttClient != null)
                {
                    foreach (ValueInput topicInput in topicsToSubscribe)
                    {
                        string topic = flow.GetValue<string>(topicInput);
                        

                        if (!string.IsNullOrEmpty(topic))
                        {
                        //mqttClient.ChangeDebugText(topic);
                        mqttClient.TrySubscribe(topic);
                        Debug.Log("I subscribed to this topic:" + topic);
                        }
                    }
                }
            }

            return outputFlow;
        }

        private string NumberToString(int value)
        {
            string returnString = value + "th";
            switch (value)
            {
                case 1:
                    returnString = "1st";
                    break;
                case 2:
                    returnString = "2nd";
                    break;
                case 3:
                    returnString = "3rd";
                    break;
                default:
                    break;
            }
            return returnString;
        }

    }

