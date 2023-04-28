using UnityEngine;

namespace UI
{
    public  class ComponentExtensions
    { 
        public void EnsureComponent<T>( Component component, ref T output) where T : Component
        {
            if (!output && !component.TryGetComponent<T>(out output))
            {
                output = component.gameObject.AddComponent<T>();
            }
        }
    }
}