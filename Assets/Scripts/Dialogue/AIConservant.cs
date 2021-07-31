using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Control;
using RPG.Dialogue;
using UnityEngine;

public class AIConservant : MonoBehaviour, IRaycastable
{
    [SerializeField] private Dialogue currentDialogue;
    [SerializeField] private string conversantName;
    public bool HandleRaycast(PlayerController callingController)
    {
        if (currentDialogue == null)
        {
            return false;
        }

        if (GetComponent<Health>().IsDead())
        {
            return false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            callingController.GetComponent<PlayerConversant>().StartDialogue(this,currentDialogue);
        }

        return true;
    }

    public string GetName()
    {
        return conversantName;
    }

    public CursorType GetCursorType()
    {
        return CursorType.Dialogue;
    }
}
