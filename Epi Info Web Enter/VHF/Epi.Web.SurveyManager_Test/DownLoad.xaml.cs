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
using Epi.Web.Enter.Common.Exception;
using System.Text.RegularExpressions;

namespace Epi.Web.SurveyManager.Client
{
    /// <summary>
    /// Interaction logic for Page_Download.xaml
    /// </summary>
    public partial class DownLoad : Page
    {
        public DownLoad()
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
        EWEManagerService.EWEManagerServiceClient client = ServiceClient.GetClient();

            Epi.Web.Enter.Common.Message.SurveyInfoRequest Request = new Epi.Web.Enter.Common.Message.SurveyInfoRequest();


            //Checking the Organization key guid is in correct format
            if (!IsGuid(passOrganizationKey.Password))
            {
                MessageBox.Show("Organization key is not in correct format");
                return;
            }
            //Assign the organization key
            Request.Criteria.OrganizationKey = new Guid(passOrganizationKey.Password);



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

            int PageNumber = 0;
            int PageSize = 0;

            try
            {
                if (this.chkIsSizeRequestSurveyInfo.IsChecked == true)
                {
                    Request.Criteria.ReturnSizeInfoOnly = true;
                    Epi.Web.Enter.Common.Message.SurveyInfoResponse Result = client.GetSurveyInfo(Request);
                    if (!string.IsNullOrEmpty(Result.Message))
                    {
                        SurveyInfoResponseTextBox.AppendText(string.Format(Result.Message));
                        
                    
                    }else{
                    SurveyInfoResponseTextBox.AppendText(string.Format(" - Number of Pages: {0}   \n\n", Result.NumberOfPages));
                    SurveyInfoResponseTextBox.AppendText(string.Format(" - Pages Size:   {0} \n ", Result.PageSize));
                    }
                }
                else 
                {

                    // 2 Step process:
                    //      1 - get sizing information.
                    //      2 - Loop thru calls for query results


                    // 1 - get sizing information
                    Request.Criteria.ReturnSizeInfoOnly = true;
                    Epi.Web.Enter.Common.Message.SurveyInfoResponse SizeResult = client.GetSurveyInfo(Request);


                    if (!string.IsNullOrEmpty(SizeResult.Message))
                    {
                        SurveyInfoResponseTextBox.AppendText(string.Format(SizeResult.Message));

                    }
                    else
                    {
                        SurveyInfoResponseTextBox.AppendText(string.Format(" - Number of Pages: {0}   \n\n", SizeResult.NumberOfPages));
                        SurveyInfoResponseTextBox.AppendText(string.Format(" - Pages Size:   {0}  \n", SizeResult.PageSize));

                        // 2 - loop thru calls for query results
                        PageSize = SizeResult.PageSize;
                        PageNumber = SizeResult.NumberOfPages;
                        Request.Criteria.ReturnSizeInfoOnly = false;

                        for (int i = 1; i <= PageNumber; i++)
                        {
                            Request.Criteria.PageNumber = i;

                            Epi.Web.Enter.Common.Message.SurveyInfoResponse Result = client.GetSurveyInfo(Request);

                            foreach (Epi.Web.Enter.Common.DTO.SurveyInfoDTO SurveyInfo in Result.SurveyInfoList)
                            {
                                SurveyInfoResponseTextBox.AppendText(string.Format("{0} - {1} - {2}\n", SurveyInfo.SurveyId, SurveyInfo.SurveyName, SurveyInfo.ClosingDate));
                            }
                        }
                    }
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
        EWEManagerService.EWEManagerServiceClient client = ServiceClient.GetClient();

            Epi.Web.Enter.Common.Message.SurveyAnswerRequest Request = new Epi.Web.Enter.Common.Message.SurveyAnswerRequest();

            foreach (string id in SurveyAnswerIdListBox.Items)
            {
                Request.Criteria.SurveyAnswerIdList.Add(id);
            }

            if (!string.IsNullOrEmpty(this.SurveyAnswerCriteria_SurveyIdTextBox.Text.Trim()))
            {
                Request.Criteria.SurveyId = this.SurveyAnswerCriteria_SurveyIdTextBox.Text;
            }

            if (!string.IsNullOrEmpty(this.passUserPublishKeySurveyResponse.Password.Trim()) && IsGuid(passUserPublishKeySurveyResponse.Password))
            {
                Request.Criteria.UserPublishKey = new Guid(this.passUserPublishKeySurveyResponse.Password);

            }
            else
            {
                MessageBox.Show("Publish key is not in correct format");
                return;
            }

            if (this.datePicker1.SelectedDate != null)
            {
                Request.Criteria.DateCompleted = (DateTime)this.datePicker1.SelectedDate;
            }

            if ((bool)this.OnlyCompletedCheckBox.IsChecked)
            {
                Request.Criteria.StatusId = 3;
            }
            //chkIsSizeRequestSurveyResponse



            //Checking the Organization key guid is in correct format
            if (!IsGuid(passOrganizationKey.Password))
            {
                MessageBox.Show("Organization key is not in correct format");
                return;
            }
            //Assign the organization key
            Request.Criteria.OrganizationKey = new Guid(passOrganizationKey.Password);



             
            SurveyAnswerResponseTextBox.Document.Blocks.Clear();
            int PageNumber = 0;
            int PageSize = 0;

            try
            {



                if (this.chkIsSizeRequestSurveyResponse.IsChecked == true)
                {
                    Request.Criteria.ReturnSizeInfoOnly = true;
                    Epi.Web.Enter.Common.Message.SurveyAnswerResponse Result = client.GetSurveyAnswer(Request);

                    if (!string.IsNullOrEmpty(Result.Message))
                    {
                        SurveyAnswerResponseTextBox.AppendText(string.Format(Result.Message));
                    }
                    else
                    {
                        SurveyAnswerResponseTextBox.AppendText(string.Format(" - Number of Pages: {0}   \n\n", Result.NumberOfPages));
                        SurveyAnswerResponseTextBox.AppendText(string.Format(" - Pages Size:   {0}  ", Result.PageSize));
                    }

                }
                else
                {
                    Request.Criteria.ReturnSizeInfoOnly = true;
                     Epi.Web.Enter.Common.Message.SurveyAnswerResponse SizeResult = client.GetSurveyAnswer(Request);

                    if (!string.IsNullOrEmpty(SizeResult.Message))
                    {
                        SurveyAnswerResponseTextBox.AppendText(string.Format(SizeResult.Message));
                    }
                    else
                    {
                        PageSize = SizeResult.PageSize;
                        Request.Criteria.ReturnSizeInfoOnly = false;

                        SurveyAnswerResponseTextBox.AppendText(string.Format(" - Number of Pages: {0}   \n\n", SizeResult.NumberOfPages));
                        SurveyAnswerResponseTextBox.AppendText(string.Format(" - Pages Size:   {0}  \n", SizeResult.PageSize));





                        for (int i = 1; i <= SizeResult.NumberOfPages; i++)
                        {
                            Request.Criteria.PageNumber = i;
                            Request.Criteria.PageSize = PageSize;
                            Epi.Web.Enter.Common.Message.SurveyAnswerResponse Result = client.GetSurveyAnswer(Request);
                            SurveyAnswerResponseTextBox.AppendText(string.Format(" -Number of available records: {0}\n\n", Result.SurveyResponseList.Count));
                            foreach (Epi.Web.Enter.Common.DTO.SurveyAnswerDTO SurveyAnswer in Result.SurveyResponseList)
                            {
                                SurveyAnswerResponseTextBox.AppendText(string.Format("{0} - {1} - {2} - {3}\n", SurveyAnswer.ResponseId, SurveyAnswer.Status, SurveyAnswer.DateUpdated, SurveyAnswer.XML));
                            }
                        }
                    }
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

        private bool IsGuid(string expression)
        {
            if (expression != null)
            {
                Regex guidRegEx = new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");

                return guidRegEx.IsMatch(expression);
            }
            return false;
        }

        private void passOrganizationKey_PasswordChanged(object sender, RoutedEventArgs e)
        {

        }

    }
}
