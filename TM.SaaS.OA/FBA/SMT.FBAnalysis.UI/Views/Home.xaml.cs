namespace SMT.FBAnalysis.UI
{
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using SMT.FBAnalysis.ClientServices.FBAnalysisWS;
    using SMT.FBAnalysis.ClientServices.DailyUpdateCheckStateWS;

    public partial class Home : Page
    {

        public Home()
        {
            InitializeComponent();            
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //DailyUpdateCheckStateServiceClient css = new DailyUpdateCheckStateServiceClient();
            //css.UpdateCheckStateCompleted += new System.EventHandler<UpdateCheckStateCompletedEventArgs>(css_UpdateCheckStateCompleted);
            //css.UpdateCheckStateAsync(null, null, null, null);
        }

        void css_UpdateCheckStateCompleted(object sender, UpdateCheckStateCompletedEventArgs e)
        {
            if (e.Error != null)
            { 
            }
        }

    }
}
