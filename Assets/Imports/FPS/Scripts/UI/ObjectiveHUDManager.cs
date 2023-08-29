using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.UI
{
    public class ObjectiveHUDManager : MonoBehaviour
    {
        [Tooltip("UI panel containing the layoutGroup for displaying objectives")]
        public RectTransform ObjectivePanel;

        [Tooltip("Prefab for the primary objectives")]
        public GameObject PrimaryObjectivePrefab;

        [Tooltip("Prefab for the primary objectives")]
        public GameObject SecondaryObjectivePrefab;

        Dictionary<Objective, ObjectiveToast> m_ObjectivesDictionnary;

        void Awake()
        {
            m_ObjectivesDictionnary = new Dictionary<Objective, ObjectiveToast>();

            EventManager.AddListener<ObjectiveUpdateEvent>(OnUpdateObjective);

            Objective.OnObjectiveCreated += RegisterObjective;
            Objective.OnObjectiveCompleted += UnregisterObjective;
        }

        public void RegisterObjective(Objective objective)
        {
        }

        public void UnregisterObjective(Objective objective)
        {
        }

        void OnUpdateObjective(ObjectiveUpdateEvent evt)
        {
        }

        void OnDestroy()
        {
            EventManager.AddListener<ObjectiveUpdateEvent>(OnUpdateObjective);

            Objective.OnObjectiveCreated -= RegisterObjective;
            Objective.OnObjectiveCompleted -= UnregisterObjective;
        }
    }
}