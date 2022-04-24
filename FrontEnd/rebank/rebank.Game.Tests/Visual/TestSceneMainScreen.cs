using osu.Framework.Graphics;
using osu.Framework.Screens;

namespace rebank.Game.Tests.Visual
{
    public class TestSceneMainScreen : rebankTestScene
    {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.

        public TestSceneMainScreen()
        {
            Add(new ScreenStack(new GameplayScreen()) { RelativeSizeAxes = Axes.Both });
        }
    }
}
