using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace TrayMementoApp
{
    public class TextMemento
    {
        public string Text { get; }
        public int SelectionStart { get; }
        public int SelectionLength { get; }

        public TextMemento(string text, int selectionStart, int selectionLength)
        {
            Text = text;
            SelectionStart = selectionStart;
            SelectionLength = selectionLength;
        }
    }
}
