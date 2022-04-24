using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Input.Events;
using osuTK.Graphics;

namespace rebank.Game.Elements
{
    public class HoverableRoundedBox : RoundedBox
    {
        public Color4 NormalColor;
        public Color4 HoverColor;

        protected override bool OnHover(HoverEvent e)
        {
            Colour = HoverColor;

            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            Colour = NormalColor;

            base.OnHoverLost(e);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Colour = NormalColor;
        }
    }
}
