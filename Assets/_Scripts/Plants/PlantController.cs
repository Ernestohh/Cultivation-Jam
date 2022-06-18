using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Plants
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class PlantController : MonoBehaviour, IGrowable
    {
        [field: SerializeField] public string PlantName { get; set; }
        [field: SerializeField] public List<int> AmountOfGrowthStages { get; set; }
        [field: SerializeField] public int AmountOfDaysToGrow { get; set; }
        [field: SerializeField] public bool IsGrowing { get; set; }
        [field: SerializeField] public bool IsSick { get; set; }
        [field: SerializeField] public bool IsHarvestable { get; set; }
        [field: SerializeField] public List<Mesh> PlantMeshes { get; set; }
        [field: SerializeField] public Material PlantMaterial { get; set; }
        [field: SerializeField] public PlantScriptableObject PlantScriptableObject { get; set; }
        public UnityEvent OnGrowingHealthy { get; set; }
        public UnityEvent OnGetCured { get; set; }
        public UnityEvent OnGrowingSick { get; set; }
        public UnityEvent OnFullyGrownHealthy { get; set; }
        public UnityEvent OnFullyGrownSick { get; set; }

        private float _baseSickness;
        private float maximumScale = 3f;
        private List<string> _positivePlantEffects, _negativePlantEffects;
        
        private void Start()
        {
            SetPlantData();
            gameObject.GetComponent<MeshFilter>().mesh = PlantMeshes[0];
            gameObject.GetComponent<MeshRenderer>().material = PlantMaterial;
            gameObject.GetComponent<MeshCollider>().convex = true;
            gameObject.GetComponent<MeshCollider>().sharedMesh = PlantMeshes[0];
        }

        public void SetPlantData()
        {
            PlantName = PlantScriptableObject.PlantName;
            AmountOfDaysToGrow = PlantScriptableObject.AmountOfDaysToGrow;
            AmountOfGrowthStages = PlantScriptableObject.AmountOfGrowthStages;
            _baseSickness = PlantScriptableObject.BaseSicknessChance;
            _positivePlantEffects = PlantScriptableObject.PositivePlantEffects;
            _negativePlantEffects = PlantScriptableObject.NegativePlantEffects;
            PlantMeshes = PlantScriptableObject.PlantMeshes;
            PlantMaterial = PlantScriptableObject.PlantMaterial;
        }
    }
}