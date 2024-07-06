using Naninovel;
using Naninovel.UI;
using UnityEngine;

namespace Source.MiniGameService
{
    [InitializeAtRuntime]
    public class MiniGameService : IEngineService
    {
        private const string Minigame = "MiniGame";

        private readonly IUIManager _uiManager;
        private readonly ICustomVariableManager _customVariableManager;

        private UniTask _changeVisibilityTask;

        public MiniGameService(IUIManager uiManager, ICustomVariableManager customVariableManager)
        {
            _uiManager = uiManager;
            _customVariableManager = customVariableManager;
        }

        public UniTask InitializeServiceAsync()
        {
            _customVariableManager.OnVariableUpdated += OnVariableUpdated;
            return UniTask.CompletedTask;
        }

        public void ResetService()
        {
        }

        public void DestroyService() => 
            _customVariableManager.OnVariableUpdated -= OnVariableUpdated;

        private void OnVariableUpdated(CustomVariableUpdatedArgs obj)
        {
            if (obj.Name == "ItemTook" && obj.Value == "True")
            {
                Debug.Log(obj.Value);
                Stop();
            }
        }

        public async UniTask StartMiniGame()
        {
            IManagedUI ui = _uiManager.GetUI(Minigame);
            _changeVisibilityTask = ui.ChangeVisibilityAsync(true, 0);
            await UniTask.WhenAll(_changeVisibilityTask);
        }

        private async void Stop() => 
            await StopMiniGame();

        public async UniTask StopMiniGame()
        {
            IManagedUI ui = _uiManager.GetUI(Minigame);
            _changeVisibilityTask = ui.ChangeVisibilityAsync(false, 0);
            await UniTask.WhenAll(_changeVisibilityTask);
            ReturnToMap();
        }

        private void ReturnToMap() => 
            Engine.GetService<IScriptPlayer>().PreloadAndPlayAsync("Map");
    }
}