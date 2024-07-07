using Naninovel;
using UnityEngine;

namespace Source.Infrastucture
{
    public class GameInit : MonoBehaviour
    {
        private async void Start() => 
            await RuntimeInitializer.InitializeAsync();
    }
}
