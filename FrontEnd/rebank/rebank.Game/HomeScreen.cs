using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;
using osu.Framework.Graphics.Transforms;
using rebank.Game.Elements;

namespace rebank.Game
{
    public class HomeScreen : Screen
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChild = new FillFlowContainer
            {
                Children = new Drawable[]
                {
                    new GameBanner
                    {
                        ClickAction = () =>
                        {
                            this.Push(new GameplayScreen());
                        }
                    },
                    new AccountBanner
                    {

                    }
                },
                RelativeSizeAxes = Axes.Both,
                Size = new osuTK.Vector2(1),
                Direction = FillDirection.Vertical
            };
        }


    }
}
