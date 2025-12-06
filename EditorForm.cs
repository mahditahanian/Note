using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TrayMementoApp;

namespace NoteApp
{
    public partial class EditorForm : Form
    {
        private readonly TextBox _textBox;
        private readonly ContextMenuStrip _contextMenu;

        private readonly MementoStack<TextMemento> _history = new MementoStack<TextMemento>();
        private bool _internalChange = false;
        private string _lastText = string.Empty;

        public EditorForm()
        {
            InitializeComponent();

            Text = "Note";
            Width = 600;
            Height = 400;
            

            _textBox = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.Both,
                AcceptsReturn = true,
                AcceptsTab = true,
                WordWrap = false
            };

            _contextMenu = new ContextMenuStrip();
            _contextMenu.Items.Add("Undo ", null, OnUndoClick);
            _contextMenu.Items.Add(new ToolStripSeparator());
            _contextMenu.Items.Add("Font...", null, OnFontClick);
            _contextMenu.Items.Add(new ToolStripSeparator());
            _contextMenu.Items.Add("Save...", null, OnSaveClick);

            _textBox.ContextMenuStrip = _contextMenu;

            Controls.Add(_textBox);

            _textBox.TextChanged += TextBox_TextChanged;

            PushState(); 
        }

       

        private void TextBox_TextChanged(object? sender, EventArgs e)
        {
            if (_internalChange)
                return;

            var previousText = _lastText;
            _lastText = _textBox.Text;

            _history.Push(new TextMemento(
                previousText,
                _textBox.SelectionStart,
                _textBox.SelectionLength));
        }

        private void PushState()
        {
            _lastText = _textBox.Text;
            _history.Push(new TextMemento(
                _textBox.Text,
                _textBox.SelectionStart,
                _textBox.SelectionLength));
        }

        private void RestoreState(TextMemento m)
        {
            _internalChange = true;
            _textBox.Text = m.Text;

            try
            {
                _textBox.SelectionStart = Math.Min(m.SelectionStart, _textBox.Text.Length);
                _textBox.SelectionLength = Math.Min(
                    m.SelectionLength,
                    Math.Max(0, _textBox.Text.Length - _textBox.SelectionStart));
            }
            catch
            {
               
            }

            _internalChange = false;
            _lastText = _textBox.Text;
        }

       

        private void OnUndoClick(object? sender, EventArgs e)
        {
            if (_history.Count == 0)
                return;

            var current = _history.Pop();
            if (_history.Count == 0)
            {
                RestoreState(current);
                return;
            }

            var previous = _history.Peek();
            RestoreState(previous);
        }

        private void OnFontClick(object? sender, EventArgs e)
        {
            using var fontDlg = new FontDialog
            {
                Font = _textBox.Font,
                ShowColor = false
            };

            if (fontDlg.ShowDialog(this) == DialogResult.OK)
            {
                _textBox.Font = fontDlg.Font;
            }
        }

        private void OnSaveClick(object? sender, EventArgs e)
        {
            using var dlg = new SaveFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                DefaultExt = "txt"
            };

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                File.WriteAllText(dlg.FileName, _textBox.Text);
            }
        }
    }

   
}
