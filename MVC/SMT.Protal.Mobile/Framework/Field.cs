using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Collections;
using System.Drawing;
using System.Data;

namespace SMT.Portal.Common.SmtForm.Framework
{
    public class Field
    {

        public Field()
        {
            _prefix = "ctrl_";
        }

        public Field(string prefix)
        {
            _prefix = prefix;
        }


        public Field(string fieldName, string captionName, Constant.CtrlType controlType, bool isNeed, int fieldType)
        {
            _fieldName = fieldName;
            _captionName = captionName;
            _controlType = controlType;
            _isNeed = isNeed;
            _fieldType = fieldType;
        }

        public Field(string fieldName, string captionName, Constant.CtrlType controlType, bool isNeed)
        {
            _fieldName = fieldName;
            _captionName = captionName;
            _controlType = controlType;
            _isNeed = isNeed;
        }

        public Field(string fieldName, string captionName, Constant.CtrlType controlType, bool isNeed, double min, double max)
        {
            _fieldName = fieldName;
            _captionName = captionName;
            _controlType = controlType;
            _isNeed = isNeed;
            _minValue = min;
            _maxValue = max;
        }
        /// <summary>
        /// 缩放
        /// </summary>
        private decimal _zoom = 1;
        public decimal Zoom
        {
            get
            {
                return _zoom;
            }
            set
            {
                _zoom = value;
            }
        }

        private string cssName = "";
        public string CssName
        {
            get { return cssName; }
            set { cssName = value; }
        }

        private int _colspan = 1;
        public int Colspan
        {
            get
            {
                return _colspan;
            }
            set
            {
                _colspan = value;
            }
        }

        private string _columnOrder ;
        public string ColumnOrder
        {
            get
            {
                return _columnOrder;
            }
            set
            {
                _columnOrder = value;
            }
        }
        private string _prefix = "";

        public string Prefix
        {
            get
            {
                return _prefix;
            }
            set
            {
                _prefix = value;
            }
        }

        private int _width = 200;
        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }




        private string _fieldName;
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName
        {
            get
            {
                return _fieldName;
            }
            set
            {
                _fieldName = value;
            }
        }


        private int _fieldType = 1;
        /// <summary>
        /// 如果为1，则表明为系统处理字段，否则，根据类型自行处理
        /// </summary>
        public int FieldType
        {
            get
            {
                return _fieldType;
            }
            set
            {
                _fieldType = value;
            }
        }




        private string _typeFullName;
        /// <summary>
        /// 字段所属类型的完整名称
        /// </summary>
        public string TypeFullName
        {
            get
            {
                return _typeFullName;
            }
            set
            {
                _typeFullName = value;
            }
        }

        private string _captionName;
        /// <summary>
        /// 标题
        /// </summary>
        public string CaptionName
        {
            get
            {
                return _captionName;
            }
            set
            {
                _captionName = value;
            }
        }
        private bool _isShowCaption;
        /// <summary>
        /// 是否显示标题
        /// </summary>
        public bool IsShowCaption
        {
            get
            {
                return _isShowCaption;
            }
            set
            {
                _isShowCaption = value;
            }
        }


        private string _description;
        /// <summary>
        /// 标题
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        private string _param = "";
        //控件可能用到的参数
        public string Param
        {
            get
            {
                return _param;
            }
            set
            {
                _param = value;
            }
        }

        private double? _minValue = null;
        public double? MinValue
        {
            get
            {
                return _minValue;
            }
            set
            {
                _minValue = value;
            }
        }

        private double? _maxValue = null;
        public double? MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                _maxValue = value;
            }
        }

        private Constant.CtrlType _controlType;
        /// <summary>
        /// 控件类型
        /// </summary>
        public Constant.CtrlType ControlType
        {
            get
            {
                return _controlType;
            }
            set
            {
                _controlType = value;
            }
        }


        private SqlDbType _dataType;
        /// <summary>
        /// 数据类型
        /// </summary>
        public SqlDbType DataType
        {
            get
            {
                return _dataType;
            }
            set
            {
                _dataType = value;
            }
        }

        private int _orderNo;
        /// <summary>
        /// 排序字段
        /// </summary>
        public int OrderNo
        {
            get
            {
                return _orderNo;
            }
            set
            {
                _orderNo = value;
            }
        }


        private bool _isNeed;
        /// <summary>
        /// 是否必填
        /// </summary>
        public bool IsNeed
        {
            get
            {
                return _isNeed;
            }
            set
            {
                _isNeed = value;
            }
        }
        private bool _isDisplayNeed = true;
        /// <summary>
        /// 是否必填
        /// </summary>
        public bool IsDisplayNeed
        {
            get
            {
                return _isDisplayNeed;
            }
            set
            {
                _isDisplayNeed = value;
            }
        }

        /// <summary>
        /// 是否显示限制参数
        /// </summary>
        private bool _isDisplayDes = true;

        public bool IsDisplayDes
        {
            get
            {
                return _isDisplayDes;
            }
            set
            {
                _isDisplayDes = value;
            }
        }

        private string _maxLength;
        /// <summary>
        /// 字符长度
        /// </summary>
        public string MaxLength
        {
            get
            {
                return _maxLength;
            }
            set
            {
                _maxLength = value;
            }
        }

        private int _dictID;
        /// <summary>
        /// 数据字典项目ID
        /// </summary>
        public int DictID
        {
            get
            {
                return _dictID;
            }
            set
            {
                _dictID = value;
            }
        }


        private string _defaultValue;
        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue
        {
            get
            {
                return _defaultValue;
            }
            set
            {
                _defaultValue = value;
            }
        }

        private Label _captionControl;
        /// <summary>
        /// 标题控件
        /// </summary>
        public Label CaptionControl
        {

            get
            {
                if (_captionControl == null)
                {
                    _captionControl = new Label();
                    _captionControl.ID = this.Prefix + "_caption" + this.FieldName;
                    _captionControl.Text = this.CaptionName + "";
                }
                return _captionControl;
            }
            set
            {
                _captionControl = value;
            }
        }


        private ArrayList _mainControl;
        /// <summary>
        /// 主控件
        /// </summary>
        public ArrayList MainControl
        {
            get
            {
                if (_mainControl == null)
                {
                    _mainControl = CreateControl();
                }
                return _mainControl;
            }
            set
            {
                _mainControl = value;
            }
        }


        private Label _descriptionControl;
        /// <summary>
        /// 标题控件
        /// </summary>
        public Label DescriptionControl
        {
            get
            {
                if (_descriptionControl == null)
                {
                    _descriptionControl = new Label();
                    _descriptionControl.ID = this.Prefix + "_description" + this.FieldName;
                    _descriptionControl.Text = this.Description;
                    _descriptionControl.ForeColor = Color.FromName("#999999");
                    string range = "";
                    if (IsDisplayDes)
                    {
                        if (this.MinValue != null)
                        {
                            range += "≥" + MinValue / Convert.ToDouble(Zoom);
                        }
                        if (this.MaxValue != null)
                        {
                            if (range.IndexOf("≥") != -1)
                            {
                                range += "且";
                            }
                            range += "≤" + MaxValue / Convert.ToDouble(Zoom);
                        }

                        if (range != "")
                        {
                            range = String.Format("({0})", range);
                            range = "<font color='#999999'>" + range + "</font>";
                            _descriptionControl.Text += " " + range;
                        }
                    }

                }
                return _descriptionControl;
            }
            set
            {
                _descriptionControl = value;
            }
        }


        private bool _isEditMode;
        /// <summary>
        /// 是否编辑模式
        /// </summary>
        public bool IsEditMode
        {
            get
            {
                return _isEditMode;
            }
            set
            {
                _isEditMode = value;
            }
        }
        private bool _isVisible = true;
        /// <summary>
        /// 是否编辑模式
        /// </summary>
        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
            }
        }

        private bool _enabled = true;
        /// <summary>
        /// 控件是否可用
        /// </summary>
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
            }
        }



        public object Value
        {
            get
            {
                switch (this.ControlType)
                {
                    case Constant.CtrlType.TextBox:
                        return ((TextBox)this.MainControl[0]).Text;
                        break;
                    case Constant.CtrlType.UserControl:
                        //return ((UserControl)this.MainControl[0]);
                        return null;
                        break;
                    case Constant.CtrlType.CheckBox:
                        return ((CheckBox)this.MainControl[0]).Checked;
                        break;
                    case Constant.CtrlType.CheckBoxList:
                        CheckBoxList cbl = ((CheckBoxList)this.MainControl[0]);

                        string tempValue = "";
                        for (int n = 0; n < cbl.Items.Count; n++)
                        {
                            if (cbl.Items[n].Selected)
                            {
                                if (tempValue != "")
                                {
                                    tempValue = tempValue + ",";
                                }
                                tempValue += cbl.Items[n].Value;
                            }
                        }
                        return tempValue;

                        break;
                    case Constant.CtrlType.DateTextBox:
                        return
                            !String.IsNullOrEmpty(((TextBox)this.MainControl[0]).Text)
                                ? ((DateTime?)DateTime.Parse(((TextBox)this.MainControl[0]).Text))
                                : null;
                        break;
                    case Constant.CtrlType.DecimalTextBox:
                        return
                            !String.IsNullOrEmpty(((TextBox)this.MainControl[0]).Text)
                                ? ((Decimal?)Decimal.Parse(((TextBox)this.MainControl[0]).Text)) * Zoom
                                : null;
                        break;
                    case Constant.CtrlType.DoubleTextBox:
                        return
                            !String.IsNullOrEmpty(((TextBox)this.MainControl[0]).Text)
                                ? ((Double?)Double.Parse(((TextBox)this.MainControl[0]).Text)) * Convert.ToDouble(Zoom)
                                : null;
                        break;

                    case Constant.CtrlType.DropDownList:

                        if (this.TypeFullName != null
                                ? (this.TypeFullName.IndexOf("System.String") != -1)
                                : false || this.DataType == SqlDbType.NVarChar || this.DataType == SqlDbType.NText ||
                                  this.DataType == SqlDbType.NChar || this.DataType == SqlDbType.VarChar ||
                                  this.DataType == SqlDbType.Char || this.DataType == SqlDbType.Text)
                        {
                            return ((DropDownList)this.MainControl[0]).SelectedValue;
                        }
                        else if (this.TypeFullName != null
                                     ? (this.TypeFullName.IndexOf("System.Boolean") != -1)
                                     : false || this.DataType == SqlDbType.Bit)
                        {
                            return Convert.ToBoolean(Convert.ToInt32(((DropDownList)this.MainControl[0]).SelectedValue));
                        }
                        else if (this.TypeFullName != null
                                     ? (this.TypeFullName.IndexOf("System.DateTime") != -1)
                                     : false || this.DataType == SqlDbType.DateTime ||
                                       this.DataType == SqlDbType.SmallDateTime)
                        {
                            return Convert.ToDateTime(((DropDownList)this.MainControl[0]).SelectedValue);
                        }
                        else if (this.TypeFullName != null
                                     ? (this.TypeFullName.IndexOf("System.Int32") != -1)
                                     : false || this.DataType == SqlDbType.SmallInt ||
                                       this.DataType == SqlDbType.BigInt || this.DataType == SqlDbType.Int)
                        {
                            if (((DropDownList)this.MainControl[0]).SelectedValue != "")
                                return Convert.ToInt32(((DropDownList)this.MainControl[0]).SelectedValue);
                            else
                                return 0;
                        }
                        else if (this.TypeFullName != null
                                     ? (this.TypeFullName.IndexOf("System.Double") != -1)
                                     : false || this.DataType == SqlDbType.Float)
                        {
                            return Convert.ToDouble(((DropDownList)this.MainControl[0]).SelectedValue);
                        }
                        else if (this.TypeFullName != null
                                     ? (this.TypeFullName.IndexOf("System.Decimal") != -1)
                                     : false || this.DataType == SqlDbType.Decimal)
                        {
                            return
                                Convert.ToDecimal(((DropDownList)this.MainControl[0]).SelectedValue);
                        }

                        break;

                    case Constant.CtrlType.IntTextBox:
                        return
                            !String.IsNullOrEmpty(((TextBox)this.MainControl[0]).Text)
                                ? ((Int32?)Int32.Parse(((TextBox)this.MainControl[0]).Text)) * Zoom
                                : null;
                        break;
                    case Constant.CtrlType.Label:
                    case Constant.CtrlType.Rtf:
                        return ((Label)this.MainControl[0]).Text;
                        break;
                    case Constant.CtrlType.ListBox:
                        return ((ListBox)this.MainControl[0]).SelectedValue;
                        break;
                    case Constant.CtrlType.RadioButton:
                        return ((RadioButton)this.MainControl[0]).Checked;
                        break;
                    case Constant.CtrlType.RadioButtonList:
                        return ((RadioButtonList)this.MainControl[0]).SelectedValue;
                        break;
                    case Constant.CtrlType.TextArea:
                        return Functions.TextAreaEncode(((TextBox)this.MainControl[0]).Text);
                        break;
                    default :
                        return _value;
                }
                return null;
            }
            set
            {
                //if (this.MainControl[0] != null)
                {
                    switch (this.ControlType)
                    {
                        case Constant.CtrlType.TextBox:
                            ((TextBox)this.MainControl[0]).Text = (value != null ? value.ToString() : "");
                            break;
                      
                        case Constant.CtrlType.UserControl:

                            break;
                        case Constant.CtrlType.CheckBox:
                            ((CheckBox)this.MainControl[0]).Checked = (bool)((value != null && (bool)value == true) ? true : false);
                            break;
                        case Constant.CtrlType.CheckBoxList:
                            CheckBoxList cbl = ((CheckBoxList)this.MainControl[0]);
                            if (value != null)
                            {
                                for (int n = 0; n < cbl.Items.Count; n++)
                                {
                                    if (("," + value + ",").IndexOf("," + cbl.Items[n].Value + ",") != -1)
                                    {
                                        cbl.Items[n].Selected = true;

                                    }
                                }
                            }
                            break;
                        case Constant.CtrlType.DateTextBox:
                            ((TextBox)this.MainControl[0]).Text = (value != null ? ((DateTime)value).ToString() : "");
                            break;

                        case Constant.CtrlType.DecimalTextBox:
                            ((TextBox)this.MainControl[0]).Text = (value != null ? Convert.ToDouble(((decimal)value) / Zoom).ToString() : "");
                            break;
                        case Constant.CtrlType.DoubleTextBox:
                            ((TextBox)this.MainControl[0]).Text = (value != null ? ((Convert.ToDouble(value) / Convert.ToDouble(Zoom)).ToString()) : "");
                            break;
                        case Constant.CtrlType.DropDownList:
                            ((DropDownList)this.MainControl[0]).SelectedValue = (value != null ? value.ToString() : "");
                            break;

                        case Constant.CtrlType.IntTextBox:
                            ((TextBox)this.MainControl[0]).Text = (value != null ? (((int)value) / Zoom).ToString() : "");
                            break;
                        case Constant.CtrlType.Label:
                        case Constant.CtrlType.Rtf:
                            ((Label)this.MainControl[0]).Text = (value != null ? value.ToString() : "");
                            break;
                        case Constant.CtrlType.ListBox:
                            ((ListBox)this.MainControl[0]).SelectedValue = (value != null ? value.ToString() : "");
                            break;
                        case Constant.CtrlType.RadioButton:
                            ((RadioButton)this.MainControl[0]).Checked = (bool)value;
                            break;
                        case Constant.CtrlType.RadioButtonList:
                            ((RadioButtonList)this.MainControl[0]).SelectedValue = (value != null ? value.ToString() : "");
                            break;
                     
                        case Constant.CtrlType.TextArea:
                            ((TextBox)this.MainControl[0]).Text = Functions.TextAreaDecode((value != null ? value.ToString() : ""));
                            break;

                        case Constant.CtrlType.Link:
                            _value = value;
                            break;
                    }
                }

            }
        }
        private object _value = null;
        
        /// <summary>
        /// 创建主控件组
        /// </summary>
        /// <returns></returns>
        public ArrayList CreateControl()
        {
            ArrayList objList = new ArrayList();
            switch (this.ControlType)
            {
                case Constant.CtrlType.CheckBox:
                    {
                        CheckBox ctrl = new CheckBox();

                        ctrl.ID = this.Prefix + this.FieldName;
                        ctrl.Attributes.Add("CaptionName", this.CaptionName);
                        if (!this.IsEditMode)
                        {
                            ctrl.Checked = Convert.ToBoolean(this.DefaultValue);
                        }
                        ctrl.Enabled = this.Enabled;
                        objList.Add(ctrl);
                        SetControlValidator(objList, ctrl.ID);

                    }
                    break;
                case Constant.CtrlType.TextBox:
                    {
                        TextBox ctrl = new TextBox();
                        ctrl.ID = this.Prefix + this.FieldName;
                        ctrl.Attributes.Add("CaptionName", this.CaptionName);
                        ctrl.Width = Unit.Pixel(Width);
                        if (!this.IsEditMode)
                        {
                            ctrl.Text = this.DefaultValue;
                        }

                        if (!this.Enabled)
                        {
                            ctrl.BackColor = Color.FromName("#f8f8f8");
                            ctrl.Attributes.Add("readonly", "readonly");
                        }
                        objList.Add(ctrl);
                        SetControlValidator(objList, ctrl);

                    }
                    break;
              
                case Constant.CtrlType.UserControl:
                    {
                        UserControl ctrl = new UserControl();
                        //ctrl.Width = Unit.Percentage(Width);
                        ctrl.ID = this.Prefix + this.FieldName;
                        ctrl.Attributes.Add("CaptionName", this.CaptionName);



                        objList.Add(ctrl);
                    }
                    break;
                case Constant.CtrlType.CheckBoxList:
                    {
                        CheckBoxList ctrl = new CheckBoxList();
                        ctrl.BorderWidth = 0;
                        ctrl.CellPadding = 0;
                        ctrl.CellSpacing = 0;
                        ctrl.RepeatColumns = 4;
                        ctrl.RepeatDirection = System.Web.UI.WebControls.RepeatDirection.Horizontal;
                        //ctrl.Width = Unit.Percentage(Width);
                        ctrl.ID = this.Prefix + this.FieldName;
                        ctrl.Attributes.Add("CaptionName", this.CaptionName);
                        if (!this.IsEditMode && Functions.IsNotNull(this.DefaultValue))
                        {
                            if (this.DefaultValue.IndexOf(",") == -1)
                            {
                                ctrl.SelectedValue = this.DefaultValue;
                            }
                        }
                        else
                        {

                        }

                        ctrl.Enabled = this.Enabled;

                        objList.Add(ctrl);
                        SetControlValidator(objList, ctrl.ID);
                    }
                    break;
                case Constant.CtrlType.DateTextBox:
                    {
                        TextBox ctrl = new TextBox();
                        ctrl.ID = this.Prefix + this.FieldName;
                        ctrl.Attributes.Add("CaptionName", this.CaptionName);
                        ctrl.Width = Unit.Pixel(Width);
                        if (!this.IsEditMode)
                        {
                            ctrl.Text = this.DefaultValue;
                        }

                        if (!this.Enabled)
                        {
                            ctrl.BackColor = Color.FromName("#f8f8f8");
                            ctrl.Attributes.Add("readonly", "readonly");
                        }
                        ctrl.Attributes.Add("onclick", "SelectDate(this,'yyyy-MM-dd')");
                        objList.Add(ctrl);
                        SetControlValidator(objList, ctrl.ID);
                    }
                    break;
              
                case Constant.CtrlType.DecimalTextBox:
                    {
                        TextBox ctrl = new TextBox();
                        ctrl.ID = this.Prefix + this.FieldName;
                        ctrl.Attributes.Add("CaptionName", this.CaptionName);
                        ctrl.Width = Unit.Pixel(Width);
                        if (!this.IsEditMode)
                        {
                            ctrl.Text = this.DefaultValue;
                        }

                        if (!this.Enabled)
                        {
                            ctrl.BackColor = Color.FromName("#f8f8f8");
                            ctrl.Attributes.Add("readonly", "readonly");
                        }
                        objList.Add(ctrl);
                        ctrl.Attributes.Add("onkeypress", "return onlynum(event,this)");
                        ctrl.Attributes.Add("onblur", "if(isNaN(value))execCommand('undo')");
                        SetControlValidator(objList, ctrl);

                    }
                    break;

                case Constant.CtrlType.DoubleTextBox:
                    {
                        TextBox ctrl = new TextBox();
                        ctrl.ID = this.Prefix + this.FieldName;
                        ctrl.Attributes.Add("CaptionName", this.CaptionName);
                        ctrl.Width = Unit.Pixel(Width);
                        if (!this.IsEditMode)
                        {
                            ctrl.Text = this.DefaultValue;
                        }

                        if (!this.Enabled)
                        {
                            ctrl.BackColor = Color.FromName("#f8f8f8");
                            ctrl.Attributes.Add("readonly", "readonly");
                        }
                        objList.Add(ctrl);
                        ctrl.Attributes.Add("onkeypress", "return onlynum(event,this)");
                        ctrl.Attributes.Add("onblur", "if(isNaN(value))execCommand('undo')");
                        SetControlValidator(objList, ctrl);

                    }
                    break;

                case Constant.CtrlType.DropDownList:
                    {
                        DropDownList ctrl = new DropDownList();
                        ctrl.ID = this.Prefix + this.FieldName;
                        ctrl.Attributes.Add("CaptionName", this.CaptionName);
                        if (!this.IsEditMode)
                        {
                            ctrl.SelectedValue = this.DefaultValue;
                        }
                        ctrl.Enabled = this.Enabled;

                        objList.Add(ctrl);
                        SetControlValidator(objList, ctrl.ID);
                    }
                    break;

                case Constant.CtrlType.IntTextBox:
                    {
                        TextBox ctrl = new TextBox();
                        ctrl.ID = this.Prefix + this.FieldName;
                        ctrl.Attributes.Add("CaptionName", this.CaptionName);
                        ctrl.Width = Unit.Pixel(Width);
                        if (!this.IsEditMode)
                        {
                            ctrl.Text = this.DefaultValue;
                        }

                        if (!this.Enabled)
                        {
                            ctrl.BackColor = Color.FromName("#f8f8f8");
                            ctrl.Attributes.Add("readonly", "readonly");
                        }
                        objList.Add(ctrl);
                        ctrl.Attributes.Add("onkeypress", "return onlyint(event,this)");
                        ctrl.Attributes.Add("onblur", "if(isNaN(value))execCommand('undo')");
                        SetControlValidator(objList, ctrl);
                    }
                    break;
                case Constant.CtrlType.Label:
                case Constant.CtrlType.Rtf:
                    {
                        Label ctrl = new Label();
                        if (!this.IsEditMode)
                        {
                            ctrl.Text = this.DefaultValue;
                        }
                        ctrl.ID = this.Prefix + this.FieldName;
                        ctrl.Attributes.Add("CaptionName", this.CaptionName);
                        ctrl.Enabled = this.Enabled;
                        if (!this.Enabled)
                        {
                            ctrl.BackColor = Color.FromName("#f8f8f8");
                        }
                        objList.Add(ctrl);
                    }
                    break;
                case Constant.CtrlType.ListBox:
                    {
                        ListBox ctrl = new ListBox();
                        ctrl.ID = this.Prefix + this.FieldName;
                        ctrl.Attributes.Add("CaptionName", this.CaptionName);
                        ctrl.Enabled = this.Enabled;
                        if (!this.Enabled)
                        {
                            ctrl.BackColor = Color.FromName("#f8f8f8");
                        }
                        objList.Add(ctrl);
                    }
                    break;
                case Constant.CtrlType.RadioButton:
                    {
                        RadioButton ctrl = new RadioButton();
                        if (!this.IsEditMode)
                        {
                            ctrl.Checked = Convert.ToBoolean(Convert.ToInt32(this.DefaultValue));
                        }
                        ctrl.ID = this.Prefix + this.FieldName;
                        ctrl.Attributes.Add("CaptionName", this.CaptionName);
                        objList.Add(ctrl);
                        SetControlValidator(objList, ctrl.ID);
                    }
                    break;
                case Constant.CtrlType.RadioButtonList:
                    {
                        RadioButtonList ctrl = new RadioButtonList();
                        ctrl.ID = this.Prefix + this.FieldName;
                        ctrl.Attributes.Add("CaptionName", this.CaptionName);
                        if (!this.IsEditMode)
                        {
                            ctrl.SelectedValue = this.DefaultValue;
                        }
                        ctrl.Enabled = this.Enabled;

                        objList.Add(ctrl);
                        SetControlValidator(objList, ctrl.ID);

                    }
                    break;
                case Constant.CtrlType.TextArea:
                    {
                        TextBox ctrl = new TextBox();
                        ctrl.ID = this.Prefix + this.FieldName;
                        ctrl.Attributes.Add("CaptionName", this.CaptionName);
                        ctrl.TextMode = TextBoxMode.MultiLine;
                        if (!this.IsEditMode)
                        {
                            ctrl.Text = this.DefaultValue;
                        }
                        ctrl.Rows = 5;

                        ctrl.Width = Unit.Percentage(80);

                        if (!this.Enabled)
                        {
                            ctrl.BackColor = Color.FromName("#f8f8f8");
                            ctrl.Attributes.Add("readonly", "readonly");
                        }
                        objList.Add(ctrl);
                        SetControlValidator(objList, ctrl);
                    }
                    break;
                case Constant.CtrlType.Link:
                    {
                        //创建链接
                        HtmlAnchor anchor = new HtmlAnchor();
                        anchor.Attributes.Add("class", "linkForClick");

                        anchor.Attributes.Add("onclick", "linkClick('" + this.Value.ToString() + "');return false;");
                        var vs = this.Value.ToString().Split('|');
                        var dv = vs.LastOrDefault();
                        anchor.Attributes.Add("class", "linkForClick");

                        anchor.InnerText = dv;
                        objList.Add(anchor);
                        break;
                    }
               
            }
            return objList;
        }


        public void SetControlValidator(ArrayList list, TextBox tb)
        {
            if (IsNeed)
            {
                if (IsDisplayNeed)
                {
                    bool hasflag = false;
                    foreach (Control ctrl in list)
                    {
                        if (ctrl.ID == this.Prefix + "lbl_" + this.FieldName)
                        {
                            hasflag = true;
                        }
                    }
                    if (hasflag == false)
                    {
                        Label lbl = new Label();
                        lbl.ID = this.Prefix + "lbl_" + this.FieldName;
                        lbl.Text = "*";
                        lbl.ForeColor = System.Drawing.Color.Red;
                        list.Add(lbl);
                    }

                }

                tb.Attributes.Add("IsNeed", IsNeed.ToString());
            }
            else
            {
                tb.Attributes.Add("IsNeed", "false");
            }
            if (MinValue != null || MaxValue != null)
            {
                if (this.MinValue != null)
                {
                    string min = (this.MinValue / Convert.ToDouble(Zoom)).ToString();
                    tb.Attributes.Add("Min", min);
                }

                if (this.MaxValue != null)
                {
                    string max = (this.MaxValue / Convert.ToDouble(Zoom)).ToString();
                    tb.Attributes.Add("Max", max);
                }
            }


        }

        public void SetControlValidator(ArrayList obj, string validControldID)
        {
            if (IsNeed)
            {
                if (IsDisplayNeed)
                {
                    Label lbl = new Label();
                    lbl.ID = this.Prefix + "lbl_" + this.FieldName;
                    lbl.Text = "*";
                    lbl.ForeColor = System.Drawing.Color.Red;
                    obj.Add(lbl);

                }

                RequiredFieldValidator RequiredFieldTextBox = new RequiredFieldValidator();
                RequiredFieldTextBox.ID = this.Prefix + "required_" + this.FieldName;
                RequiredFieldTextBox.ControlToValidate = validControldID;
                RequiredFieldTextBox.ErrorMessage = "请填写[" + this.CaptionName + "]";
                RequiredFieldTextBox.ForeColor = System.Drawing.Color.Red;
                RequiredFieldTextBox.Display = ValidatorDisplay.None;
                obj.Add(RequiredFieldTextBox);

            }
            if (MinValue != null || MaxValue != null)
            {
                RangeValidator rv = new RangeValidator();
                rv.ID = this.Prefix + "range_" + this.FieldName;
                rv.ControlToValidate = validControldID;
                rv.Type = ValidationDataType.Double;
                rv.ErrorMessage = "[" + this.CaptionName + "]必须";

                if (this.MinValue != null)
                {
                    rv.MinimumValue = (this.MinValue / Convert.ToDouble(Zoom)).ToString();
                    rv.ErrorMessage += "≥" + rv.MinimumValue;
                    if (this.MaxValue == null)
                    {
                        rv.MaximumValue = "999999999";
                    }
                }

                if (this.MaxValue != null)
                {
                    if (rv.ErrorMessage.IndexOf("≥") != -1)
                    {
                        rv.ErrorMessage += "且";
                    }
                    rv.MaximumValue = (this.MaxValue / Convert.ToDouble(Zoom)).ToString();
                    rv.ErrorMessage += "≤" + rv.MaximumValue;
                    if (this.MinValue == null)
                    {
                        rv.MinimumValue = "0";
                    }
                }
                rv.Display = ValidatorDisplay.None;
                obj.Add(rv);
            }
        }

        /// <summary>
        /// 清除所有控件，重新生成
        /// </summary>
        public void ClearControl()
        {
            this._mainControl = null;
            this._descriptionControl = null;
            this._captionControl = null;
        }



        public static ListBase<Field> GetListField<ET>(string preFix)
        {
            ListBase<Field> _listMemberSetField = new ListBase<Field>("FieldName");
            PropertyInfo[] piEntity;
            piEntity = typeof(ET).GetProperties();
            int i = 0;
            foreach (PropertyInfo pif in piEntity)
            {
                {
                    Field field = new Field();
                    field.FieldName = pif.Name;
                    field.Prefix = preFix;
                    field.CaptionName = pif.Name;
                    field.OrderNo = i * 10;
                    //根据实体属性字段的数据类型，生成对应的控件
                    field.TypeFullName = pif.PropertyType.FullName;

                    if (pif.PropertyType.FullName.IndexOf("System.String") != -1)
                    {
                        field.ControlType = Constant.CtrlType.TextBox;
                    }
                    else if (pif.PropertyType.FullName.IndexOf("System.Boolean") != -1)
                    {
                        field.ControlType = Constant.CtrlType.CheckBox;
                    }
                    else if (pif.PropertyType.FullName.IndexOf("System.DateTime") != -1)
                    {
                        field.ControlType = Constant.CtrlType.DateTextBox;
                    }
                    else if (pif.PropertyType.FullName.IndexOf("System.Int32") != -1)
                    {
                        field.ControlType = Constant.CtrlType.IntTextBox;
                    }
                    else if (pif.PropertyType.FullName.IndexOf("System.Double") != -1)
                    {
                        field.ControlType = Constant.CtrlType.DoubleTextBox;
                    }
                    else if (pif.PropertyType.FullName.IndexOf("System.Decimal") != -1)
                    {
                        field.ControlType = Constant.CtrlType.DecimalTextBox;
                    }
                    _listMemberSetField.Add(field);
                    i++;

                }
            }
            _listMemberSetField.Sort("OrderNo", true);
            _listMemberSetField.Remove(_listMemberSetField["EntityState"]);
            _listMemberSetField.Remove(_listMemberSetField["TableName"]);
            _listMemberSetField.Remove(_listMemberSetField["PrimaryKeyField"]);
            return _listMemberSetField;
        }


        public static void CreateTable(HtmlTable tableCtrl, ListBase<Field> listField, Button btnQuery, string name)
        {
            tableCtrl.Rows.Clear();
            HtmlTableRow tr = new HtmlTableRow();
            HtmlTableCell cellBottom = new HtmlTableCell();
            foreach (Field field in listField)
            {
                HtmlTableCell td1 = new HtmlTableCell();
                td1.Controls.Add(field.CaptionControl);
                td1.Visible = field.IsVisible;
                td1.Style.Add("padding-left", "20px");
                HtmlTableCell td2 = new HtmlTableCell();
                for (int n = 0; n < field.MainControl.Count; n++)
                {
                    td2.Controls.Add((Control)field.MainControl[n]);
                }
                td2.Controls.Add(field.DescriptionControl);
                td2.Visible = field.IsVisible;

                tr.Cells.Add(td1);
                tr.Cells.Add(td2);
            }
            btnQuery.ID = name + "btnQuery";
            btnQuery.Text = "查 询";
            btnQuery.CssClass = "button";
            cellBottom.Controls.Add(btnQuery);
            tr.Cells.Add(cellBottom);
            tableCtrl.Rows.Add(tr);
        }

        public static void CreateTable(HtmlTable tableCtrl, ListBase<Field> listField, int colsCount, string name)
        {
            //先清除所有行
            tableCtrl.Rows.Clear();

            int i = 0;
            HtmlTableRow tr = null;
            int visibleCount = 0;
            foreach (Field field in listField)
            {
                if (field.IsVisible)
                {
                    visibleCount++;
                }
            }
            int j = 0;
            foreach (Field field in listField)
            {
                if (field.IsVisible)
                {
                    if (j == visibleCount - 1)
                    {
                        field.Colspan = (colsCount - 1 - i % colsCount) + 1;
                    }
                    if (i % colsCount == 0)
                    {
                        tr = new HtmlTableRow();
                        tr.ID = name + "tr" + i;
                    }

                    HtmlTableCell td1 = new HtmlTableCell();
                    td1.Width = (100 / (colsCount * 4)) + "%";
                    td1.Attributes.Add("align", "right");
                    td1.Attributes.Add("class", "cell_l");
                    td1.Controls.Add(field.CaptionControl);
                    td1.Visible = field.IsVisible;

                    HtmlTableCell td2 = new HtmlTableCell();
                    td2.ColSpan = (field.Colspan - 1) * 2 + 1;
                    td2.Width = (((100 / (colsCount * 4)) * 3) + (100 / (colsCount * 4)) * (field.Colspan - 1)) + "%"; ;
                    td2.Attributes.Add("class", "cell_r");
                    for (int n = 0; n < field.MainControl.Count; n++)
                    {
                        td2.Controls.Add((Control)field.MainControl[n]);
                    }
                    td2.Controls.Add(field.DescriptionControl);
                    td2.Visible = field.IsVisible;

                    tr.Controls.Add(td1);
                    tr.Controls.Add(td2);
                    tableCtrl.Rows.Add(tr);


                    i = i + field.Colspan;
                    j++;


                }
                else
                {
                    HtmlTableCell td2 = new HtmlTableCell();
                    td2.ColSpan = (field.Colspan - 1) * 2 + 1;
                    td2.Width = (((100 / (colsCount * 4)) * 3) + (100 / (colsCount * 4)) * (field.Colspan - 1)) + "%"; ;
                    td2.Attributes.Add("class", "cell_r");
                    for (int n = 0; n < field.MainControl.Count; n++)
                    {
                        td2.Controls.Add((Control)field.MainControl[n]);
                    }
                    td2.Controls.Add(field.DescriptionControl);
                    td2.Visible = field.IsVisible;

                    HtmlTableRow trhidden = new HtmlTableRow();
                    trhidden.Controls.Add(td2);
                    trhidden.Visible = false;
                    tableCtrl.Rows.Add(trhidden);
                }

            }

        }

        public static void CreateTableBottom(HtmlTable tableCtrl, Button BtnOk, Button BtnCancel, int colsCount, string name)
        {
            //增加底部按钮行
            HtmlTableRow rowBottom = new HtmlTableRow();
            HtmlTableCell cellBottom = new HtmlTableCell();

            cellBottom.ColSpan = 2 * colsCount;
            cellBottom.Attributes.Add("class", "cell_b");
            cellBottom.Align = "center";
            cellBottom.Height = "40";
            cellBottom.VAlign = "middle";

            BtnOk.ID = name + "btnOk";
            BtnOk.CssClass = "button";
            BtnOk.Text = "确定";
            cellBottom.Controls.Add(BtnOk);

            Label space = new Label();
            space.Width = Unit.Pixel(100);
            cellBottom.Controls.Add(space);
            BtnCancel.ID = name + "btnCancel";
            BtnCancel.CssClass = "button";
            BtnCancel.Text = "取消";
            //btnCancel.Attributes.Add("onClick", string.Format("JavaScript:window.location.href='{0}';return false;", ReturnPath));
            cellBottom.Controls.Add(BtnCancel);


            rowBottom.Cells.Add(cellBottom);
            tableCtrl.Rows.Add(rowBottom);
        }


        public static void Save<ET, PT>(ListBase<Field> listField, ET entityObj)
        {
            {
                MethodInfo saveMethod = typeof(PT).GetMethod("Save", new Type[] { typeof(ET) });
                saveMethod.Invoke(null, new object[] { entityObj });
            }
        }


        /// <summary>
        /// 获取控件的值并赋给实体
        /// </summary>
        public static void GetControlValue<ET>(ListBase<Field> listField, object entity)
        {
            foreach (PropertyInfo pif in typeof(ET).GetProperties())
            {
                if (listField[pif.Name] != null)
                {
                    if (listField[pif.Name].ControlType != Constant.CtrlType.Label && listField[pif.Name].FieldType == 1 && listField[pif.Name].IsVisible)
                    {
                        try
                        {
                            if (pif.PropertyType == typeof(System.Boolean?) && (listField[pif.Name].Value is string))
                            {
                                pif.SetValue(entity, Convert.ToBoolean(Convert.ToInt32(listField[pif.Name].Value)), null);
                            }
                            else if (pif.PropertyType == typeof(Int32) && (listField[pif.Name].Value is string))
                            {
                                pif.SetValue(entity, Convert.ToInt32(Functions.IsNotNull(listField[pif.Name].Value.ToString()) ? listField[pif.Name].Value : "0"), null);
                            }
                            else if (pif.PropertyType == typeof(DateTime?) && (listField[pif.Name].Value is string))
                            {
                                pif.SetValue(entity, Convert.ToDateTime(listField[pif.Name].Value), null);
                            }
                            else if (pif.PropertyType == typeof(Double?) && (listField[pif.Name].Value is string))
                            {
                                pif.SetValue(entity, Convert.ToDouble(Functions.IsNotNull(listField[pif.Name].Value.ToString()) ? listField[pif.Name].Value : "0.00"), null);
                            }
                            else
                            {
                                pif.SetValue(entity, listField[pif.Name].Value, null);
                            }

                        }
                        catch (System.ArgumentException ex)
                        {
                            //碰到DropDownList SeleledValue 是INT型，做类型转换
                            SMT.SaaS.Common.ErrorLog.Log("获取控件值时出错：" + ex.ToString());
                            pif.SetValue(entity, Convert.ToInt32(Functions.IsNotNull(listField[pif.Name].Value.ToString()) ? listField[pif.Name].Value : "0"), null);
                        }

                    }
                }
            }
        }


        /// <summary>
        /// 根据实体设置控件的值
        /// </summary>
        public static void SetControlValue<ET>(ListBase<Field> listField, object entity)
        {


            foreach (PropertyInfo pif in typeof(ET).GetProperties())
            {
                if (listField[pif.Name] != null)
                {
                    listField[pif.Name].Value = pif.GetValue(entity, null);
                }
            }
        }


    }
}