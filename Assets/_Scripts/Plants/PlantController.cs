using System;
using System.Collections.Generic;
using _Scripts.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Plants
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class PlantController : MonoBehaviour, IGrowable
    {
        public bool IsGrowing { get; set; }
        public bool IsSick { get; set; }
        public bool IsHarvestable { get; set; }
        [field: SerializeField] public PlantScriptableObject PlantScriptableObject { get; set; }
        public UnityEvent OnGrowingHealthy { get; set; }
        public UnityEvent OnGrowingSick { get; set; }
        public UnityEvent OnFullyGrownHealthy { get; set; }
        public UnityEvent OnFullyGrownSick { get; set; }

        private string plantName;
        private int growTime;
        private float baseSickness;
        private List<string> positivePlantEffects, negativePlantEffects;
        private Mesh plantMesh;
        private Material plantMaterial;

        private void Start()
        {
            InitializePlantData();
        }

        private void InitializePlantData()
        {
            SetPlantData();
            gameObject.GetComponent<MeshFilter>().mesh = plantMesh;
            gameObject.GetComponent<MeshRenderer>().material = plantMaterial;
            gameObject.GetComponent<MeshCollider>().convex = true;
        }

        private void SetPlantData()
        {
            plantName = PlantScriptableObject.PlantName;
            growTime = PlantScriptableObject.GrowTime;
            baseSickness = PlantScriptableObject.BaseSicknessChance;
            positivePlantEffects = PlantScriptableObject.PositivePlantEffects;
            negativePlantEffects = PlantScriptableObject.NegativePlantEffects;
            plantMesh = PlantScriptableObject.PlantMesh;
            plantMaterial = PlantScriptableObject.PlantMaterial;
        }
    }
}