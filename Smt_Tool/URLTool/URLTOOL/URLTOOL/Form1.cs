using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace URLTOOL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private int indexNum = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            indexNum = 0;
            string yearStr = DateTime.Now.Year.ToString()+"-";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(openFileDialog1.FileName);
                //MessageBox.Show(sr.ReadToEnd());


                List<URLString> listAll = new List<URLString>();
                while (!sr.EndOfStream)
                {

                    URLString obj = new URLString();
                    string valuer = sr.ReadLine();
                    if (valuer.Contains(yearStr))
                    {
                        int start = valuer.IndexOf(yearStr);
                        string k = valuer.Substring(start, 19);
                        obj.DataTimeString = k;
                        if(!valuer.Contains("访问网址"))
                        {
                            string valuer2 = sr.ReadLine();
                            valuer = valuer + valuer2;
                        }

                        if (valuer.Contains("访问网址:"))
                        {
                            string[] urlall = valuer.Split('问');
                            if (urlall.Length > 1)
                            {
                                string url = urlall[1];
                                string strk = url.Substring(url.Length - 1, 1);
                                if (strk == ".")
                                {
                                    url = url.Substring(0, url.Length - 1);
                                    url = url.Substring(3);
                                    if(url.Contains(".js")
                                        ||url.Contains(".css"))
                                    {
                                        continue;
                                    }
                                    obj.URLAdress = url;
                                    indexNum++;
                                    obj.Index = indexNum;
                                    listAll.Add(obj);
                                }
                                else
                                {
                                    string valuer2 = sr.ReadLine();
                                    url = url + valuer2;
                                    url = url.Substring(0, url.Length - 1);
                                    if (url.Contains("网址"))
                                    {
                                        url = url.Substring(3);
                                    }
                                    obj.URLAdress = url;
                                    indexNum++;
                                    obj.Index = indexNum;
                                    listAll.Add(obj);
                                }
                            }
                        }
                    }
                   
                }
                dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.DataSource = listAll;
                sr.Close();
            }
        }

        public class URLString
        {
            private int index;
            private string dtstr;
            private string urlstr;
            
            public int Index
            {
                get { return index; }
                set { index = value; }
            }
            public string DataTimeString
            {
                get { return dtstr; }
                set { dtstr = value; }
            }
            public string URLAdress
            {
                get { return urlstr; }
                set { urlstr = value; }
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dataGridView1.CurrentRow!=null)
            {
                webBrowser1.Stop();
                string urlstring = this.dataGridView1.CurrentRow.Cells["URLAdress"].Value.ToString();


                string urlstr=HttpUtility.UrlDecode(urlstring);
                txtUrl.Text = urlstr;
                webBrowser1.Navigate(urlstring);
                labelMsg.Text = "开始加载中......";
            }

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            labelMsg.Text = "已加载完成！";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           //webBrowser1.Document.Encoding = Encoding.UTF8.EncodingName;
            webBrowser1.ScriptErrorsSuppressed = true;
        }
    }
}
