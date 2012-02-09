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
            this.WindowTitle = "Manage Survey Updates";
        }


        private List<Epi.Web.Common.DTO.SurveyInfoDTO> SurveyInfoList;
        private int selectedIndex;

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
            SearchResultListBox.Items.Clear();
            try
            {
                Epi.Web.Common.Message.SurveyInfoResponse Result = client.GetSurveyInfo(Request);
                SurveyInfoList = Result.SurveyInfoList;
                SearchResultListBox.Items.Clear();



                SurveyInfoResponseTextBox.AppendText(string.Format("{0} - records. \n\n", Result.SurveyInfoList.Count));
                foreach (Epi.Web.Common.DTO.SurveyInfoDTO SurveyInfo in SurveyInfoList)
                {
                    //SurveyInfoResponseTextBox.AppendText(string.Format("{0} - {1} - {2}\n", SurveyInfo.SurveyId, SurveyInfo.SurveyName, SurveyInfo.ClosingDate));
                    //System.Collections.Generic.KeyValuePair<string, string> kvp = new KeyValuePair<string, string>(SurveyInfo.SurveyId,string.Format("{0} - {1} - {2}\n", SurveyInfo.SurveyId, SurveyInfo.SurveyName, SurveyInfo.ClosingDate));

                    SearchResultListBox.Items.Add(string.Format("{0} - {1} - {2}\n", SurveyInfo.SurveyId, SurveyInfo.SurveyName, SurveyInfo.ClosingDate)); 
                    
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



        private void SearchResultListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                this.selectedIndex = this.SearchResultListBox.SelectedIndex;

                this.SurveyNameTextBox.Text = this.SurveyInfoList[this.selectedIndex].SurveyName;
                this.DepartmentTextBox.Text = this.SurveyInfoList[this.selectedIndex].DepartmentName;
                this.SurveyNumberTextBox.Text = this.SurveyInfoList[this.selectedIndex].SurveyNumber;
                this.OrganizationTextBox.Text = this.SurveyInfoList[this.selectedIndex].OrganizationName;
                this.datePicker1.SelectedDate = this.SurveyInfoList[this.selectedIndex].ClosingDate;
                if (this.SurveyInfoList[this.selectedIndex].SurveyType == 1)
                {
                    this.IsSingleResponseCheckBox.IsChecked = true;
                }
                else
                {
                    this.IsSingleResponseCheckBox.IsChecked = false;
                }

        }


        private void UpdateSurveyButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.selectedIndex > -1)
            {
                SurveyInfoResponseTextBox.Document.Blocks.Clear();

                SurveyManagerService.ManagerServiceClient client = new SurveyManagerService.ManagerServiceClient();

                Epi.Web.Common.Message.SurveyInfoRequest Request = new Epi.Web.Common.Message.SurveyInfoRequest();
                Request.Action = "Update";
                if (this.datePicker1.SelectedDate == null)
                {
                    TimeSpan t = new TimeSpan(10, 0, 0, 0);
                    this.SurveyInfoList[this.selectedIndex].ClosingDate = DateTime.Now + t;
                }
                else
                {
                    this.SurveyInfoList[this.selectedIndex].ClosingDate = (DateTime)this.datePicker1.SelectedDate;
                }

                this.SurveyInfoList[this.selectedIndex].DepartmentName = this.DepartmentTextBox.Text;
                //this.SurveyInfoList[this.selectedIndex].IntroductionText = new TextRange(this.IntroductionTextBox.Document.ContentStart, this.IntroductionTextBox.Document.ContentEnd).Text;
                //this.SurveyInfoList[this.selectedIndex].ExitText = new TextRange(this.ExitTextTextBox.Document.ContentStart, this.ExitTextTextBox.Document.ContentEnd).Text;
                if ((bool)this.IsSingleResponseCheckBox.IsChecked)
                {
                    this.SurveyInfoList[this.selectedIndex].SurveyType = 1;
                }
                else
                {
                    this.SurveyInfoList[this.selectedIndex].SurveyType = 2;
                }

                this.SurveyInfoList[this.selectedIndex].OrganizationName = this.OrganizationTextBox.Text;
                this.SurveyInfoList[this.selectedIndex].SurveyName = this.SurveyNameTextBox.Text;
                this.SurveyInfoList[this.selectedIndex].SurveyNumber = this.SurveyNumberTextBox.Text;
                //this.SurveyInfoList[this.selectedIndex].XML = new TextRange(this.TemplateXMLTextBox.Document.ContentStart, this.TemplateXMLTextBox.Document.ContentEnd).Text;

                Request.SurveyInfoList.Add(this.SurveyInfoList[this.selectedIndex]);

                try
                {
                    Epi.Web.Common.Message.SurveyInfoResponse Result = client.SetSurveyInfo(Request);


                    SurveyInfoResponseTextBox.AppendText("Succefully updated survey:");
                    //SurveyInfoResponseTextBox.AppendText(Result.PublishInfo.IsPulished.ToString());
                    //SurveyInfoResponseTextBox.AppendText("\nURL: ");
                    //SurveyInfoResponseTextBox.AppendText(Result.PublishInfo.URL);
                    //SurveyInfoResponseTextBox.AppendText("\nStatus Text: ");
                    //SurveyInfoResponseTextBox.AppendText(Result.PublishInfo.StatusText);

                    

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
        }
    }
}
