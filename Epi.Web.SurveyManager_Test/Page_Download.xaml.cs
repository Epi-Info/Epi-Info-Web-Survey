using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Epi.Web.SurveyManager.Client
{
    /// <summary>
    /// Interaction logic for Page_Download.xaml
    /// </summary>
    public partial class Page_Download : Page
    {
        public Page_Download()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Page_Publish page_Publish = new Page_Publish();
            this.NavigationService.Navigate(page_Publish);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            SurveyManagerService.ManagerServiceClient client = new SurveyManagerService.ManagerServiceClient();

            Epi.Web.Common.Message.SurveyInfoRequest Request = new Epi.Web.Common.Message.SurveyInfoRequest();
            
            if (!string.IsNullOrEmpty(this.SurveyCriteria_SurveyId.Text))
            {
                Request.Criteria.SurveyId = this.SurveyCriteria_SurveyId.Text;
            }

            new TextRange(SurveyInfoResponseTextBox.Document.ContentStart, SurveyInfoResponseTextBox.Document.ContentEnd).Text = "";
            try
            {
                Epi.Web.Common.Message.SurveyInfoResponse Result = client.GetSurveyInfo(Request);

                SurveyInfoResponseTextBox.AppendText(string.Format("{0} - records.\n", Result.SurveyInfoList.Count));
                foreach(Epi.Web.Common.DTO.SurveyInfoDTO SurveyInfo in Result.SurveyInfoList)
                {
                    SurveyInfoResponseTextBox.AppendText(string.Format("{0} - {1} - {2}\n", SurveyInfo.SurveyId, SurveyInfo.SurveyName, SurveyInfo.ClosingDate));
                }
            }
            catch (Exception ex)
            {
                SurveyInfoResponseTextBox.AppendText("error:\n");
                SurveyInfoResponseTextBox.AppendText(ex.ToString());
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            SurveyManagerService.ManagerServiceClient client = new SurveyManagerService.ManagerServiceClient();

            Epi.Web.Common.Message.SurveyAnswerRequest Request = new Epi.Web.Common.Message.SurveyAnswerRequest();

            if (!string.IsNullOrEmpty(this.SurveyAnswerCriteria_SurveyIdTextBox.Text))
            {
                Request.Criteria.SurveyId = this.SurveyAnswerCriteria_SurveyIdTextBox.Text;
            }

            new TextRange(SurveyAnswerResponseTextBox.Document.ContentStart, SurveyAnswerResponseTextBox.Document.ContentEnd).Text = "";
            
            try
            {
                Epi.Web.Common.Message.SurveyAnswerResponse Result = client.GetSurveyAnswer(Request);

                SurveyAnswerResponseTextBox.AppendText(string.Format("{0} - records .\n", Result.SurveyResponseList.Count));
                foreach (Epi.Web.Common.DTO.SurveyAnswerDTO SurveyAnswer in Result.SurveyResponseList)
                {
                    SurveyAnswerResponseTextBox.AppendText(string.Format("{0} - {1} - {2}\n",SurveyAnswer.SurveyId, SurveyAnswer.Status, SurveyAnswer.DateLastUpdated));
                }
            }
            catch (Exception ex)
            {
                SurveyAnswerResponseTextBox.AppendText("error:\n");
                SurveyAnswerResponseTextBox.AppendText(ex.ToString());
            }
        }
    }
}
