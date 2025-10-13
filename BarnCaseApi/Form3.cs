using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace BarnCaseApi
{   
    public partial class Form3 : Form
    {  
        public string LoggedInUsername { get; set; }

        public Form3()
        {
            InitializeComponent();
            this.Load += Form3_Load;

            txtAge.MaxLength = 2;
            cmbType.MaxLength = 8;
            cmbGender.MaxLength = 6;


        }

        private void Form3_Load(object sender, EventArgs e)
        {
        
            if (!string.IsNullOrEmpty(LoggedInUsername))
            {
                label1.Text = LoggedInUsername;
            }
        }        
    }
}