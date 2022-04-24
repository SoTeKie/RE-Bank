using osu.Framework;
using osu.Framework.Platform;

namespace rebank.Game.Tests
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableHost("visual-tests"))
            using (var game = new rebankTestBrowser())
                host.Run(game);
        }
    }
}
