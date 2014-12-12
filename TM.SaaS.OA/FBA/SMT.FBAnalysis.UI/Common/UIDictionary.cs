using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using SMT.SAAS.ClientUtility;
using System.Collections.Generic;
using SMT.FBAnalysis.UI.Views.DailyManagement;
using SMT.FBAnalysis.UI.Form;
using SMT.FBAnalysis.UI.Views;

namespace SMT.FBAnalysis.UI
{
    public partial class BaseForm
    {
        public BaseForm()
        {
            currentMain = Application.Current.RootVisual as MainPage;
            base.Loaded += new RoutedEventHandler(BaseForm_Loaded);
        }

        void BaseForm_Loaded(object sender, RoutedEventArgs e)
        {
            DictionaryManager dm = new DictionaryManager();
            if (!UIDictionary.DictOfDict.ContainsKey(this.GetType()))
            {
                if (this.Loaded != null)
                {
                    this.Loaded(this, new RoutedEventArgs());
                }
                return;
            }
            dm.OnDictionaryLoadCompleted += (o, args) =>
            {
                if (this.Loaded != null)
                {
                    this.Loaded(o, new RoutedEventArgs());
                }
            };
            dm.LoadDictionary(UIDictionary.DictOfDict[this.GetType()]);
        }

        public new event RoutedEventHandler Loaded;
    }

    public partial class BasePage
    {
        public BasePage()
        {
            SMT.SaaS.FrameworkUI.Validator.ValidatorService.ResourceMgr = SMT.SaaS.Globalization.Localization.ResourceMgr;
            CheckResourceConverter();
            base.Loaded += new RoutedEventHandler(BaseForm_Loaded);
        }

        void BaseForm_Loaded(object sender, RoutedEventArgs e)
        {
            DictionaryManager dm = new DictionaryManager();
            if (!UIDictionary.DictOfDict.ContainsKey(this.GetType()))
            {
                if (this.Loaded != null)
                {
                    this.Loaded(this, new RoutedEventArgs());
                }
                return;
            }
            dm.OnDictionaryLoadCompleted += (o, args) =>
            {
                if (this.Loaded != null)
                {
                    this.Loaded(o, new RoutedEventArgs());
                }
            };
            dm.LoadDictionary(UIDictionary.DictOfDict[this.GetType()]);
        }

        public new event RoutedEventHandler Loaded;
    }

    public static class UIDictionary
    {
        static UIDictionary()
        {
            try
            {
                GetUIDictionary();
            }
            catch (Exception ex)
            {
                MessageBox.Show("1!!" + ex.Message);
            }
        }

        public static Dictionary<Type, List<string>> DictOfDict;

        /// <summary>
        /// 在此处写个子页面需要的字典.
        /// </summary>
        private static void GetUIDictionary()
        {
            List<KeyValuePair<Type, List<string>>> dicts = new List<KeyValuePair<Type, List<string>>>();

            dicts.Add(new KeyValuePair<Type, List<string>>(typeof(BorrowApplyManagement), new List<string> { "CHECKSTATE", "BorrowType" }));
            dicts.Add(new KeyValuePair<Type, List<string>>(typeof(BorrowAppForm), new List<string> { "CHECKSTATE", "BorrowType" }));

            dicts.Add(new KeyValuePair<Type, List<string>>(typeof(RepayApplyManagement), new List<string> { "CHECKSTATE", "REPAYTYPE", "RepayType" }));
            dicts.Add(new KeyValuePair<Type, List<string>>(typeof(RepayApplyForm), new List<string> { "CHECKSTATE", "REPAYTYPE", "RepayType" }));
            //个人费用报销
            dicts.Add(new KeyValuePair<Type, List<string>>(typeof(ChargeApplyManagement), new List<string> { "CHECKSTATE", "PayType", "CHARGEAPPLYISPAYED" }));
            dicts.Add(new KeyValuePair<Type, List<string>>(typeof(ChargeApplyForm), new List<string> { "CHECKSTATE", "PayType" }));

            dicts.Add(new KeyValuePair<Type, List<string>>(typeof(ContactDetails), new List<string> { "CHECKSTATE", "BorrowType", "RepayType" }));
            dicts.Add(new KeyValuePair<Type, List<string>>(typeof(AcountsView), new List<string> { "CHECKSTATE", "BorrowType", "RepayType" }));

            dicts.Add(new KeyValuePair<Type, List<string>>(typeof(StandingBook), new List<string> { "BUDGETORDERTYPE", "ORDERSTATE", "CHECKSTATE" }));

            //dicts.Add(new KeyValuePair<Type, List<string>>(typeof(ChargeApplyManagement), new List<string> { "CHARGEAPPLYISPAYED" }));

            DictOfDict = new Dictionary<Type, List<string>>();

            foreach (var item in dicts)
            {
                if (!DictOfDict.ContainsKey(item.Key))
                {
                    DictOfDict.Add(item.Key, item.Value);
                }
            }

        }
    }
}
