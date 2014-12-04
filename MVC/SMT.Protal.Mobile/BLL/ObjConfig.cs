using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using SMT.Portal.Common.SmtForm.Framework;

namespace SMT.Portal.Common.SmtForm.BLL
{
    /// <summary>
    /// 对应Config.xml文档
    /// </summary>
    [XmlRoot("Config")]
    public class ObjConfig
    {
        /// <summary>
        /// 样式模板
        /// </summary>
        private ObjTemplate[] _arrTemplate = null;
        [XmlElement(ElementName = "Template")]
        public ObjTemplate[] ArrTemplate
        {
            get { return _arrTemplate; }
            set { _arrTemplate = value; }
        }

        /// <summary>
        /// 查看类型，是Phone还是Pad
        /// </summary>
        private ObjViewmode[] _arrViewmode = null;
        [XmlElement(ElementName = "Viewmode")]
        public ObjViewmode[] ArrViewmode
        {
            get { return _arrViewmode; }
            set { _arrViewmode = value; }
        }

        /// <summary>
        /// 获取样式模板
        /// </summary>
        /// <returns>返回样式模板集合</returns>
        public ListBase<ObjTemplate> GetTemplate()
        {
            ListBase<ObjTemplate> _TemplateDict = new ListBase<ObjTemplate>();
            _TemplateDict.KeyFieldName = "Name";
            foreach (ObjTemplate obj in _arrTemplate)
            {
                _TemplateDict.Add( obj);
            }
            return _TemplateDict;
        }

        /// <summary>
        /// 获取查看类型
        /// </summary>
        /// <returns>返回查看类型集合</returns>
        public ListBase<ObjViewmode> GetViewmode()
        {
            ListBase<ObjViewmode> _ViewmodeDict = new ListBase<ObjViewmode>();
            _ViewmodeDict.KeyFieldName = "Name";
            foreach (ObjViewmode obj in _arrViewmode)
            {
                _ViewmodeDict.Add( obj);
            }
            return _ViewmodeDict;
        }
    }
}