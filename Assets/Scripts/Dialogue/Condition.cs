using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    [System.Serializable]
    public class Condition
    {
        [SerializeField] private Disjunction[] and;

        public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            foreach (Disjunction disjunction in and)
            {
                if (disjunction.Check(evaluators) == false)
                {
                    return false;
                }
            }
            return true;
        }

        [System.Serializable]
        class Disjunction
        {
            [SerializeField]
            Predicate[] or;
            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (Predicate pred in or)
                {
                    if (pred.Check(evaluators) == true)
                    {
                        return true;
                    }
                }

                return false;
            }
        }
        
        
        [System.Serializable]
        class Predicate
        {
            [SerializeField] private string predicate;
            [SerializeField] private string[] paramaters;
            [SerializeField] private bool negate = false;
            
            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (var evaluator in evaluators)
                {
                    bool? result = evaluator.Evaluate(predicate, paramaters);
                    if (result == null)
                    {
                        continue;
                    }

                    if (result == negate)
                    {
                        return false;
                    }
                }

                return true;
            }
        }
        
        
       
    }

}
