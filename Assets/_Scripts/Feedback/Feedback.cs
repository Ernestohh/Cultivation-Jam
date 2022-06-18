using System;
using UnityEngine;

namespace _Scripts.Feedback
{
    public abstract class Feedback : MonoBehaviour
    {
        public abstract void CreateFeedback();
        public abstract void CompletePreviousFeedback();

        protected virtual void OnDestroy()
        {
            CompletePreviousFeedback();
        }
    }
}