using System;
using Controller;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class UI_DialogueManager : MonoBehaviour
    {
        public static UI_DialogueManager instance;

        [Header("UI References")] [SerializeField]
        private GameObject dialoguePanel;

        [SerializeField] private TMP_Text characterNameText;
        [SerializeField] private Image characterImage;
        [SerializeField] private TMP_Text dialogueText;

        [Header("Configuration")] [SerializeField]
        private float typingSpeed = 0.05f; // Seconds Per Character

        private DialogueData _currentDialogue;
        private int _messageIndex;

        // Typing Data
        private bool _isTyping;
        private float _typingTimer;
        private int _charIndex;
        private string _currentSentence;

        internal event Func<bool> OnEndDialogue;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            dialoguePanel.gameObject.SetActive(false);

            ControllerManager.AddPerformanceEvent(InputNames.PassDialogue, OnSkipDialogue, ActionMapType.UI);
        }

        /// <summary>
        /// Start new Dialogue.
        /// </summary>
        public void InitDialogue(DialogueData dialogue)
        {
            _currentDialogue = dialogue;
            _messageIndex = 0;

            dialoguePanel.gameObject.SetActive(true);
            characterNameText.color = dialogue.speakerColor;
            characterNameText.text = dialogue.speakerName;
            characterImage.sprite = dialogue.speakerImage;

            StartTypingLine(_currentDialogue.message[_messageIndex]);
        }

        /// <summary>
        /// Configure new starting point.
        /// </summary>
        private void StartTypingLine(string sentence)
        {
            _currentSentence = sentence;
            _charIndex = 0;
            _typingTimer = 0f;
            _isTyping = true;
            dialogueText.text = string.Empty;
        }

        private void Update()
        {
            if (!dialoguePanel.gameObject.activeSelf)
                return;

            if (!_isTyping) return;

            _typingTimer += Time.deltaTime;
            if (!(_typingTimer >= typingSpeed)) return;

            dialogueText.text += _currentSentence[_charIndex];
            _charIndex++;
            _typingTimer = 0f;

            if (_charIndex >= _currentSentence.Length)
            {
                _isTyping = false;
            }
        }

        /// <summary>
        /// End dialogue.
        /// </summary>
        private void EndDialogue()
        {
            if (OnEndDialogue != null && OnEndDialogue())
            {
                dialoguePanel.gameObject.SetActive(false);
            }
            else if (OnEndDialogue == null)
            {
                dialoguePanel.gameObject.SetActive(false);
            }
        }

        private void OnSkipDialogue(InputAction.CallbackContext context)
        {
            if (!dialoguePanel.gameObject.activeSelf)
                return;

            if (_isTyping)
            {
                dialogueText.text = _currentSentence;
                _isTyping = false;
            }
            else
            {
                _messageIndex++;
                if (_messageIndex < _currentDialogue.message.Length)
                {
                    StartTypingLine(_currentDialogue.message[_messageIndex]);
                }
                else
                {
                    EndDialogue();
                }
            }
        }
    }
}