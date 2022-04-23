using osu.Framework.Platform;
using osu.Framework;
using REBankOsu.Game;

namespace REBankOsu.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableHost(@"REBankOsu"))
            using (osu.Framework.Game game = new REBankOsuGame())
                host.Run(game);
        }
    }
}
