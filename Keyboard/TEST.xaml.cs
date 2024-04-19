using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Keyboard
{
    /// <summary>
    /// Lógica de interacción para TEST.xaml
    /// </summary>
    public partial class TEST : UserControl
    {
        private KeyboardComponent keyboard = new KeyboardComponent();

        public TEST()
        {

            InitializeComponent();

            keyboard.InitializeKeyboard(this);
        }



    }
}
