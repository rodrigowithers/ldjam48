using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class ResourceStorageVisualizer : MonoBehaviour
    {
        public List<Resource> Resources;
        
        private void Update()
        {
            foreach (Resource resource in Resources)
            {
                resource.UpdateResource();
            }
        }
    }
}