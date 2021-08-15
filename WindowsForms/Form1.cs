using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsForms.Pagination;

namespace WindowsForms
{
    public partial class Form1 : Form
    {
        List<SampleClass> DataList;
        int PageNumer = 1;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataList = SampleData.GetData();
            for (int i = 1; i <= 100; i++)
            {
                comboBox1.Items.Add(i);
            }
            comboBox1.SelectedIndex = 0;
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(((ComboBox)sender).Text, out PageNumer)) PageNumer = 1;
            dataGridView1.DataSource = PagedList<SampleClass>.ToPagedList(DataList, PageNumer, 10);
        }
    }
}
