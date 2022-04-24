using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;

namespace rebank.Game.Elements
{
    public class BindableNumberText : SpriteText
    {
        public readonly Bindable<int> Number = new Bindable<int>();

        public BindableNumberText()
        {
            Number.ValueChanged += number_ValueChanged;
        }

        private void number_ValueChanged(ValueChangedEvent<int> obj)
        {
            Text = obj.NewValue.ToString();
        }
    }
}
