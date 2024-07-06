using Naninovel;

namespace Source.MiniGameService
{
    [Command.CommandAlias("startMiniGame")]
    public class StartMiniGame : Command
    {
        public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            MiniGameService miniGameService = Engine.GetService<MiniGameService>();
            await miniGameService.StartMiniGame();
        }
    }
}
