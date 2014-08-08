using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Activation;
using System.ServiceModel;

namespace mpost.FileUploadServiceLibrary
{
   
    public class SilverlightUploadService : mpost.FileUploadServiceLibrary.UploadService
    {
        protected override void FinishedFileUpload(string fileName, string parameters)
        {
            base.FinishedFileUpload(fileName, parameters);

            //Do your work here
        }

        protected override string GetUploadFolder()
        {
            return "yourFolder";
        }
    }
}
