using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMT.Portal.Common.SmtForm.BLL;
using System.Xml;
using SMT.Portal.Common.SmtForm.Framework;

namespace SMT.Portal.Common.SmtForm.Logic
{
    public class BillData
    {
        public static ObjBill CreateBill(string xml, ref string error)
        {
            ObjBill bill = new ObjBill();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            //读取Form中的数据
            XmlNode nodeResult = doc.SelectSingleNode("//Result");
            if (nodeResult != null)
            {
                if (((XmlElement)nodeResult).GetAttribute("Code") != "0")
                {
                    error = ((XmlElement)nodeResult).InnerText.ToString();
                    throw new SmtException(((XmlElement)nodeResult).InnerText);
                }
                else
                {
                    XmlNode nodeForm = doc.SelectSingleNode("//Form");
                    bill.ModelCode = ((XmlElement)nodeForm).GetAttribute("ModelCode");
                    bill.OrderType = ((XmlElement)nodeForm).GetAttribute("OrderType");
                    bill.Text = ((XmlElement)nodeForm).GetAttribute("Text");
                    bill.FormID = ((XmlElement)nodeForm).GetAttribute("FormID");
                    bill.MessageStatus = ((XmlElement)nodeForm).GetAttribute("MESSAGESTATUS");
                    ObjBill schemaBill = DataManager<ObjBill>.Get(HttpContext.Current.Server.MapPath("BillSchema/" + bill.ModelCode + ".xml"));

                    XmlNode nodeObject = doc.SelectSingleNode("//Object");

                    { //读取主表数据

                        XmlNodeList list = nodeObject.SelectNodes("Attribute");
                        ObjDetailList detailList = new ObjDetailList();
                        detailList.ObjFieldList = new ObjField[list.Count];
                        foreach (var item in schemaBill.DetailLists)
                        {
                            item.Load();
                        }
                        for (int i = 0; i < list.Count; i++)
                        {
                            XmlElement node = list[i] as XmlElement;
                            string Name = ((XmlElement)node).GetAttribute("Name");
                            string Text = ((XmlElement)node).GetAttribute("Text");
                            string DataType = ((XmlElement)node).GetAttribute("DataType");
                            string DataValue = ((XmlElement)node).GetAttribute("DataValue");
                            string fgColor = string.Empty;
                            string Tooltip = string.Empty;
                            if (node.HasAttribute("Color"))
                            {
                                fgColor = node.GetAttribute("Color");
                            }
                            if (node.HasAttribute("Tooltip"))
                            {
                                Tooltip = node.GetAttribute("Tooltip");
                            }

                            ObjField dataField = new ObjField();
                            dataField.Name = Name;
                            dataField.Text = Text;
                            dataField.DataType = DataType;
                            dataField.DataValue = DataValue.Replace("\n","<br />");
                            dataField.Color = fgColor;
                            dataField.Tooltip = Tooltip;

                            //将数据样式应用于字段
                            if (schemaBill.DetailLists.Length > 0 &&
                                schemaBill.DetailLists[0].ObjFieldList.Length > 0 && schemaBill.DetailLists[0].GetObjFieldDict()[Name] != null)
                            {

                                ObjField schemaField = schemaBill.DetailLists[0].GetObjFieldDict()[Name];
                                dataField.Colspan = schemaField.Colspan;
                                dataField.IsVisible = schemaField.IsVisible;
                                dataField.CssName = schemaField.CssName;
                                dataField.Orderno = schemaField.Orderno;
                                dataField.ColumnOrder = schemaField.ColumnOrder;
                                dataField.Text = schemaField.Text;
                                dataField.IsShowCaption = schemaField.IsShowCaption;
                                dataField.IsLink = schemaField.IsLink;
                                if (dataField.DataType.ToLower() == "rtf")
                                {
                                    bill.RtfData = dataField.DataValue;
                                }
                                //因业务编号的问题，需要重新获取编号（目前出差报销）
                                if (dataField.DataValue == "")
                                {
                                    //TODO:需要重新获取编号
                                    //UNDONE:需要重新获取编号

                                }
                            }
                            else //在配置文件中没找到的字段，都不显示 
                            {
                                dataField.IsVisible = false;
                            }
                            detailList.ObjFieldList[i] = dataField;
                            detailList.PadRepeatCount = schemaBill.DetailLists[0].PadRepeatCount;
                            detailList.PhoneRepeatCount = schemaBill.DetailLists[0].PhoneRepeatCount;
                        }
                        bill.DetailLists = new ObjDetailList[] { detailList };

                    }


                    bill.SubLists = CreateSubList(nodeObject, "", schemaBill, false);

                    XmlNode ConsultationList = doc.SelectSingleNode("//ConsultationList");
                    if (ConsultationList != null)
                    {
                        bill.ConsultationLists = CreateConsultationList(ConsultationList);
                    }
                    XmlNode nodeApproval = doc.SelectSingleNode("//ApprovalList");

                    if (nodeApproval != null)
                    {//读取审核列表数据

                        XmlNodeList list = nodeApproval.SelectNodes("Atrribute");
                        ObjApprovalList approvalList = new ObjApprovalList();
                        approvalList.Text = "审核信息";
                        approvalList.ApprovalList = new ObjApproval[list.Count];
                        for (int i = 0; i < list.Count; i++)
                        {
                            XmlNode node = list[i];
                            string Approver = ((XmlElement)node).GetAttribute("Approver");
                            string ApprovalTime = ((XmlElement)node).GetAttribute("ApprovalTime");
                            string ApprovalState = ((XmlElement)node).GetAttribute("ApprovalState");
                            string ApprovalRemark = ((XmlElement)node).GetAttribute("ApprovalRemark");
                            string FlowType = ((XmlElement)node).GetAttribute("FlowType");
                            string Flag = ((XmlElement)node).GetAttribute("Flag");
                            ObjApproval dataField = new ObjApproval();
                            dataField.Approver = Approver;
                            dataField.ApprovalTime = Convert.ToDateTime(ApprovalTime);
                            dataField.ApprovalState = ApprovalState;
                            dataField.ApprovalRemark = ApprovalRemark;
                            dataField.Flag = Flag;
                            dataField.FlowType = FlowType;

                            //应用样式于字段
                            if (null != schemaBill.ApprovalLists && schemaBill.ApprovalLists.Length > 0 && null != schemaBill.ApprovalLists[0].ApprovalList &&
                                schemaBill.ApprovalLists[0].ApprovalList.Length > 0 && schemaBill.ApprovalLists[0].GetApprovalDict()[Approver] != null)
                            {
                                ObjApproval schemaField = schemaBill.ApprovalLists[0].GetApprovalDict()[Approver];

                                //dataField.Colspan = schemaField.Colspan;
                            }
                            approvalList.ApprovalList[i] = dataField;
                        }
                        bill.ApprovalLists = new ObjApprovalList[] { approvalList };
                    }


                    XmlNode nodeAttach = doc.SelectSingleNode("//AttachList");
                    if (nodeAttach != null)
                    {//读取附件列表数据

                        XmlNodeList list = nodeAttach.SelectNodes("Atrribute");
                        ObjAttachList approvalList = new ObjAttachList();
                        approvalList.AttachList = new ObjAttach[list.Count];
                        for (int i = 0; i < list.Count; i++)
                        {
                            XmlNode node = list[i];
                            string Name = ((XmlElement)node).GetAttribute("Name");
                            string Url = ((XmlElement)node).GetAttribute("Url");

                            ObjAttach dataField = new ObjAttach();
                            dataField.Name = Name;
                            dataField.Url = Url;

                            //将样式应用于字段
                            if (null != schemaBill && null != schemaBill.AttachLists && schemaBill.AttachLists.Length > 0 &&
                                schemaBill.AttachLists[0].AttachList != null &&
                                schemaBill.AttachLists[0].AttachList.Length > 0 && schemaBill.AttachLists[0].GetAttachDict()[Name] != null)
                            {
                                ObjAttach schemaField = schemaBill.AttachLists[0].GetAttachDict()[Name];
                                //dataField.Colspan = schemaField.Colspan;
                            }
                            approvalList.AttachList[i] = dataField;
                        }
                        bill.AttachLists = new ObjAttachList[] { approvalList };
                    }


                }



            }

            return bill;

        }

        public static ObjConsultationList[] CreateConsultationList(XmlNode nodeObject)
        {
            XmlNodeList list = nodeObject.SelectNodes("Consultation");
            ObjConsultationList[] consultationList = new ObjConsultationList[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                XmlElement element = list[i] as XmlElement;
                string Content = element.GetAttribute("Content");

                string ConsultationDate = element.GetAttribute("ConsultationDate");
                string ConsultationUserName = element.GetAttribute("ConsultationUserName");
                string Flag = element.GetAttribute("Flag");
                string ReplyContent = element.GetAttribute("ReplyContent");
                string ReplyDate = element.GetAttribute("ReplyDate");
                string ReplyUserName = element.GetAttribute("ReplyUserName");
                string ConsultationID = element.GetAttribute("ConsultationID");
                ObjConsultationList objConsultation = new ObjConsultationList();
                objConsultation.Content = Content;
                objConsultation.ConsultationDate = Convert.ToDateTime(ConsultationDate);
                objConsultation.ConsultationUserName = ConsultationUserName;
                objConsultation.Flag = Flag;
                objConsultation.ReplyContent = ReplyContent;
                objConsultation.ReplyDate = Convert.ToDateTime(ReplyDate);
                objConsultation.ReplyUserName = ReplyUserName;
                objConsultation.ConsultationID = ConsultationID;
                consultationList[i] = objConsultation;
            }

            return consultationList;
        }

        public static ObjSubList[] CreateSubList(XmlNode nodeObject, string listName, ObjBill schemaBill, bool isSub)
        {

            //选取ObjectList节点
            XmlNodeList listList = nodeObject.SelectNodes("ObjectList");
            //根据节点数建立实体集
            ObjSubList[] sublists = new ObjSubList[listList.Count];
            //遍历结点
            for (int j = 0; j < listList.Count; j++)
            {
                //从该结点获取一个ObjectList的Name
                string subListName = ((XmlElement)listList[j]).GetAttribute("Name");
                string subListText = ((XmlElement)listList[j]).GetAttribute("Text");
                //建立Sublist对象
                ObjSubList objSubList = new ObjSubList();
                objSubList.Name = subListName;
                objSubList.Text = subListText;
                var temp =schemaBill.GetSubListsDict()[subListName];
                if (temp != null)
                {
                    objSubList.Align = temp.Align;
                    objSubList.HideWhenEmpty = temp.HideWhenEmpty;
                }

                //把billschema的columnList的信息赋给bill

                //从该结点中获取Object节点集，对应Sublist的行数据
                XmlNodeList objlist = listList[j].SelectNodes("Object");
                //根据xml数据中的object节点数建立sublist对象的rowlist数组
                objSubList.RowList = new ObjRow[objlist.Count];
                //遍历 object节点集，以构建数据
                for (int i = 0; i < objlist.Count; i++)
                {
                    //得到object的name
                    string rowName = ((XmlElement)objlist[i]).GetAttribute("Name");
                    string rowID = ((XmlElement)objlist[i]).GetAttribute("id");
                    if (string.IsNullOrEmpty(rowID))
                    {
                      //  continue;
                    }
                    //从object结点中得到所有的attribute，即该行的所有字段
                    XmlNodeList list = objlist[i].SelectNodes("Attribute");
                    //建立对应的objrow对象以容纳字段数据
                    ObjRow row = new ObjRow();
                    row.Name = rowName;
                    row.Id = rowID;
                    //根据attribut的数量建立fieldlist数组
                    row.ObjFieldList = new ObjField[list.Count];
                    //遍历attribute结点
                    for (int m = 0; m < list.Count; m++)
                    {
                        //从attribute节点中得到数据信息
                        XmlElement node = list[m] as XmlElement;
                        string Name = ((XmlElement)node).GetAttribute("Name");
                        string Text = ((XmlElement)node).GetAttribute("Text");
                        string DataType = ((XmlElement)node).GetAttribute("DataType");
                        string DataValue = ((XmlElement)node).GetAttribute("DataValue");

                        //建立对应的objfield对象
                        ObjField dataField = new ObjField();
                        dataField.Name = Name;
                        //dataField.Text = Text;
                        dataField.DataType = DataType;
                        dataField.DataValue = DataValue;
                        string fgColor = string.Empty;
                        string Tooltip = string.Empty;
                        if (node.HasAttribute("Color"))
                        {
                            fgColor = node.GetAttribute("Color");
                        }
                        if (node.HasAttribute("Tooltip"))
                        {
                            Tooltip = node.GetAttribute("Tooltip");
                        }
                        dataField.Color = fgColor;
                        dataField.Tooltip = Tooltip;


                        //如果能够在配置文件中找到对应的定义项，则把该定义项的相关信息赋值到该对象。
                        //通过Name作为key进行过索引
                        if (schemaBill.GetSubListsDict()[subListName] != null && schemaBill.GetSubListsDict()[subListName].GetRowDict()[rowID] != null && schemaBill.GetSubListsDict()[subListName].GetRowDict()[rowID].GetObjFieldDict()[Name] != null)
                        {
                            ObjField schemaField = schemaBill.GetSubListsDict()[subListName].GetRowDict()[rowID].GetObjFieldDict()[Name];
                            dataField.Colspan = schemaField.Colspan;
                            dataField.IsVisible = schemaField.IsVisible;
                            dataField.CssName = schemaField.CssName;
                            dataField.ColumnOrder = schemaField.ColumnOrder;
                        }
                        //把建立的field对象赋给对应索引的fieldlist数组。
                        row.ObjFieldList[m] = dataField;

                    }
                    //把建立的row对象赋给对应索引的 rowlist数组
                    objSubList.RowList[i] = row;
                    row.SubSubList = CreateSubList(objlist[i], objSubList.Name, schemaBill, true);
                }
                //把建立的sublist对象赋给对应索引的sublist数组
                if (schemaBill.GetSubListsDict()[objSubList.Name] != null && isSub == false)
                {
                    objSubList.ColumnList = schemaBill.GetSubListsDict()[objSubList.Name].ColumnList;
                }
                //把建立的sublist对象赋给对应索引的sublist数组
                if (schemaBill.GetSubListsDict()[listName] != null && isSub == true)
                {
                    objSubList.ColumnList = schemaBill.GetSubListsDict()[listName].SubColumnList;
                }
                sublists[j] = objSubList;
            }
            return sublists;
        }
    }
}