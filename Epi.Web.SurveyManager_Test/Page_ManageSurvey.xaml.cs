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
    /// Interaction logic for Page_ManageSurvey.xaml
    /// </summary>
    public partial class Page_ManageSurvey : Page
    {
        public Page_ManageSurvey()
        {
            InitializeComponent();
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
                foreach (Epi.Web.Common.DTO.SurveyInfoDTO SurveyInfo in Result.SurveyInfoList)
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

        private void ClearSurveyCriteriaButton_Click(object sender, RoutedEventArgs e)
        {
            this.SurveyCriteria_SurveyId.Text = "";
            this.SurveyInfoCriteria_SurveyTypeListBox.SelectedIndex = -1;
            this.SurveyCriteria_CurrentlyOpenCheckBox.IsChecked = false;
        }


        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            Page_Download page_Download = new Page_Download();
            this.NavigationService.Navigate(page_Download);
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Page_Publish page_Publish = new Page_Publish();
            this.NavigationService.Navigate(page_Publish);
        }
    }
}
