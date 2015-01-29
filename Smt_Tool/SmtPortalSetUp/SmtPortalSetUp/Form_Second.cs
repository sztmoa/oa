using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace SmtPortalSetUp
{
    public partial class Form_Second : Form
    {
        /// <summary>
        /// 安装源路径
        /// </summary>
        private string SourcePath = string.Empty;
        /// <summary>
        /// 安装路径
        /// </summary>
        private string setUpPath = string.Empty;

        public Form_Second()
        {
            InitializeComponent();
        }

        private void Form_Second_Load(object sender, EventArgs e)
        {
            SourcePath = Application.StartupPath + "\\SMTOnlinePortal";
            txtSourcePath.Text = SourcePath;
            //txtSourcePath.ReadOnly = true;
            //txtFilePath.ReadOnly = true;

            setUpPath = @"C:\Users\Administrator\Desktop\publish";
            txtFilePath.Text = setUpPath;

            //txtConnectStringOld.Text = @"data source=smtsaasdb;User Id=smthrm;Password=oracle";
            //txtConnectString.Text = @"data source=smtsaasdb;User Id=prreits;Password=oracle";

            //txtURLOld.Text = @"172.16.1.57";
            //txtURL.Text = "portal.smt-online.net";
            //TxtOld.Text = "172.30.50.20,172.20.30.105,172.30.60.55";
            Utility.progreesValue = 0;

            labelAlet.Text = @"如果是从其他文件夹拷贝安装，文件名包含有“.txt .log .bak 备份 复制” 
的文件将会被剔除掉，文件夹名包含有“备份 复制 ErrorLog back bak log upload”的文件夹也将会被剔除掉，请注意。";
        }

        private void btnSelectPath_Click(object sender, EventArgs e)
        {
            string filepath = "";
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = "请选择IIS的根目录";//描述弹出框功能 
            //folderBrowser.RootFolder = Environment.SpecialFolder.MyDocuments;　// 打开到我的文档 
            folderBrowser.ShowDialog();　// 打开目录选择对话框 
            filepath = folderBrowser.SelectedPath;　// 返回用户选择的目录地址            
            txtFilePath.Text = filepath;
            setUpPath = filepath;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(txtConnectString.Text)
            //    || string.IsNullOrEmpty(txtFilePath.Text))
            //{
            //    MessageBox.Show("请输入完整的安装路径及数据库连接字符串！");
            //    return;
            //}
            DialogResult MsgBoxResult;//设置对话框的返回值
            MsgBoxResult = MessageBox.Show("请确认需要替换的已正确填入，确认开始？",//对话框的显示内容 

            "提示",//对话框的标题 
            MessageBoxButtons.YesNo,//定义对话框的按钮，这里定义了YSE和NO两个按钮
            MessageBoxIcon.Exclamation,//定义对话框内的图表式样，这里是一个黄色三角型内加一个感叹号 
            MessageBoxDefaultButton.Button2);//定义对话框的按钮式样
            if (MsgBoxResult == DialogResult.Yes)//如果对话框的返回值是YES（按"Y"按钮）
            {
                //Directory.SetCurrentDirectory(Directory.GetParent(binPath).FullName);
                //binPath = Directory.GetCurrentDirectory();
                try
                {
                    SourcePath = txtSourcePath.Text;
                    setUpPath = txtFilePath.Text;
                    string[] files = System.IO.Directory.GetFiles(SourcePath, "*", System.IO.SearchOption.AllDirectories);
                    progressBar.Maximum = files.Length;
                    Utility.MaxProgreesValue = progressBar.Maximum;
                    Utility.from = this;
                    Thread t = new Thread(new ThreadStart(startCopy));
                    t.Start();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
            }
        }

        Dictionary<string, string> replacePra = new Dictionary<string, string>();
        private void startCopy()
        {

            //Utility.DeleteFile(setUpPath);
            try
            {
                Utility.CopyDirectory(SourcePath, setUpPath, replacePra);
                MessageBox.Show("安装完毕！");
                //Utility.EditDirectoryFiles(setUpPath, replacePra);
            }
            catch (Exception ex)
            {
                ShowMessage("安装文件异常：" + ex.ToString());
                ShowProgress(0);
                MessageBox.Show("安装文件异常，请查看错误日志！");

            }
        }

        private void btnSourcePath_Click(object sender, EventArgs e)
        {
            string filepath = "";
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = "请选择安装原始文件的目录";//描述弹出框功能 
            //folderBrowser.RootFolder = Environment.SpecialFolder.MyDocuments;　// 打开到我的文档 
            folderBrowser.ShowDialog();　// 打开目录选择对话框 
            filepath = folderBrowser.SelectedPath;　// 返回用户选择的目录地址  

            txtSourcePath.Text = filepath;
            SourcePath = filepath;

        }

        delegate void DelShow(String Msg); //代理
        //将对控件的操作写到一个函数中 
        public void ShowMessage(String para)
        {
            if (!txtMessagebox.InvokeRequired) //不需要唤醒，就是创建控件的线程
            //如果是创建控件的线程，直接正常操作 
            {
                txtMessagebox.Text = DateTime.Now.ToLongTimeString() +" "+ para + System.Environment.NewLine + txtMessagebox.Text;
            }
            else //非创建线程，用代理进行操作
            {
                DelShow ds = new DelShow(ShowMessage);
                //唤醒主线程，可以传递参数，也可以为null，即不传参数
                Invoke(ds, new object[] { para });
            }
        }


        delegate void DelShowProgress(int Msg); //代理
        //将对控件的操作写到一个函数中 
        public void ShowProgress(int para)
        {
            if(para>=progressBar.Maximum)
            {
                para = progressBar.Maximum;
            }
            if (!progressBar.InvokeRequired) //不需要唤醒，就是创建控件的线程
            //如果是创建控件的线程，直接正常操作 
            {
                progressBar.Value = para;
            }
            else //非创建线程，用代理进行操作
            {
                DelShowProgress ds = new DelShowProgress(ShowProgress);
                //唤醒主线程，可以传递参数，也可以为null，即不传参数
                Invoke(ds, new object[] { para });
            }
        }



        delegate void DelMaxProgress(int Msg); //代理
        //将对控件的操作写到一个函数中 
        public void SetProgressMaxValue(int para)
        {
            if (!progressBar.InvokeRequired) //不需要唤醒，就是创建控件的线程
            //如果是创建控件的线程，直接正常操作 
            {
                progressBar.Maximum = para;
            }
            else //非创建线程，用代理进行操作
            {
                DelMaxProgress ds = new DelMaxProgress(SetProgressMaxValue);
                //唤醒主线程，可以传递参数，也可以为null，即不传参数
                Invoke(ds, new object[] { para });
            }
        }

        private void txtMessagebox_DoubleClick(object sender, EventArgs e)
        {
            txtMessagebox.SelectAll();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string oldString = TxtOld.Text;
            string newString=txtNew.Text;
            replacePra.Add(oldString, newString);

            this.listOld.Items.Add(oldString);
            this.listNew.Items.Add(newString);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.listOld.Items.Clear();
            this.listNew.Items.Clear();
            replacePra.Clear();
        }
    }

    
}
