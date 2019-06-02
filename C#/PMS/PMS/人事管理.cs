﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace PMS
{
    public partial class 人事管理 : Form
    {
        private List<People> list = null;
        SqlConnection sqlConnection = SetConnection.GetConnection();

        public 人事管理()
        {
            InitializeComponent();
        }

        public object PMSDataSet { get; private set; }

        private void Label1_Click(object sender, EventArgs e)
        {
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void Button3_Click(object sender, EventArgs e) //增加
        {
            String dept = comboBox2.Text.Substring(0, 1);
            String Sno = textBox1.Text.Trim();
            String Sname = textBox2.Text.Trim();
            String Ssex = comboBox1.Text.Trim();
            String Sbirth = textBox4.Text.Trim();
            String Deptno = dept.Trim();
            String Slevel = textBox3.Text.Trim();
            String Stel = textBox5.Text.Trim();
            String Jointime = textBox6.Text.Trim();
            try
            {
                GetData();
                StringBuilder stringBuilder = new StringBuilder("UPDATE STAFF SET ");
                for (int i = 0; i < list.Count; i++)
                {
                    stringBuilder.Append(list[i].name + " = '" + list[i].attr + "'");
                    if (i != list.Count - 1)
                    {
                        stringBuilder.Append(" , ");
                    }
                }

                SqlCommand cmd = new SqlCommand(stringBuilder.ToString(), sqlConnection);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("输入数据违反要求!", "警告");
            }

            sTAFFTableAdapter.Fill(this.pMSDataSet1.STAFF);
        }

        private void 人事管理_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“pMSDataSet1.STAFF”中。您可以根据需要移动或删除它。
            //sTAFFTableAdapter.Fill(this.pMSDataSet1.STAFF);
        }

        private void Button1_Click(object sender, EventArgs e) //查询
        {
            GetData();
            SqlDataReader sqlDataReader = null;
            StringBuilder stringBuilder = new StringBuilder("select * from STAFF where ");
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].attr == "")
                {
                    list.Remove(list[i]);
                }
            }

            if (list.Count == 0)
            {
                MessageBox.Show("请输入信息！");
            }
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    stringBuilder.Append(list[i].name + " = '" + list[i].attr + "'");
                    if (i != list.Count - 1)
                    {
                        stringBuilder.Append(" and ");
                    }
                }

                SqlCommand sqlCommand = new SqlCommand(stringBuilder.ToString(), sqlConnection);
                sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    BindingSource bindingSource = new BindingSource();
                    bindingSource.DataSource = sqlDataReader;
                    stafftable.DataSource = bindingSource;
                }
                else
                {
                    MessageBox.Show("未找到数据！");
                }

                if (sqlDataReader != null)
                {
                    sqlDataReader.Close();
                }
            }
        }

        private void Stafftable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void Button2_Click(object sender, EventArgs e) //修改
        {
            try
            {
                GetData();
                StringBuilder stringBuilder = new StringBuilder("UPDATE STAFF SET ");
                for (int i = 0; i < list.Count; i++)
                {
                    stringBuilder.Append(list[i].name + " = '" + list[i].attr + "'");
                    if (i != list.Count - 1)
                    {
                        stringBuilder.Append(" , ");
                    }
                }

                if (list[0].attr == "" && list[1].attr == "")
                {
                    MessageBox.Show("工号或姓名不能全为空！");
                }
                else
                {
                    stringBuilder.Append("where " + (list[0].attr == ""
                                             ? list[1].name + "='" + list[1].attr + "'"
                                             : list[0].name + "='" + list[0].attr + "'"));
                    SqlCommand cmd = new SqlCommand(stringBuilder.ToString(), sqlConnection);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("修改成功！");
                }
            }
            catch
            {
                MessageBox.Show("输入数据违反要求!", "警告");
            }
        }

        private void Button4_Click(object sender, EventArgs e) //删除(如何实现查询后删除)
        {
            try
            {
                string select_Sno = stafftable.SelectedRows[0].Cells[0].Value.ToString(); //选择的当前行第一列的值，也就是ID
                string delete_by_Sno = "delete from STAFF where Sno=" + select_Sno; //sql删除语句
                SqlCommand cmd = new SqlCommand(delete_by_Sno, sqlConnection);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("请正确选择行!");
            }

            this.sTAFFTableAdapter.Fill(this.pMSDataSet1.STAFF);
        }

        public void GetData()
        {
            list = null;
            list = new List<People>();
            list.Add(new People("Sno", textBox1.Text.Trim()));
            list.Add(new People("Sname", textBox2.Text.Trim()));
            list.Add(new People("Ssex", comboBox1.Text));
            list.Add(new People("Sbirth", textBox3.Text.Trim()));
            list.Add(new People("Deptno", comboBox2.Text));
            list.Add(new People("Slevel", textBox4.Text.Trim()));
            list.Add(new People("Stel", textBox5.Text.Trim()));
            list.Add(new People("JoinTime", textBox6.Text.Trim()));
        }
    }

    public class People
    {
        public People(string name, string attr)
        {
            this.name = name;
            this.attr = attr;
        }

        public string name { set; get; }
        public string attr { set; get; }
    }
}