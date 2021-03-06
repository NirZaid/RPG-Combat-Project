using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestStatus
    { 
        private Quest quest;
        private List<string> completedObjectives = new List<string>();
        [System.Serializable] 
        class QuestStatusRecord
        {
            public string questName;
            public List<string> completedObjectives;
            
        }

        public object CaptureState()
        {
            QuestStatusRecord state = new QuestStatusRecord();
            state.questName = quest.name;
            state.completedObjectives = completedObjectives;
            return state;
        }

        public QuestStatus(Quest quest)
        {
            this.quest = quest;
        }

        public QuestStatus(object record)
        {
            QuestStatusRecord state = record as QuestStatusRecord;
            quest = Quest.GetByName(state.questName);
            completedObjectives = state.completedObjectives;
        }


        public Quest GetQuest()
        {
            return quest;
        }

        public int GetCompletedCount()
        {
            return completedObjectives.Count;
        }

        public bool IsObjectiveComplete(string objective)
        {
            return completedObjectives.Contains(objective);
        }

        public void CompleteObjective(string objective)
        {
            if (quest.HasObjective(objective))
            {
                completedObjectives.Add(objective);
            }
        }

        public bool IsComplete()
        {
            return true;
        }
    }

}
