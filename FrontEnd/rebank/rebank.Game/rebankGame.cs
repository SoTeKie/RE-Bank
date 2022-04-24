using System.Drawing;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Screens;

namespace rebank.Game
{
    public class rebankGame : rebankGameBase
    {
        private ScreenStack screenStack;

        [BackgroundDependencyLoader]
        private void load(FrameworkConfigManager config)
        {
            // Add your top-level game components here.
            // A screen stack and sample screen has been provided for convenience, but you can replace it if you don't want to use screens.
            Child = screenStack = new ScreenStack { RelativeSizeAxes = Axes.Both };
            config.GetBindable<Size>(FrameworkSetting.WindowedSize).Value = new Size(366, 768);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            //screenStack.Push(new GameplayScreen());
            screenStack.Push(new LoginScreen());
        }
    }
}
