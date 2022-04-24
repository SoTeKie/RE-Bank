using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Graphics.UserInterface;
using osuTK.Graphics;

namespace rebank.Game.Elements
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
