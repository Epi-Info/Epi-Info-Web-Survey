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

        private void DownloadSurveyInfoButton_Click(object sender, RoutedEventArgs e)
        {
            SurveyManagerService.ManagerServiceClient client = new SurveyManagerService.ManagerServiceClient();

            Epi.Web.Common.Message.SurveyInfoRequest Request = new Epi.Web.Common.Message.SurveyInfoRequest();
            
            if (!string.IsNullOrEmpty(this.SurveyCriteria_SurveyId.Text.Trim()))
            {
                Request.Criteria.SurveyIdList.Add(this.SurveyCriteria_SurveyId.Text);
            }

            if ((bool)this.SurveyCriteria_CurrentlyOpenCheckBox.IsChecked)
            {
                Request.Criteria.ClosingDate = DateTime.Now;
            }

            if (this.SurveyInfoCriteria_SurveyTypeListBox.SelectedIndex > -1)
            {
                Request.Criteria.SurveyType = int.Parse(((ListBoxItem)this.SurveyInfoCriteria_SurveyTypeListBox.Items[this.SurveyInfoCriteria_SurveyTypeListBox.SelectedIndex]).Tag.ToString());
            }


            SurveyInfoResponseTextBox.Document.Blocks.Clear();
            try
            {
                Epi.Web.Common.Message.SurveyInfoResponse Result = client.GetSurveyInfo(Request);

                SurveyInfoResponseTextBox.AppendText(string.Format("{0} - records. \n\n", Result.SurveyInfoList.Count));
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

        private void DownloadSurveyAnswersButton_Click(object sender, RoutedEventArgs e)
        {
            SurveyManagerService.ManagerServiceClient client = new SurveyManagerService.ManagerServiceClient();

            Epi.Web.Common.Message.SurveyAnswerRequest Request = new Epi.Web.Common.Message.SurveyAnswerRequest();

            foreach (string id in SurveyAnswerIdListBox.Items)
            {
                Request.Criteria.SurveyAnswerIdList.Add(id);
            }

            if (!string.IsNullOrEmpty(this.SurveyAnswerCriteria_SurveyIdTextBox.Text.Trim()))
            {
                Request.Criteria.SurveyId = this.SurveyAnswerCriteria_SurveyIdTextBox.Text;
            }

            if (this.datePicker1.SelectedDate != null)
            {
                Request.Criteria.DateCompleted = (DateTime)this.datePicker1.SelectedDate;
            }

            if ((bool)this.OnlyCompletedCheckBox.IsChecked)
            {
                Request.Criteria.StatusId = 1;
            }

            SurveyAnswerResponseTextBox.Document.Blocks.Clear();
            try
            {
                Epi.Web.Common.Message.SurveyAnswerResponse Result = client.GetSurveyAnswer(Request);

                SurveyAnswerResponseTextBox.AppendText(string.Format("{0} - records.\n\n", Result.SurveyResponseList.Count));
                foreach (Epi.Web.Common.DTO.SurveyAnswerDTO SurveyAnswer in Result.SurveyResponseList)
                {
                    SurveyAnswerResponseTextBox.AppendText(string.Format("{0} - {1} - {2}\n",SurveyAnswer.ResponseId, SurveyAnswer.Status, SurveyAnswer.DateLastUpdated));
                }
            }
            catch (Exception ex)
            {
                SurveyAnswerResponseTextBox.AppendText("error:\n");
                SurveyAnswerResponseTextBox.AppendText(ex.ToString());
            }
        }

        private void ClearListButton_Click(object sender, RoutedEventArgs e)
        {
            this.SurveyAnswerIdListBox.Items.Clear();
            this.datePicker1.SelectedDate = null;
            this.SurveyAnswerCriteria_SurveyIdTextBox.Text = "";
            this.AddAnswerIdTextBox.Text = "";
            this.OnlyCompletedCheckBox.IsChecked = false;
        }

        private void AddSurveyAnswerIdButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(AddAnswerIdTextBox.Text))
            {
                this.SurveyAnswerIdListBox.Items.Add(AddAnswerIdTextBox.Text);
            }
        }

        private void ClearSurveyCriteriaButton_Click(object sender, RoutedEventArgs e)
        {
            this.SurveyCriteria_SurveyId.Text = "";
            this.SurveyInfoCriteria_SurveyTypeListBox.SelectedIndex = -1;
            this.SurveyCriteria_CurrentlyOpenCheckBox.IsChecked = false;
        }
    }
}
