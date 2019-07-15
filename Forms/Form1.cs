using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog() { CheckFileExists = true })
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                var filePath = dialog.FileName;
                using (var icon = Icon.ExtractAssociatedIcon(filePath))
                {
                    IconPicture.Image = icon.ToBitmap();
                }
            }
        }
    }
}
