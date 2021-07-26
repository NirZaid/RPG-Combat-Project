using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using TMPro;
using UnityEngine;

namespace RPG.UI.Quests
{
    
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI progress;

        private QuestStatus questStatus;
        
        public void Setup(QuestStatus status)
        {
            questStatus = status;
            title.text = status.GetQuest().GetTitle();
            progress.text = status.GetCompletedCount() + "/" + status.GetQuest().GetObjectiveCount();
        }


        public QuestStatus GetQuestStatus()
        {
            return questStatus;
        }

    }

}
