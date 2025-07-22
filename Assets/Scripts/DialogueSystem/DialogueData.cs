using System;
using DialogueSystem.Preconditions;
using UnityEngine;
using UnityEngine.Serialization;

namespace DialogueSystem
{
    [Serializable]
    public struct DialogueData
    {
        public string speakerName;
        public string[] message;
        public Color speakerColor;
        public Sprite speakerImage;
        public SO_DialogueCondition[] preConditions;
        [SerializeField] internal bool hasBeenCompleted;
    }
}