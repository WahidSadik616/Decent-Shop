using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Decent.IMS.Data;
using Decent.IMS.Framework;

namespace Decent.IMS.GUI
{
    public partial class ConnectionTestForm : MetroFramework.Forms.MetroForm
    {
        public ConnectionTestForm()
        {
            InitializeComponent();
        }

        private void ConnectionTestForm_Load(object sender, EventArgs e)
        {
            
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection connection = new SqlConnection("Data Source=DESKTOP-TALBLJ3;Initial Catalog=LibraryDb;Integrated Security=True");
                connection.Open();
                string query = "Insert into UserInfo(Name)" +
                                "values('" + txtName.Text + "')";
                SqlCommand command = new SqlCommand(query, connection);
                int row = command.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                MetroFramework.MetroMessageBox.Show(this, exception.Message);
            }
        }
    }

   
}
