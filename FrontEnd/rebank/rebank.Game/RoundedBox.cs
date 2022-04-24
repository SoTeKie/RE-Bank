using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace rebank.Game
{
    public class RoundedBox : CompositeDrawable
    {
        private CircularContainer content;

        public override Quad ScreenSpaceDrawQuad => content.ScreenSpaceDrawQuad;

        public bool RoundTopLeft = true;
        public bool RoundTopRight = true;
        public bool RoundBottomLeft = true;
        public bool RoundBottomRight = true;

        public Texture Texture = null;
        public Axes TextureRelativeSizeAxes = Axes.None;
        public RectangleF TextureRectangle = new RectangleF(0, 0, 32, 32);

        public RoundedBox()
        {
            //Padding = new MarginPadding { Horizontal = -5 / 2f };
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            List<Drawable> children = new List<Drawable>();
            if (Texture == null)
                children.Add(content = new Circle
                {
                    RelativeSizeAxes = Axes.Both
                });
            else
                children.Add(content = new CircularContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    Children = new Drawable[]
                    {
                        new Sprite
                        {
                            RelativeSizeAxes = Axes.Both,
                            Texture = Texture,
                            TextureRelativeSizeAxes = TextureRelativeSizeAxes,
                            TextureRectangle = TextureRectangle,
                        }
                    }
                });

            if (!RoundTopLeft)
                children.Add(new Sprite
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.5f, 0.5f),
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopRight,
                    Texture = Texture,
                    TextureRelativeSizeAxes = TextureRelativeSizeAxes,
                    TextureRectangle = TextureRectangle,
                });
            if (!RoundTopRight)
                children.Add(new Sprite
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.5f, 0.5f),
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopLeft,
                    Texture = Texture,
                    TextureRelativeSizeAxes = TextureRelativeSizeAxes,
                    TextureRectangle = TextureRectangle,
                });
            if (!RoundBottomLeft)
                children.Add(new Sprite
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.5f, 0.5f),
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomRight,
                    Texture = Texture,
                    TextureRelativeSizeAxes = TextureRelativeSizeAxes,
                    TextureRectangle = TextureRectangle,
                });
            if (!RoundBottomRight)
                children.Add(new Sprite
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.5f, 0.5f),
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomLeft,
                    Texture = Texture,
                    TextureRelativeSizeAxes = TextureRelativeSizeAxes,
                    TextureRectangle = TextureRectangle,
                });

            InternalChild = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Children = children
            };
        }
    }
}
