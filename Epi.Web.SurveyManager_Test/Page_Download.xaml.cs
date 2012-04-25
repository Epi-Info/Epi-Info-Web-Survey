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

using System.ServiceModel;
using Epi.Web.Common.Exception;

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
            this.WindowTitle = "Manage Download of Survey and Response Infomation";
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
            catch (FaultException<CustomFaultException> cfe)
            {
                SurveyInfoResponseTextBox.AppendText("FaultException<CustomFaultException>:\n");
                SurveyInfoResponseTextBox.AppendText(cfe.ToString());
            }
            catch (FaultException fe)
            {
                SurveyInfoResponseTextBox.AppendText("FaultException:\n");
                SurveyInfoResponseTextBox.AppendText(fe.ToString());
            }
            catch (CommunicationException ce)
            {
                SurveyInfoResponseTextBox.AppendText("CommunicationException:\n");
                SurveyInfoResponseTextBox.AppendText(ce.ToString());
            }
            catch (TimeoutException te)
            {
                SurveyInfoResponseTextBox.AppendText("TimeoutException:\n");
                SurveyInfoResponseTextBox.AppendText(te.ToString());
            }
            catch (Exception ex)
            {
                SurveyInfoResponseTextBox.AppendText("Exception:\n");
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

                if (!string.IsNullOrEmpty(this.UserPublishKeytextBox.Text.Trim()))
                {
                    Request.Criteria.UserPublishKey = new Guid(this.UserPublishKeytextBox.Text);
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
                    SurveyAnswerResponseTextBox.AppendText(string.Format("{0} - {1} - {2} - {3}\n",SurveyAnswer.ResponseId, SurveyAnswer.Status, SurveyAnswer.DateUpdated, SurveyAnswer.XML));
                }
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                SurveyAnswerResponseTextBox.AppendText("FaultException<CustomFaultException>:\n");
                SurveyAnswerResponseTextBox.AppendText(cfe.ToString());
            }
            catch (FaultException fe)
            {
                SurveyAnswerResponseTextBox.AppendText("FaultException:\n");
                SurveyAnswerResponseTextBox.AppendText(fe.ToString());
            }
            catch (CommunicationException ce)
            {
                SurveyAnswerResponseTextBox.AppendText("CommunicationException:\n");
                SurveyAnswerResponseTextBox.AppendText(ce.ToString());
            }
            catch (TimeoutException te)
            {
                SurveyAnswerResponseTextBox.AppendText("TimeoutException:\n");
                SurveyAnswerResponseTextBox.AppendText(te.ToString());
            }
            catch (Exception ex)
            {
                SurveyAnswerResponseTextBox.AppendText("Exception:\n");
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

        private void ManageSurveyButton_Click(object sender, RoutedEventArgs e)
        {
            Page_ManageSurvey page_ManageSurvey = new Page_ManageSurvey();
            this.NavigationService.Navigate(page_ManageSurvey);
        }

        private void AddUser_click(object sender, RoutedEventArgs e)
        {


            Page_AddUser page_AddUser = new Page_AddUser();
            this.NavigationService.Navigate(page_AddUser);
        }
    }
}
