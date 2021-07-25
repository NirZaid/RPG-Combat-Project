using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {

        Dialogue currentDialogue;
        private DialogueNode currentNode = null;
        private bool isChoosing = false;
        private AIConservant currentConversant = null;

        public event Action onConversationUpdated;


        public bool IsActive()
        {
            return currentDialogue != null;
        }

        [SerializeField] private string playerName;

        public string GetCurrentConversantName()
        {
            if (isChoosing)
            {
                return playerName;
            }
            else
            {
                return currentConversant.GetName();
            }
        }

        public bool IsChoosing()
        {
            return isChoosing;
        }

        public void Quit()
        {
            TriggerExitAction();
            currentConversant = null;
            currentDialogue = null;
            currentNode = null;
            isChoosing = false;
            onConversationUpdated();
        }

        public string GetText()
        {
            if (currentNode == null)
            {
                return "";
            }

            return currentNode.GetText();
        }

        public void StartDialogue(AIConservant newConservant, Dialogue newDialogue)
        {
            currentConversant = newConservant;
            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode();
            TriggerEnterAction();
            onConversationUpdated();
        }


        public void Next()
        {
            int numOfPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();
            if (numOfPlayerResponses > 0)
            {
                isChoosing = true;
                TriggerExitAction();
                onConversationUpdated();
                return;
            }
            DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Length);
            TriggerExitAction();
            currentNode = children[randomIndex];
            TriggerEnterAction();
            onConversationUpdated();
        }

        public bool HasNext()
        {
            if (currentDialogue == null)
            {
                return false;
            }

            return currentDialogue.GetAllChildren(currentNode).Count() > 0;
        }


        public void SelectChoice(DialogueNode chosenNode)
        {
            currentNode = chosenNode;
            TriggerEnterAction();
            isChoosing = false;
            Next();
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            foreach (DialogueNode node in currentDialogue.GetPlayerChildren(currentNode))
            {
                yield return node;
            }

        }

        private void TriggerEnterAction()
        {
            if (currentNode != null)
            {
                TriggerAction(currentNode.GetOnEnterAction());
            }
        }
        
        private void TriggerExitAction()
        {
            if (currentNode != null)
            {
                TriggerAction(currentNode.GetOnExitAction());
            }
        }

        private void TriggerAction(string action)
        {
            if (action == "")
            {
                return;
            }

            foreach (DialogueTrigger trigger in currentConversant.GetComponents<DialogueTrigger>())
            {
                trigger.Trigger(action);
            }
        }
    }

}
