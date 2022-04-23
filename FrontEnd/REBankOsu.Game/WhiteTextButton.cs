using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics.UserInterface;
using osuTK.Graphics;

namespace REBankOsu.Game
{
    public class WhiteTextButton : BasicButton
    {

        [BackgroundDependencyLoader]
        private void load()
        {
            SpriteText.Colour = Color4.White;
        }
    }
}
