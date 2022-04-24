using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;

namespace rebank.Game.Elements
{
    public class Map : CompositeDrawable
    {
        private List<Cut> cuts = new List<Cut>();
        private List<River> rivers = new List<River>();
        public List<Quad> CollisionBoxes = new List<Quad>();
        private Player player;
        private bool isMouseDown = false;

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            var pos = ToLocalSpace(e.MouseDownPosition);
            pos = Vector2.Divide(pos, ScreenSpaceDrawQuad.Size);

            if(pos.X < 0 || pos.X > 1 || pos.Y < 0 || pos.Y > 1)
                return base.OnMouseDown(e);

            player.MoveTowards(pos);
            isMouseDown = true;
            return true;
        }

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            if(isMouseDown)
            {
                var pos = ToLocalSpace(e.MousePosition);
                pos = Vector2.Divide(pos, ScreenSpaceDrawQuad.Size);

                if (pos.X < 0 || pos.X > 1 || pos.Y < 0 || pos.Y > 1)
                    return base.OnMouseMove(e);

                player.MoveTowards(pos);
                return true;
            }

            return base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            isMouseDown = false;
            base.OnMouseUp(e);
        }

        bool doneInit = false;
        protected override void Update()
        {
            if (!doneInit)
            {
                OnPlayerMove(true);
                doneInit = true;
            }

            base.Update();
        }

        public Map()
        {
            CollisionBoxes.Add(new Quad(-0.1f, -0.1f, 1.2f, 0.1f));
            CollisionBoxes.Add(new Quad(-0.1f, -0.1f, 0.1f, 1.2f));
            CollisionBoxes.Add(new Quad(1.1f, -0.1f, 0.1f, 1.2f));
            CollisionBoxes.Add(new Quad(-0.1f, 1.1f, 1.2f, 0.1f));

            var r = new Random();

            var num = r.Next(2, 7);
            for (int i = 0; i < num; i++)
            {
                var side = FromID(r.Next(0, 4));
                var offset = r.Next(0, 16);
                var length = r.Next(1, 16 - offset);
                if (side == Side.Top || side == Side.Bottom)
                {
                    offset = r.Next(0, 14);
                    length = r.Next(1, 14 - offset);
                }
                cuts.Add(new Cut
                {
                    Side = side,
                    Offset = offset,
                    Length = length
                });
                var x = (side == Side.Top || side == Side.Bottom) ? offset + 1 : (side == Side.Left ? 0 : 15);
                var y = (side == Side.Left || side == Side.Right) ? offset : (side == Side.Top ? 0 : 15);
                var w = (side == Side.Left || side == Side.Right) ? 1 : length;
                var h = (side == Side.Top || side == Side.Bottom) ? 1 : length;
                CollisionBoxes.Add(new Quad(x * 0.0625f, y * 0.0625f, w * 0.0625f, h * 0.0625f));
            }

            num = r.Next(2, 7);
            for (int i = 0; i < num; i++)
            {
                var side = FromID(r.Next(0, 4));
                var offset = r.Next(0, 16);
                var fixup = 0;
                if (FieldIsCut2(side, offset))
                    fixup = 1;
                var x = (side == Side.Top || side == Side.Bottom) ? offset : (side == Side.Left ? fixup : 15 - fixup);
                var y = (side == Side.Left || side == Side.Right) ? offset : (side == Side.Top ? fixup : 15 - fixup);
                if(rivers.Any(z => (z.X == x && z.Y == y) || (z.Segments.Any(g => g.x == x && g.y == y))))
                {
                    i--;
                    continue;
                }
                var river = new River
                {
                    InFlowSide = side,
                    X = x,
                    Y = y,
                    Segments = new List<(int x, int y)>()
                };
                river.Segments.Add((x, y));
                while (r.NextDouble() < 0.9)
                {
                    var lastSegment = river.Segments.Last();

                    var lex = PieceExists(lastSegment.x - 1, lastSegment.y, river);
                    var rex = PieceExists(lastSegment.x + 1, lastSegment.y, river);
                    var tex = PieceExists(lastSegment.x, lastSegment.y - 1, river);
                    var bex = PieceExists(lastSegment.x, lastSegment.y + 1, river);

                    var check = new List<int>();
                    if (tex)
                        check.Add(0);
                    if (bex)
                        check.Add(1);
                    if (lex)
                        check.Add(2);
                    if (rex)
                        check.Add(3);

                    if (check.Count < 1)
                        break;

                    var rside = r.Next(0, check.Count);
                    var frside = check[rside];
                    river.Segments.Add(MoveByID(frside, lastSegment.x, lastSegment.y));
                }
                rivers.Add(river);
            }
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            var container = new List<Drawable>
            {
                new Sprite
                {
                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.875f),
                    Colour = Color4.LightGreen,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Texture = textures.Get("grass", osu.Framework.Graphics.OpenGL.Textures.WrapMode.Repeat, osu.Framework.Graphics.OpenGL.Textures.WrapMode.Repeat),
                    TextureRelativeSizeAxes = Axes.None,
                    TextureRectangle = new RectangleF(0, 0, 32, 32),
                    
                }
            };
            var tlCut = !(!FieldIsCut(Side.Left, 0) && (!FieldIsCut(Side.Top, 0) || !FieldIsCut(Side.Left, 1)));
            var trCut = !(!FieldIsCut(Side.Right, 0) && (!FieldIsCut(Side.Top, 13) || !FieldIsCut(Side.Right, 1)));
            var blCut = !(!FieldIsCut(Side.Left, 15) && (!FieldIsCut(Side.Bottom, 0) || !FieldIsCut(Side.Left, 14)));
            var brCut = !(!FieldIsCut(Side.Right, 15) && (!FieldIsCut(Side.Bottom, 13) || !FieldIsCut(Side.Right, 14)));
            if (!tlCut)
            {
                var topRight = FieldIsCut(Side.Top, 0);
                var botLeft = FieldIsCut(Side.Left, 1);

                container.Add(new RoundedBox
                {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.0625f),
                    Position = new Vector2(0),
                    Colour = Color4.LightGreen,
                    RoundBottomRight = false,
                    RoundTopRight = topRight,
                    RoundBottomLeft = botLeft,
                    Texture = textures.Get("grass", osu.Framework.Graphics.OpenGL.Textures.WrapMode.Repeat, osu.Framework.Graphics.OpenGL.Textures.WrapMode.Repeat),
                });
            }
            if (!trCut)
            {
                var topLeft = FieldIsCut(Side.Top, 13);
                var botRight = FieldIsCut(Side.Right, 1);

                container.Add(new RoundedBox
                {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.0625f),
                    Position = new Vector2(0.9375f, 0),
                    Colour = Color4.LightGreen,
                    RoundBottomLeft = false,
                    RoundTopLeft = topLeft,
                    RoundBottomRight = botRight,
                    Texture = textures.Get("grass", osu.Framework.Graphics.OpenGL.Textures.WrapMode.Repeat, osu.Framework.Graphics.OpenGL.Textures.WrapMode.Repeat),
                });
            }
            if (!blCut)
            {
                var botRight = FieldIsCut(Side.Bottom, 0);
                var topLeft = FieldIsCut(Side.Left, 14);

                container.Add(new RoundedBox
                {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.0625f),
                    Position = new Vector2(0, 0.9375f),
                    Colour = Color4.LightGreen,
                    RoundTopRight = false,
                    RoundTopLeft = topLeft,
                    RoundBottomRight = botRight,
                    Texture = textures.Get("grass", osu.Framework.Graphics.OpenGL.Textures.WrapMode.Repeat, osu.Framework.Graphics.OpenGL.Textures.WrapMode.Repeat),
                });
            }
            if (!brCut)
            {
                var topRight = FieldIsCut(Side.Right, 14);
                var botLeft = FieldIsCut(Side.Bottom, 13);

                container.Add(new RoundedBox
                {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.0625f),
                    Position = new Vector2(0.9375f),
                    Colour = Color4.LightGreen,
                    RoundTopLeft = false,
                    RoundTopRight = topRight,
                    RoundBottomLeft = botLeft,
                    Texture = textures.Get("grass", osu.Framework.Graphics.OpenGL.Textures.WrapMode.Repeat, osu.Framework.Graphics.OpenGL.Textures.WrapMode.Repeat),
                });
            }
            for (int i = 0; i < 14; i++)
            {
                if (FieldIsCut(Side.Top, i))
                    continue;

                var tl = FieldIsCut2(Side.Top, i - 1);
                var tr = FieldIsCut2(Side.Top, i + 1);

                container.Add(new RoundedBox
                {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.0625f),
                    Position = new Vector2(0.0625f * (i + 1), 0),
                    Colour = Color4.LightGreen,
                    RoundBottomRight = false,
                    RoundBottomLeft = false,
                    RoundTopRight = tr,
                    RoundTopLeft = tl,
                    Texture = textures.Get("grass", osu.Framework.Graphics.OpenGL.Textures.WrapMode.Repeat, osu.Framework.Graphics.OpenGL.Textures.WrapMode.Repeat),
                });
            }
            for (int i = 0; i < 14; i++)
            {
                if (FieldIsCut(Side.Bottom, i))
                    continue;

                var bl = FieldIsCut2(Side.Bottom, i - 1);
                var br = FieldIsCut2(Side.Bottom, i + 1);

                container.Add(new RoundedBox
                {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.0625f),
                    Position = new Vector2(0.0625f * (i + 1), 0.9375f),
                    Colour = Color4.LightGreen,
                    RoundTopRight = false,
                    RoundTopLeft = false,
                    RoundBottomRight = br,
                    RoundBottomLeft = bl,
                    Texture = textures.Get("grass", osu.Framework.Graphics.OpenGL.Textures.WrapMode.Repeat, osu.Framework.Graphics.OpenGL.Textures.WrapMode.Repeat),
                });
            }
            for (int i = 1; i < 15; i++)
            {
                if (FieldIsCut(Side.Left, i))
                    continue;

                var tl = FieldIsCut2(Side.Left, i - 1);
                var bl = FieldIsCut2(Side.Left, i + 1);

                container.Add(new RoundedBox
                {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.0625f),
                    Position = new Vector2(0, 0.0625f * i),
                    Colour = Color4.LightGreen,
                    RoundTopRight = false,
                    RoundTopLeft = tl,
                    RoundBottomRight = false,
                    RoundBottomLeft = bl,
                    Texture = textures.Get("grass", osu.Framework.Graphics.OpenGL.Textures.WrapMode.Repeat, osu.Framework.Graphics.OpenGL.Textures.WrapMode.Repeat),
                });
            }
            for (int i = 1; i < 15; i++)
            {
                if (FieldIsCut(Side.Right, i))
                    continue;

                var tr = FieldIsCut2(Side.Right, i - 1);
                var br = FieldIsCut2(Side.Right, i + 1);

                container.Add(new RoundedBox
                {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = new Vector2(0.0625f),
                    Position = new Vector2(0.9375f, 0.0625f * i),
                    Colour = Color4.LightGreen,
                    RoundTopRight = tr,
                    RoundTopLeft = false,
                    RoundBottomRight = br,
                    RoundBottomLeft = false,
                    Texture = textures.Get("grass", osu.Framework.Graphics.OpenGL.Textures.WrapMode.Repeat, osu.Framework.Graphics.OpenGL.Textures.WrapMode.Repeat),
                });
            }
            foreach (var rvr in rivers)
            {
                var x = rvr.X * 0.0625f;
                if (rvr.InFlowSide != Side.Left)
                    x += 0.025f;
                var y = rvr.Y * 0.0625f;
                if (rvr.InFlowSide != Side.Top)
                    y += 0.025f;

                container.Add(new RoundedBox
                {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.Both,
                    Size = (rvr.InFlowSide == Side.Left || rvr.InFlowSide == Side.Right) ? new Vector2(0.0375f, 0.0125f) : new Vector2(0.0125f, 0.0375f),
                    Position = new Vector2(x, y),
                    RoundTopLeft = (rvr.InFlowSide == Side.Bottom || rvr.InFlowSide == Side.Right),
                    RoundTopRight = (rvr.InFlowSide == Side.Bottom || rvr.InFlowSide == Side.Left),
                    RoundBottomLeft = (rvr.InFlowSide == Side.Top || rvr.InFlowSide == Side.Right),
                    RoundBottomRight = (rvr.InFlowSide == Side.Top || rvr.InFlowSide == Side.Left),
                    Colour = Color4.DarkCyan
                });

                (int x, int y) prevseg = (0,0);
                bool done = false;
                foreach (var seg in rvr.Segments)
                {
                    if (!done)
                    {
                        prevseg = seg;
                        done = true;
                        continue;
                    }
                    container.Add(new RoundedBox
                    {
                        RelativeSizeAxes = Axes.Both,
                        RelativePositionAxes = Axes.Both,
                        Size = new Vector2((Math.Abs(seg.x - prevseg.x) * 0.0625f) + 0.0125f, (Math.Abs(seg.y - prevseg.y) * 0.0625f) + 0.0125f),
                        Position = new Vector2(Math.Min(seg.x, prevseg.x) * 0.0625f + 0.025f, Math.Min(seg.y, prevseg.y) * 0.0625f + 0.025f),
                        Colour = Color4.DarkCyan
                    });
                    prevseg = seg;
                }
            }
            container.Add(player = new Player(this)
            {
                Position = new Vector2(0.5f)
            });
            InternalChild = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Size = new Vector2(1),
                Children = container
            };
        }

        private bool PieceExists(int x, int y, River? curRiver = null)
        {
            if (rivers.Any(z => z.Segments.Any((a) => a.x == x && a.y == y)) || ((curRiver?.Segments.Any((a) => a.x == x && a.y == y)) ?? false))
                return false;

            if (x >= 1 && x <= 14 && y >= 1 && y <= 14)
                return true;

            if (y == 0)
                return !FieldIsCut2(Side.Top, x - 1);
            if (y == 15)
                return !FieldIsCut2(Side.Bottom, x - 1);
            if (x == 0)
                return !FieldIsCut2(Side.Left, y);
            if (x == 15)
                return !FieldIsCut2(Side.Right, y);

            return false;
        }

        private bool FieldIsCut2(Side side, int offset)
        {
            var tlCut = !(!FieldIsCut(Side.Left, 0) && (!FieldIsCut(Side.Top, 0) || !FieldIsCut(Side.Left, 1)));
            var trCut = !(!FieldIsCut(Side.Right, 0) && (!FieldIsCut(Side.Top, 13) || !FieldIsCut(Side.Right, 1)));
            var blCut = !(!FieldIsCut(Side.Left, 15) && (!FieldIsCut(Side.Bottom, 0) || !FieldIsCut(Side.Left, 14)));
            var brCut = !(!FieldIsCut(Side.Right, 15) && (!FieldIsCut(Side.Bottom, 13) || !FieldIsCut(Side.Right, 14)));
            if (side == Side.Top && offset == -1)
                return tlCut;
            if (side == Side.Top && offset == 14)
                return trCut;
            if (side == Side.Bottom && offset == -1)
                return blCut;
            if (side == Side.Bottom && offset == 14)
                return brCut;
            if (side == Side.Left && offset == 0)
                return tlCut;
            if (side == Side.Left && offset == 15)
                return blCut;
            if (side == Side.Right && offset == 0)
                return trCut;
            if (side == Side.Right && offset == 15)
                return brCut;
            return FieldIsCut(side, offset);
        }

        private bool FieldIsCut(Side side, int offset)
            => cuts.Any(x => x.Side == side && x.Offset <= offset && (x.Offset + x.Length) > offset);

        private Side FromID(int id)
        {
            return id switch
            {
                0 => Side.Top,
                1 => Side.Bottom,
                2 => Side.Left,
                _ => Side.Right
            };
        }

        private (int x, int y) MoveByID(int id, int x, int y)
        {
            return id switch
            {
                0 => (x, y - 1),
                1 => (x, y + 1),
                2 => (x - 1, y),
                _ => (x + 1, y)
            };
        }

        public void OnPlayerMove(bool isInitial = false)
        {
            if (isInitial)
                Position = -(player.Position + ((1.0f / 2.0f) * player.Size)) * ScreenSpaceDrawQuad.Size + ((1.0f / 2.0f) * (ScreenSpaceDrawQuad.Size / 2.5f));
            else
            {
                var hlp = (player.Position + ((1.0f / 2.0f) * player.Size)) * ScreenSpaceDrawQuad.Size + Position;
                var w = Parent.ScreenSpaceDrawQuad.Width;
                var h = Parent.ScreenSpaceDrawQuad.Height;
                if (hlp.X < w * 0.2f)
                {
                    X = -(player.Position.X + ((1.0f / 2.0f) * player.Size.X)) * ScreenSpaceDrawQuad.Size.X + ((1.0f / 5.0f) * (ScreenSpaceDrawQuad.Size.X / 2.5f));
                }
                if (hlp.X > w * 0.8f)
                {
                    X = -(player.Position.X + ((1.0f / 2.0f) * player.Size.X)) * ScreenSpaceDrawQuad.Size.X + ((4.0f / 5.0f) * (ScreenSpaceDrawQuad.Size.X / 2.5f));
                }
                if (hlp.Y < h * 0.2f)
                {
                    Y = -(player.Position.Y + ((1.0f / 2.0f) * player.Size.Y)) * ScreenSpaceDrawQuad.Size.Y + ((1.0f / 5.0f) * h);
                }
                if (hlp.Y > h * 0.8f)
                {
                    Y = -(player.Position.Y + ((1.0f / 2.0f) * player.Size.Y)) * ScreenSpaceDrawQuad.Size.Y + ((4.0f / 5.0f) * h);
                }
            }
        }
    }

    enum Side
    {
        Top,
        Bottom,
        Left,
        Right
    }

    struct Cut
    {
        public Side Side;
        public int Offset;
        public int Length;
    }

    struct River
    {
        public int X, Y;
        public Side InFlowSide;
        public List<(int x, int y)> Segments;
    }
}
