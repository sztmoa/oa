using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace SMT.Portal.Common.SmtForm.Framework
{
    public class Constant
    {
        public enum CtrlType
        {
            TextBox = 1,            
            TextArea = 2,
            Label = 3,
            DropDownList = 4,
            ListBox = 5,
            CheckBoxList = 6,
            RadioButtonList = 7,
            CheckBox = 8,
            RadioButton = 9,
            DateTextBox = 10,
            DoubleTextBox = 11,
            IntTextBox = 12,
            DecimalTextBox = 13,
            RichText = 14, 
            UserControl = 15,
            Rtf = 16,
            Link = 17,
        }

        public static string r
        {
            get
            {
                return ConfigurationManager.AppSettings["r"];
            }
        }
    }
}