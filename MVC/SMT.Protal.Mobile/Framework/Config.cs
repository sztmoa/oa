using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMT.Portal.Common.SmtForm.Framework
{
    public class Config
    {
        /// <summary>
        /// 分辨率的宽
        /// </summary>
        public static int ScreenWidth { get; set; }
        /// <summary>
        /// 分辨率的高
        /// </summary>
        public static int ScreenHeight { get; set; }
        /// <summary>
        /// TopTitle的高,对应图片Top_Background.png的高度
        /// </summary>
        public static int TopTitleHeight
        {
            get { return 44; }
        }
        /// <summary>
        /// BottomTitle的高,对应图片Bottom_Background.png的高度
        /// </summary>
        public static int BottomTitleHeight
        {
            get { return 49; }
        }
        /// <summary>
        /// 城市图片的高度，对应图片City.png的高度
        /// </summary>
        public static int CityBackgroundHeight
        {
            get { return 101; }
        }
        /// <summary>
        /// 分页行的高度
        /// </summary>
        public static int PagingHeight
        {
            get { return 20; }
        }
        /// <summary>
        /// 待办事项列表和已办事项列表的高度，对应图片List_Background.png的高度
        /// </summary>
        public static int ListHeight
        {
            get { return 45; }
        }

        /// <summary>
        /// 主页上的图片的高度
        /// </summary>
        public static int MainImageHeight
        {
            get { return 73; }
        }
        /// <summary>
        /// 主页上的图片的宽度
        /// </summary>
        public static int MainImageWidth
        {
            get { return 58; }
        }


    }
}