using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerDataTool
{
    public partial class AccountsDialog : Form
    {
        public AccountsDialog()
        {
            InitializeComponent();

            // Fetch accounts from database
            DBAccountRow[] accounts = Database.FetchAccounts();
            foreach (DBAccountRow row in accounts)
            {
                AddAccount(row);
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            CreateAccountDialog dlg = new CreateAccountDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                // Add this account to the database
                DBAccountRow accountRow = Database.AddAccount(dlg.tbAccount.Text, dlg.tbPassword.Text);

                // Add it to the list
                AddAccount(accountRow);
            }
        }

        void AddAccount(DBAccountRow accountRow)
        {
            ListViewItem lvi = lvAccounts.Items.Add(accountRow.ID.ToString());
            lvi.SubItems.Add(accountRow.UserName);
            lvi.SubItems.Add(accountRow.Password);
        }
    }
}
