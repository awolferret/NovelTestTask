using Naninovel;
using Naninovel.UI;

namespace Source.MiniGameService
{
    [InitializeAtRuntime]
    public class MiniGameService : IEngineService
    {
        private const string MiniGame = "MiniGame";
        private const string ParamName = "ItemTook";
        private const string True = "True";
        private const string ScriptName = "Map";

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
            if (obj.Name == ParamName && obj.Value == True)
            {
                Stop();
            }
        }

        public async UniTask StartMiniGame()
        {
            IManagedUI ui = _uiManager.GetUI(MiniGame);
            _changeVisibilityTask = ui.ChangeVisibilityAsync(true, 0);
            await UniTask.WhenAll(_changeVisibilityTask);
        }

        private async void Stop() => 
            await StopMiniGame();

        public async UniTask StopMiniGame()
        {
            IManagedUI ui = _uiManager.GetUI(MiniGame);
            _changeVisibilityTask = ui.ChangeVisibilityAsync(false, 0);
            await UniTask.WhenAll(_changeVisibilityTask);
            ReturnToMap();
        }

        private void ReturnToMap() => 
            Engine.GetService<IScriptPlayer>().PreloadAndPlayAsync(ScriptName);
    }
}