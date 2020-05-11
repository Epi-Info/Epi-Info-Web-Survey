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
using System.Text.RegularExpressions;
using System.Configuration;

namespace Epi.Web.SurveyManager.Client
{
    /// <summary>
    /// Interaction logic for Page_Publish.xaml
    /// </summary>
    public partial class Page_Publish : Page
    {
        String URL;
        public Guid UserPublishKey;
        
        public Page_Publish()
        {
            InitializeComponent();
            TimeSpan t = new TimeSpan(10, 0, 0, 0);
            this.datePicker1.SelectedDate = DateTime.Now + t;
            this.WindowTitle = "Publish Survey";

           
        }

        private void SubmitRequestButton_Click(object sender, RoutedEventArgs e)
        {

            ServiceResponseTextBox.Document.Blocks.Clear();
            this.OpenURLButton.IsEnabled = false;

           // SurveyManagerService.ManagerServiceClient client = ServiceClient.GetClient();

            Epi.Web.Common.Message.PublishRequest Request = new Epi.Web.Common.Message.PublishRequest();
            if (this.datePicker1.SelectedDate == null)
            {
                TimeSpan t = new TimeSpan(10, 0, 0, 0);
                Request.SurveyInfo.ClosingDate = DateTime.Now + t;
            }
            else
            {
                Request.SurveyInfo.ClosingDate = (DateTime)this.datePicker1.SelectedDate;
            }

            Request.SurveyInfo.DepartmentName = this.DepartmentTextBox.Text;
            Request.SurveyInfo.IntroductionText = new TextRange(this.IntroductionTextBox.Document.ContentStart, this.IntroductionTextBox.Document.ContentEnd).Text;
            Request.SurveyInfo.ExitText = new TextRange(this.ExitTextTextBox.Document.ContentStart, this.ExitTextTextBox.Document.ContentEnd).Text;
            if ((bool)this.IsSingleResponseCheckBox.IsChecked)
            {
                Request.SurveyInfo.SurveyType = 1;
            }
            else
            {
                Request.SurveyInfo.SurveyType = 2;
            }

            Request.SurveyInfo.OrganizationName = this.OrganizationTextBox.Text;
            Request.SurveyInfo.SurveyName = this.SurveyNameTextBox.Text;
            Request.SurveyInfo.SurveyNumber = this.SurveyNumberTextBox.Text;
            Request.SurveyInfo.XML = new TextRange(this.TemplateXMLTextBox.Document.ContentStart, this.TemplateXMLTextBox.Document.ContentEnd).Text;
           
            //Checking the publish key guid is in correct format
            if (!IsGuid(txtPublishKey.Text))
            {
                MessageBox.Show("Publish key is not in correct format");
                return;
            }

            Request.SurveyInfo.UserPublishKey = new Guid(txtPublishKey.Text);
            //get the Organization key and assign it to SurveyInfoDTO object under PublishRequest
            string strOrganizationKey = passOrganizationKey.Password.ToString();
            if (!IsGuid(strOrganizationKey))
            {
                MessageBox.Show("Organization key is not in correct format");
                return;
            }
            Guid gOrganizationkey = new Guid(strOrganizationKey);
            Request.SurveyInfo.OrganizationKey = gOrganizationkey;
            try
            {


            int ServiceVersion =   ServiceClient.GetServiceVersion();

            if (ServiceVersion == 1)
                {
                SurveyManagerService.ManagerServiceClient client = ServiceClient.GetClient();
                Epi.Web.Common.Message.PublishResponse Result = client.PublishSurvey(Request);

                passOrganizationKey.Password = string.Empty;
                URL = Result.PublishInfo.URL;
                ServiceResponseTextBox.AppendText("is published: ");
                ServiceResponseTextBox.AppendText(Result.PublishInfo.IsPulished.ToString());
                ServiceResponseTextBox.AppendText("\nURL: ");
                ServiceResponseTextBox.AppendText(Result.PublishInfo.URL);
                ServiceResponseTextBox.AppendText("\nStatus Text: ");
                ServiceResponseTextBox.AppendText(Result.PublishInfo.StatusText);
                ServiceResponseTextBox.AppendText("\n User Publish Key: ");
                ServiceResponseTextBox.AppendText(UserPublishKey.ToString());
                this.OpenURLButton.IsEnabled = Result.PublishInfo.IsPulished;
                }

            else if (ServiceVersion == 2)
                {
                SurveyManagerServiceV2.ManagerServiceV2Client client = ServiceClient.GetClientV2();
                Epi.Web.Common.Message.PublishResponse Result = client.PublishSurvey(Request);

                passOrganizationKey.Password = string.Empty;
                URL = Result.PublishInfo.URL;
                ServiceResponseTextBox.AppendText("is published: ");
                ServiceResponseTextBox.AppendText(Result.PublishInfo.IsPulished.ToString());
                ServiceResponseTextBox.AppendText("\nURL: ");
                ServiceResponseTextBox.AppendText(Result.PublishInfo.URL);
                ServiceResponseTextBox.AppendText("\nStatus Text: ");
                ServiceResponseTextBox.AppendText(Result.PublishInfo.StatusText);
                ServiceResponseTextBox.AppendText("\n User Publish Key: ");
                ServiceResponseTextBox.AppendText(UserPublishKey.ToString());
                this.OpenURLButton.IsEnabled = Result.PublishInfo.IsPulished;
                }
            else if (ServiceVersion == 3)
            {
                SurveyManagerServiceV3.ManagerServiceV3Client client = ServiceClient.GetClientV3();
                Epi.Web.Common.Message.PublishResponse Result = client.PublishSurvey(Request);

                passOrganizationKey.Password = string.Empty;
                URL = Result.PublishInfo.URL;
                ServiceResponseTextBox.AppendText("is published: ");
                ServiceResponseTextBox.AppendText(Result.PublishInfo.IsPulished.ToString());
                ServiceResponseTextBox.AppendText("\nURL: ");
                ServiceResponseTextBox.AppendText(Result.PublishInfo.URL);
                ServiceResponseTextBox.AppendText("\nStatus Text: ");
                ServiceResponseTextBox.AppendText(Result.PublishInfo.StatusText);
                ServiceResponseTextBox.AppendText("\n User Publish Key: ");
                ServiceResponseTextBox.AppendText(UserPublishKey.ToString());
                this.OpenURLButton.IsEnabled = Result.PublishInfo.IsPulished;
            }




             
                
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                ServiceResponseTextBox.AppendText("FaultException<CustomFaultException>:\n");
                ServiceResponseTextBox.AppendText(cfe.ToString());
            }
            catch (FaultException fe)
            {
                ServiceResponseTextBox.AppendText("FaultException:\n");
                ServiceResponseTextBox.AppendText(fe.ToString());
            }
            catch (CommunicationException ce)
            {
                ServiceResponseTextBox.AppendText("CommunicationException:\n");
                ServiceResponseTextBox.AppendText(ce.ToString());
            }
            catch (TimeoutException te)
            {
                ServiceResponseTextBox.AppendText("TimeoutException:\n");
                ServiceResponseTextBox.AppendText(te.ToString());
            }
            catch (Exception ex)
            {
                ServiceResponseTextBox.AppendText("Exception:\n");
                ServiceResponseTextBox.AppendText(ex.ToString());
            }
        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            DownLoad page_Download = new DownLoad();
            this.NavigationService.Navigate(page_Download);
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

        private void ClearTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            this.TemplateFileLabel.Content = "No Survey Template file is selected.";
            TemplateXMLTextBox.Document.Blocks.Clear();
        }

        private void ClearIntroButton_Click(object sender, RoutedEventArgs e)
        {
            this.IntroFileLabel.Content = "No Intro Text file is selected.";
            IntroductionTextBox.Document.Blocks.Clear();
        }

        private void OpenURLButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(URL))
            {
                using (WebBrowser browser = new WebBrowser())
                {
                    browser.Navigate(
                      URL,
                      "Epi.Web Demo", 
                      null, null
                    );
                }
            }
        }

        private void ManageSurveyButton_Click(object sender, RoutedEventArgs e)
        {
            Page_ManageSurvey page_ManageSurvey = new Page_ManageSurvey();
            this.NavigationService.Navigate(page_ManageSurvey);
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

        private void AddOrganization_click(object sender, RoutedEventArgs e)
        {


            Page_AddOrganization Page_AddOrganization = new Page_AddOrganization();
            this.NavigationService.Navigate(Page_AddOrganization);
        }

        private  bool IsGuid(string expression)
        {
            if (expression != null)
            {
                Regex guidRegEx = new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");

                return guidRegEx.IsMatch(expression);
            }
            return false;
        }

        

        private void btnGeneratePublishkey_Click(object sender, RoutedEventArgs e)
        {
            //generate the publish key guid for simulation
            UserPublishKey = Guid.NewGuid();
            txtPublishKey.Text = UserPublishKey.ToString();
        }
    }
}
