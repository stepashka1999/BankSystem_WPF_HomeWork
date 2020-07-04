using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace PPBank
{
    class Log:INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private string text;
        public string Text { get => text; set { text = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text))); } }

        public override string ToString()
        {
            return text;
        }
    }
}
