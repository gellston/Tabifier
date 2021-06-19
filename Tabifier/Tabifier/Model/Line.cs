using System;
using System.Collections.Generic;
using System.Text;

namespace Tabifier.Model
{
    public class Line : PropertyChangedBase
    {
        public Line()
        {

        }

        public int _Number = 0;
        public int Number
        {
            get => _Number;
            set => Set(ref _Number, value);
        }

        public string _Text = "";
        public string Text
        {
            get => _Text;
            set => Set(ref _Text, value);
        }

        private bool _IsCorrect = false;
        public bool IsCorrect
        {
            get => _IsCorrect;
            set => Set(ref _IsCorrect, value);
        }
    }
}
