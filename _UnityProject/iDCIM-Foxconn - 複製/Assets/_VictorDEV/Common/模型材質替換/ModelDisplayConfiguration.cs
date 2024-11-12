using System;
using System.Collections.Generic;
using UnityEngine;

namespace VictorDev.Common
{
    [Serializable]
    public class ModelDisplayConfiguration
    {
        [SerializeField] List<Transform> models;
        [SerializeField] List<string> objKeyWords;
        public List<Transform> modelsList => models;

        public void FindTargetObjects()
        {
            models = MaterialHandler.FindTargetObjects(objKeyWords);
        }

        public void AddColliderToObjects()
        {
            ObjectHandler.AddColliderToObjects(models, new BoxCollider());
        }
        public void RemoveColliderFromObjects()
        {
            ObjectHandler.RemoveColliderFromObjects(models);
        }
    }
}