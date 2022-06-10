using System;
using UnityEngine;

namespace _Scripts.Plants
{
    [RequireComponent(typeof(PlantController))]
    public class Plant : PlantController
    {
        private void Update()
        {
            if(Input.GetAxisRaw("Horizontal") > 0 && !IsGrowing)
                OnGrowingHealthy?.Invoke();
        }

        public void Growing()
        {
            StartCoroutine(StartToGrow());
        }
    }
}