using System;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace SMT.FileUpLoad.Service
{
    [DataContract]
    public class UpFileInfo
    {
        public string SystemCode
        {
            get;set;
        }
        public string ModelCode
        {
            get;set;
        }
        public string FileName
        {
             get;set;
        }
        public string Md5FileName
        {
            get;set;
        }
        public string ID
        {
            get;set;
        }
        public  byte[] ByteData
        {
            get;set;
        }
        public bool FirstChunk
        {
            get;set;
        }
        public bool LastChunk
        {
            get;set;
        }
        
    }
}
