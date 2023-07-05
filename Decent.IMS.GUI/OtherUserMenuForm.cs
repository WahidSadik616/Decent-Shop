using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Decent.IMS.GUI
{
    public partial class OtherUserMenuForm : MetroFramework.Forms.MetroForm
    {
        public OtherUserMenuForm()
        {
            InitializeComponent();
        }

        private void OtherUserForm_Load(object sender, EventArgs e)
        {
            //this.KeyPreview = true;
            
            btnProduct.Select();
        }

        
    }
}
