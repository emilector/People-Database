using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;

namespace Database
{
    public partial class SearchData : Form
    {
        public SearchData()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // display selected object data

            if (listBox1.SelectedItem != null)
            {
                AddData addData = new AddData(false);
                addData.Show();

                List<string> paraÜbergabe = new List<string>();

                XDocument doc = XDocument.Load(Directory.GetCurrentDirectory() + "/objectData/doc.xml");

                string searchedObject = listBox1.SelectedItem.ToString();
                string[] split = searchedObject.Split(new Char[] { ' ' });

                var allObjects = doc.Root.Elements();

                string target = "";

                foreach (var obj in allObjects)
                {
                    if (obj.Element("Family-name").Value == split[0] && obj.Element("Given-name").Value == split[1])
                        target = obj.Name.ToString();
                }

                var parameters = doc.Root.Element(target).Elements();

                foreach (var parameter in parameters)
                {
                    paraÜbergabe.Add(parameter.Value.ToString());
                }

                addData.fillData(paraÜbergabe, target);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // collect all keywords

            bool displayAll = false;
            List<string> keywords = new List<string>();

            foreach(var item in checkedListBox1.CheckedItems)
            {
                keywords.Add(item.ToString());

                if (item.ToString() == "ALL")
                    displayAll = true;
            }

            // collect all criteria fitting objects

            try
            {
                XDocument doc = XDocument.Load(Directory.GetCurrentDirectory() + "/objectData/doc.xml");

                listBox1.Items.Clear();
                progressBar1.Value = 0;
                progressBar1.Value = 100;

                var objects = doc.Root.Elements();

                if (!displayAll)
                {
                    foreach (var obj in objects)
                    {
                        bool hasKeyword = false;

                        foreach (var parameter in obj.Elements())
                        {
                            foreach (var keyword in keywords)
                            {
                                if (parameter.Value.ToLower().Contains(keyword.ToLower()))
                                {
                                    hasKeyword = true;
                                    break;
                                }

                                if (hasKeyword)
                                    break;
                            }

                            if (hasKeyword)
                                break;
                        }

                        // fill listbox

                        if (hasKeyword)
                            listBox1.Items.Add(obj.Element("Family-name").Value + " " + obj.Element("Given-name").Value);
                    }
                }
                else
                {
                    foreach (var obj in objects)
                    {
                        listBox1.Items.Add(obj.Element("Family-name").Value + " " + obj.Element("Given-name").Value);
                    }
                }

                checkedListBox1.Items.Clear();
            }

            catch
            {
                MessageBox.Show("No objects in Database!");
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            // add keyword to search

            if (e.KeyCode == Keys.Enter)
            {
                checkedListBox1.Items.Add(textBox1.Text);
                checkedListBox1.SetItemChecked(checkedListBox1.Items.Count - 1, true);
                textBox1.Clear();
            }
        }
    }
}
