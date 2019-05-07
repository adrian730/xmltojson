using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace XMLToJSNOConverter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Button selectButton;
        private OpenFileDialog openFileDialog1;
        private string filePath;

        List<string> allowedNodeName = new List<string>() { "obj_name", "field", "name", "type", "value" };

        private void btnConvert_Click(object sender, EventArgs e)
        {
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "object";
            xRoot.IsNullable = true;

            XmlSerializer serializer = new XmlSerializer(typeof(ObjectClass), xRoot);

            ObjectClass obj;
            List<ObjectClass> listObj = new List<ObjectClass>();

            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                using (XmlReader reader = XmlReader.Create(filePath))
                {
                    while (reader.Read())
                    {
                        if(reader.Name == "object")
                        {
                            try
                            {
                                obj = (ObjectClass)serializer.Deserialize(reader);

                                listObj.Add(obj);
                            }
                            catch(Exception ex)
                            {
                                continue;
                            }
                        }                          
                    }
                }

                List<ObjectClass> validatedList = ValidateObjects(listObj);

                string output = JsonConvert.SerializeObject(validatedList);
                txtJSON.Text = output;
            }
        }

        private List<ObjectClass> ValidateObjects(List<ObjectClass> listObj)
        {
            List<ObjectClass> result = new List<ObjectClass>();

            foreach (ObjectClass obj in listObj)
            {
                if (obj.fields.Count() == 0 || string.IsNullOrEmpty(obj.obj_name))
                    continue;

                bool isValid = true;

                foreach (FieldClass field in obj.fields)
                {
                    if (string.IsNullOrEmpty(field.name) ||
                       string.IsNullOrEmpty(field.type) ||
                       string.IsNullOrEmpty(field.value) ||
                       (field.type != "string" && field.type != "int")
                      )
                    {
                        isValid = false;
                        break;
                    }
                }

                if (!isValid)
                    continue;

                result.Add(obj);
            }

            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Environment.CurrentDirectory;

            if(openFileDialog1.ShowDialog() == DialogResult.OK)
                SetText(openFileDialog1.FileName);
        }

        private void SetText(string text)
        {
            filePath = text;
            label3.Text = string.Format("Scieżka do pliku: {0}", text);
        }
    }
}
