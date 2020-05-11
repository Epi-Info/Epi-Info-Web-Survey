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
using System.Xml;
using System.ServiceModel;
using Epi.Web.Common.Exception;
using System.Configuration;
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
            
            //if (ServiceVersion == "V1")
            //    {
            //    SurveyManagerService.ManagerServiceClient client = ServiceClient.GetClient();
            //    }

            //else if (ServiceVersion == "V2")
            //    {
            //    SurveyManagerServiceV2.ManagerServiceV2Client client = ServiceClient.GetClientV2();
            //    }
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

            if (!string.IsNullOrEmpty(this.OrgTextBox1.Text))
            {

                Request.Criteria.OrganizationKey = new Guid(OrgTextBox1.Text);
            }
            else { 
            
            }
            if (!string.IsNullOrEmpty(this.TextBoxPublish.Text))
            {
                Request.Criteria.UserPublishKey = new Guid(this.TextBoxPublish.Text);

            }
            else { 
            
                }
            SurveyInfoResponseTextBox.Document.Blocks.Clear();
            SearchResultListBox.Items.Clear();

            this.selectedIndex = -1;

            this.SurveyNameTextBox.Text = "";
            this.DepartmentTextBox.Text = "";
            this.SurveyNumberTextBox.Text = "";
            this.OrganizationTextBox.Text = "";
            this.datePicker1.SelectedDate = DateTime.Now;
            this.IsSingleResponseCheckBox.IsChecked = false;
            this.IsTestMode.IsChecked = false;
            this.IntroductionTextBox.Document.Blocks.Clear();
            this.ExitTextTextBox.Document.Blocks.Clear();
            this.TemplateXMLTextBox.Document.Blocks.Clear();
            this.datePicker2.SelectedDate = DateTime.Now;
            try
            {



            int ServiceVersion = ServiceClient.GetServiceVersion();

            if (ServiceVersion == 1)
                {
                SurveyManagerService.ManagerServiceClient client = ServiceClient.GetClient();
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

            else if (ServiceVersion == 2)
                {
                SurveyManagerServiceV2.ManagerServiceV2Client client = ServiceClient.GetClientV2();
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


            else if (ServiceVersion == 3)
            {
                SurveyManagerServiceV3.ManagerServiceV3Client client = ServiceClient.GetClientV3();
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
            DownLoad page_Download = new DownLoad();
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
                if (this.selectedIndex > -1)
                {

                    this.SurveyNameTextBox.Text = this.SurveyInfoList[this.selectedIndex].SurveyName;
                    this.DepartmentTextBox.Text = this.SurveyInfoList[this.selectedIndex].DepartmentName;
                    this.SurveyNumberTextBox.Text = this.SurveyInfoList[this.selectedIndex].SurveyNumber;
                    this.OrganizationTextBox.Text = this.SurveyInfoList[this.selectedIndex].OrganizationName;
                    this.datePicker1.SelectedDate = this.SurveyInfoList[this.selectedIndex].ClosingDate;
                    this.datePicker2.SelectedDate = this.SurveyInfoList[this.selectedIndex].StartDate;
                    this.IntroductionTextBox.AppendText( this.SurveyInfoList[this.selectedIndex].IntroductionText);
                    this.ExitTextTextBox.AppendText(this.SurveyInfoList[this.selectedIndex].ExitText);
                    this.TemplateXMLTextBox.AppendText(this.SurveyInfoList[this.selectedIndex].XML);
                    this.TextBoxPublish.Text = this.SurveyInfoList[this.selectedIndex].UserPublishKey.ToString();
                    if (this.SurveyInfoList[this.selectedIndex].SurveyType == 1)
                    {
                        this.IsSingleResponseCheckBox.IsChecked = true;
                    }
                    else
                    {
                        this.IsSingleResponseCheckBox.IsChecked = false;
                    }
                    if (this.SurveyInfoList[this.selectedIndex].IsDraftMode == true)
                    {
                        this.IsTestMode.IsChecked = true;
                    }
                    else
                    {
                        this.IsTestMode.IsChecked = false;
                    }
                }

        }


        private void UpdateSurveyButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.selectedIndex > -1)
            {
                SurveyInfoResponseTextBox.Document.Blocks.Clear();

               

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
                this.SurveyInfoList[this.selectedIndex].IntroductionText = new TextRange(this.IntroductionTextBox.Document.ContentStart, this.IntroductionTextBox.Document.ContentEnd).Text;
                this.SurveyInfoList[this.selectedIndex].ExitText = new TextRange(this.ExitTextTextBox.Document.ContentStart, this.ExitTextTextBox.Document.ContentEnd).Text;
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
                 
                if ( this.IsTestMode.IsChecked == true)
                {
                     
                    this.SurveyInfoList[this.selectedIndex].IsDraftMode = true;
                }
                else
                {
                    this.SurveyInfoList[this.selectedIndex].IsDraftMode = false;
                }
                this.SurveyInfoList[this.selectedIndex].XML = new TextRange(this.TemplateXMLTextBox.Document.ContentStart, this.TemplateXMLTextBox.Document.ContentEnd).Text;
                if (!string.IsNullOrEmpty(this.OrgTextBox1.Text))
                { 
                    this.SurveyInfoList[this.selectedIndex].OrganizationKey = new Guid(OrgTextBox1.Text);
                }
                else
                {

                }
                if (!string.IsNullOrEmpty(this.TextBoxPublish.Text))
                { 
                    this.SurveyInfoList[this.selectedIndex].UserPublishKey = new Guid(this.TextBoxPublish.Text);
                }
                else
                {

                }
                Request.SurveyInfoList.Add(this.SurveyInfoList[this.selectedIndex]);

                try
                {

                int ServiceVersion = ServiceClient.GetServiceVersion();

                if (ServiceVersion == 1)
                    {
                    SurveyManagerService.ManagerServiceClient client = ServiceClient.GetClient();
                    Epi.Web.Common.Message.SurveyInfoResponse Result = client.SetSurveyInfo(Request);

                    SurveyInfoResponseTextBox.AppendText(Result.Message);
                    }

                else if (ServiceVersion == 2)
                    {
                    SurveyManagerServiceV2.ManagerServiceV2Client client = ServiceClient.GetClientV2();
                    Epi.Web.Common.Message.SurveyInfoResponse Result = client.SetSurveyInfo(Request);

                    SurveyInfoResponseTextBox.AppendText(Result.Message);
                    }

                else if (ServiceVersion == 3)
                {
                    SurveyManagerServiceV3.ManagerServiceV3Client client = ServiceClient.GetClientV3();
                    Epi.Web.Common.Message.SurveyInfoResponse Result = client.SetSurveyInfo(Request);

                    SurveyInfoResponseTextBox.AppendText(Result.Message);
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
        }



        private void AddOrganization_click(object sender, RoutedEventArgs e) {


            Page_AddOrganization Page_AddOrganization = new Page_AddOrganization();
            this.NavigationService.Navigate(Page_AddOrganization);
        }

        private void SurveyInfoCriteria_SurveyTypeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void ClearTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            this.TemplateFileLabel.Content = "No Survey Template file is selected.";
            TemplateXMLTextBox.Document.Blocks.Clear();
        }
        private void FindFileButton_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "xml documents (.xml)|*.xml"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;

                this.TemplateFileLabel.Content = filename;

                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.Load(filename);

                TemplateXMLTextBox.Document.Blocks.Clear();
                TemplateXMLTextBox.AppendText(xmlDoc.InnerXml);
            }
        }
        private void ClearIntroButton_Click(object sender, RoutedEventArgs e)
        {
            this.IntroFileLabel.Content = "No Intro Text file is selected.";
            IntroductionTextBox.Document.Blocks.Clear();
        }
        private void FindIntroTextButton_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name
            dlg.DefaultExt = ""; // Default file extension
            dlg.Filter = "html or text documents |*.*"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;

                this.IntroFileLabel.Content = filename;
                System.IO.TextReader textReader = System.IO.File.OpenText(filename);


                IntroductionTextBox.Document.Blocks.Clear();
                IntroductionTextBox.AppendText(textReader.ReadToEnd());

                textReader.Close();

            }
        }
        private void FindExtiTextButton_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; // Default file name
            dlg.DefaultExt = ""; // Default file extension
            dlg.Filter = "html or text documents |*.*"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;

                this.ExitTextLabel.Content = filename;
                System.IO.TextReader textReader = System.IO.File.OpenText(filename);


                ExitTextTextBox.Document.Blocks.Clear();
                ExitTextTextBox.AppendText(textReader.ReadToEnd());

                textReader.Close();

            }
        }
        private void ClearExitTextButton_Click(object sender, RoutedEventArgs e)
        {
            this.ExitTextLabel.Content = "No Exit Text file is selected.";
            ExitTextTextBox.Document.Blocks.Clear();
        }

    }
}
