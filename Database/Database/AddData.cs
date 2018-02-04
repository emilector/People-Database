using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using System.Globalization;

namespace Database
{
    public partial class AddData : Form
    {
        public AddData(bool isAdd)
        {
            InitializeComponent();

            p_isAdd = isAdd;

            // set current date and time

            DateTime localDate = DateTime.Now;
            var culture = new CultureInfo("de-DE");
            textBox18.Text = localDate.ToString(culture);

            // Find out object number

            if (isAdd)
            {
                try
                {
                    XDocument doc = XDocument.Load(Directory.GetCurrentDirectory() + "/objectData/doc.xml");
                    int count = doc.Root.Elements().Count();
                    label24.Text = "Object number: " + count;
                }
                catch
                {
                    label24.Text = "0";
                }
            }

            p_processData = new ProcessData();

            // put textboxes in a list

            p_boxes.Add(textBox1);
            p_boxes.Add(textBox2);
            p_boxes.Add(textBox3);
            p_boxes.Add(textBox4);
            p_boxes.Add(textBox5);
            p_boxes.Add(textBox6);
            p_boxes.Add(textBox7);
            p_boxes.Add(textBox8);
            p_boxes.Add(textBox9);
            p_boxes.Add(textBox10);
            p_boxes.Add(textBox11);
            p_boxes.Add(textBox12);
            p_boxes.Add(textBox13);
            p_boxes.Add(textBox14);
            p_boxes.Add(textBox15);
            p_boxes.Add(textBox16);
            p_boxes.Add(textBox17);
            p_boxes.Add(textBox18);
        }

        bool p_isAdd = true;
        string p_dataFolder = Directory.GetCurrentDirectory() + "/objectData";

        List<TextBox> p_boxes = new List<TextBox>();
        string p_currentPic = "";
        ProcessData p_processData;

        private void button1_Click(object sender, EventArgs e)
        {
            if (!p_isAdd)
            {
                // delete object and replace it with new one

                XDocument doc = XDocument.Load(p_dataFolder + "/doc.xml");
                string obj = "obj" + label24.Text.Remove(0, 15);

                doc.Root.Element(obj).Remove();
                doc.Save(p_dataFolder + "/doc.xml");

                addObject(obj);

                this.Close();
            }
            else
            {
                // Check if name exists already

                XDocument doc = XDocument.Load(p_dataFolder + "/doc.xml");
                var elements = doc.Root.Elements();

                bool isOK = true;

                foreach (var el in elements)
                {
                    if (el.Element("Family-name").Value == textBox1.Text && el.Element("Given-name").Value == textBox2.Text)
                    {
                        isOK = false;
                    }
                }

                if (isOK)
                {
                    // Add object

                    string obj = "-1";
                    addObject(obj);
                }
                else
                    MessageBox.Show("Object with this name already exists!");
            }
        }

        void addObject(string objName)
        { 
                // collect parameters and pass to process data class

                string picturePath = p_dataFolder + "/" + textBox1.Text + " " + textBox2.Text + ".jpeg";

                List<string> objectData = new List<string>();

                foreach (TextBox box in p_boxes)
                {
                    objectData.Add(box.Text);
                }

                p_processData.addObject(objectData, objName);

                // copy, and move picture to data folder

                if (p_currentPic != "")
                {
                    if (textBox1.Text != "" && textBox2.Text != "")
                    {
                        if (Directory.Exists(p_dataFolder))
                        {
                            if (File.Exists(picturePath))
                                File.Delete(picturePath);

                            File.Copy(p_currentPic, picturePath);
                        }
                        else
                        {
                            Directory.CreateDirectory(p_dataFolder);

                            if (File.Exists(picturePath))
                                File.Delete(picturePath);

                            File.Copy(p_currentPic, picturePath);
                        }

                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Please enter Family-name and Given-name!");
                    }
                }
                else
                    this.Close();
        }

        public void fillData(List<string> parameters, string objectName)
        {
            // fill textboxes for searched object 

            int counter = 0;

            foreach(string para in parameters)
            {
                p_boxes[counter].Text = para;

                counter++;
            }

            // fill picture

            try
            {
                pictureBox1.BackgroundImage = Image.FromFile(p_dataFolder  + "/" + parameters[0] + " " + parameters[1] + ".jpeg");
            }
            catch
            {}

            // display object number

            label24.Text = "Object number: " + objectName.Remove(0, 3);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // select object picture

            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string path = dialog.FileName;
                p_currentPic = path;
                pictureBox1.BackgroundImage = Image.FromFile(path);
            }

            label23.Visible = false;
        }

        private void label23_Click(object sender, EventArgs e)
        {
            // traffic transfer

            pictureBox1_Click(sender, e);
        }
    }
}
