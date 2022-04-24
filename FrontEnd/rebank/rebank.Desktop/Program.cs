using osu.Framework.Platform;
using osu.Framework;
using rebank.Game;

namespace rebank.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (DesktopGameHost host = Host.GetSuitableHost(@"rebank"))
            {
                using (osu.Framework.Game game = new rebankGame())
                    host.Run(game);
            }
        }
    }
}
