using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input;

namespace rebank.Game.Elements
{
    public class PasswordTextBox : BasicTextBox, ISuppressKeyEventLogging
    {
        private readonly BindableWithCurrent<bool> currentShow = new BindableWithCurrent<bool>();

        public Bindable<bool> ShowPassword
        {
            get => currentShow.Current;
            set => currentShow.Current = value;
        }

        protected virtual char MaskCharacter => '*';

        protected override bool AllowClipboardExport => false;

        protected override bool AllowWordNavigation => false;

        protected override Drawable AddCharacterToFlow(char c) => base.AddCharacterToFlow(ShowPassword.Value ? c : MaskCharacter);

        [BackgroundDependencyLoader]
        private void load()
        {
            ShowPassword.ValueChanged += showPassword_ValueChanged;
        }

        private void showPassword_ValueChanged(ValueChangedEvent<bool> obj)
        {
            TextFlow.RemoveAll(x => true);
            foreach (char c in Text)
                AddCharacterToFlow(c);
        }
    }
}
