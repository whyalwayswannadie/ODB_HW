using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
namespace HomeWORKODB
{
    public partial class Form1 : Form

    {

        Broker model = new Broker();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            PopulateDataGridView();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Clear(); 
        }
        void Clear()
        {
            textBoxName.Text = textBoxLastName.Text = textBoxBrokageCompany.Text = "";
            buttonDelete.Enabled = true;
            model.BrokerID = 0;
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
           
            model.FirstName = textBoxName.Text.Trim();
            model.LastName = textBoxLastName.Text.Trim();

            model.BrokageCompany = textBoxBrokageCompany.Text.Trim();

            model.FirstName = textBoxName.Text.Trim();


            using (HWDBEntities1 db = new HWDBEntities1())
            {
                if(model.BrokerID == 0)//Insert
                {
                    db.Broker.Add(model);

                }
                else//update
                
               db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            Clear();
            PopulateDataGridView();
            MessageBox.Show("Submitted Successfully");
        }
        void PopulateDataGridView()
        {
            using (HWDBEntities1 db = new HWDBEntities1())
            {
                dataGridView1.DataSource = db.Broker.ToList<Broker>();
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow.Index != -1)
            {
                model.BrokerID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["BrokerID"].Value);
                   using(HWDBEntities1 db = new HWDBEntities1())
                {
                    model = db.Broker.Where(x => x.BrokerID == model.BrokerID).FirstOrDefault();
                    textBoxName.Text = model.FirstName;
                    textBoxLastName.Text = model.LastName;

                    textBoxBrokageCompany.Text = model.BrokageCompany;

                }
     
                buttonDelete.Text = "Delete";  
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

    

        private void buttonDelete_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to delete this record?",
              "EF CRUD OPERATION", MessageBoxButtons.YesNo) ==
              DialogResult.Yes)
            {
                using (HWDBEntities1 db = new HWDBEntities1())
                {
                    var entry = db.Entry(model);
                    if (entry.State == System.Data.Entity.EntityState.Detached)
                        db.Broker.Attach(model);
                    db.Broker.Remove(model);
                    db.SaveChanges();
                    PopulateDataGridView();
                    Clear();
                    MessageBox.Show("Deleted successfully!");
                }
            }
        }
    }
}
