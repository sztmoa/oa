using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace SMT.FileUpLoad.Service
{
    [ServiceContract]
    public class UserFile
    {
        #region 数据表的文件属性
        /// <summary>
        /// 文件保存的位置目录
        /// </summary>      
        [DataMember]
        public string SavePath { get; set; }
        /// <summary>
        /// 主键ID(数据库)
        /// </summary>      
        [DataMember]
        public string SmtFileListId { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>      
        [DataMember]
        public string FileName { get; set; }
        /// <summary>
        /// 文件类型(.doc、.xls、.txt、.pdf......)
        /// </summary>      
        [DataMember]
        public string FileType { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        [DataMember]
        public double FileSize { get; set; }
        /// <summary>
        /// 文件地址
        /// </summary>           
        [DataMember]
        public string FileUrl { get; set; }
        /// <summary>
        /// 公司代号
        /// </summary>      
        [DataMember]
        public string CompanyCode { get; set; }
        /// <summary>
        /// 公司名字
        /// </summary>      
        [DataMember]
        public string CompanyName { get; set; }
        /// <summary>
        /// 系统代号
        /// </summary>      
        [DataMember]
        public string SystemCode { get; set; }
        /// <summary>
        /// 模块代号
        /// </summary>      
        [DataMember]
        public string ModelCode { get; set; }
        /// <summary>
        /// 业务ID
        /// </summary>
         [DataMember]
        public string ApplicationID { get; set; }
        /// <summary>
        /// 缩略图地址
        /// </summary>      
        [DataMember]
        public string ThumbnailUrl { get; set; }
        /// <summary>
        /// 排序
        /// </summary>      
        [DataMember]
        public decimal INDEXL { get; set; }
        /// <summary>
        /// 备注
        /// </summary>      
        [DataMember]
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>      
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>      
        [DataMember]
        public string CreateName { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>      
        [DataMember]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>      
        [DataMember]
        public string UpdateName { get; set; }
        /// <summary>
        /// 文件类型（配置文件）
        /// </summary>      
        [DataMember]
        public string Type { get; set; }
        /// <summary>
        /// 最大上传数量
        /// </summary>      
        [DataMember]
        public int MaxNumber { get; set; }
        /// <summary>
        /// 最大上传大小
        /// </summary>      
        [DataMember]
        public double MaxSize { get; set; }
        /// <summary>
        /// 已上传大小
        /// </summary>
        public long BytesUploaded { get; set; }
        /// <summary>
        /// 最大上传速度
        /// </summary>      
        [DataMember]
        public double UploadSpeed { get; set; }
        /// <summary>
        /// 所属公司
        /// </summary>      
        [DataMember]
        public string OWNERCOMPANYID { get; set; }
        /// <summary>
        /// 所属部门
        /// </summary>      
        [DataMember]
        public string OWNERDEPARTMENTID { get; set; }
        /// <summary>
        /// 所属岗位
        /// </summary>      
        [DataMember]
        public string OWNERPOSTID { get; set; }
        /// <summary>
        /// 所属人
        /// </summary>      
        [DataMember]
        public string OWNERID { get; set; }

        #endregion
    }
}
