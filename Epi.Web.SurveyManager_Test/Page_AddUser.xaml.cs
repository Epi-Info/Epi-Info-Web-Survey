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
using System.Configuration;
using Epi.Web.Common.Security;
using System.Security;
using System.Text.RegularExpressions;
 
namespace Epi.Web.SurveyManager.Client
{
    /// <summary>
    /// Interaction logic for AddUser.xaml
    /// </summary>
    public partial class Page_AddUser : Page
    {
        private static string _ConfigurationAdminCode;
         
        public Page_AddUser()
        {
            InitializeComponent();
            this.WindowTitle = "Add user";

            string s = ConfigurationManager.AppSettings["SHOW_TESTING_FEATURES"];
            if (!String.IsNullOrEmpty(s))
            {
                if (s.ToUpper() == "TRUE")
                {
                    this.ViewPublishClient.Visibility = System.Windows.Visibility.Visible;
                    this.ManageSurveyButton.Visibility = System.Windows.Visibility.Visible;
                    this.ViewDownloadClient.Visibility = System.Windows.Visibility.Visible;
                    this.ResponseClientbutton.Visibility = System.Windows.Visibility.Visible;
                    this.SurveyControls.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    this.ViewPublishClient.Visibility = System.Windows.Visibility.Hidden;
                    this.ManageSurveyButton.Visibility = System.Windows.Visibility.Hidden;
                    this.ViewDownloadClient.Visibility = System.Windows.Visibility.Hidden;
                    this.ResponseClientbutton.Visibility = System.Windows.Visibility.Hidden;
                    this.SurveyControls.Visibility = System.Windows.Visibility.Hidden;
                }
            }



        }

        private void ManageSurveyButton_Click(object sender, RoutedEventArgs e)
        {
            Page_ManageSurvey page_ManageSurvey = new Page_ManageSurvey();
            this.NavigationService.Navigate(page_ManageSurvey);
        }



        private void ViewPublishClien_Click(object sender, RoutedEventArgs e)
        {
            Page_Publish Page_Publish = new Page_Publish();
            this.NavigationService.Navigate(Page_Publish);
        }

        private void ViewDownloadClient_Click(object sender, RoutedEventArgs e)
        {
           DownLoad Page_Download = new DownLoad();
            this.NavigationService.Navigate(Page_Download);
        }

        private void AddOrganization_Click(object sender, RoutedEventArgs e)
        {
            MessagerichTextBox1.Document.Blocks.Clear();
            
            Epi.Web.Common.Message.OrganizationRequest Request = new Epi.Web.Common.Message.OrganizationRequest();
           
           
            MessagerichTextBox1.Foreground = Brushes.Red;


            
            try
            {
                if (!string.IsNullOrEmpty(passwordBox1.Password.ToString()) && IsGuid(passwordBox1.Password.ToString()))
                {
                    if (!string.IsNullOrEmpty(OrganizationtextBox1.Text.ToString()))
                    {
                        if (!string.IsNullOrEmpty(GeneratedkeytextBox1.Text.ToString()))
                        {
                            Request.Organization.IsEnabled = true;
                            Request.AdminSecurityKey = new Guid(passwordBox1.Password);
                            Request.Organization.Organization = OrganizationtextBox1.Text;
                            //Request.Organization.OrganizationKey = Cryptography.Encrypt(this.GeneratedkeytextBox1.Text);
                            Request.Organization.OrganizationKey =  this.GeneratedkeytextBox1.Text.ToString();
                           // Epi.Web.Common.Message.OrganizationResponse Result = client.SetOrganization(Request);

                            int ServiceVersion = ServiceClient.GetServiceVersion();

                            if (ServiceVersion == 1)
                                {
                                SurveyManagerService.ManagerServiceClient Client = ServiceClient.GetClient();

                                Epi.Web.Common.Message.OrganizationResponse Result = Client.SetOrganization(Request);
                                MessagerichTextBox1.Document.Blocks.Clear();
                                OrganizationtextBox1.Clear();
                                GeneratedkeytextBox1.Clear();
                                if (Result.Message.ToString().Contains("Successfully"))
                                    {
                                    MessagerichTextBox1.Foreground = Brushes.Green;
                                    }
                                MessagerichTextBox1.AppendText(Result.Message.ToString());
                                }
                            else if (ServiceVersion == 2)
                                {
                                SurveyManagerServiceV2.ManagerServiceV2Client Client = ServiceClient.GetClientV2();

                                Epi.Web.Common.Message.OrganizationResponse Result = Client.SetOrganization(Request);
                                MessagerichTextBox1.Document.Blocks.Clear();
                                OrganizationtextBox1.Clear();
                                GeneratedkeytextBox1.Clear();
                                if (Result.Message.ToString().Contains("Successfully"))
                                    {
                                    MessagerichTextBox1.Foreground = Brushes.Green;
                                    }
                                MessagerichTextBox1.AppendText(Result.Message.ToString());
                                }
                             else if (ServiceVersion == 3)
                                {
                                SurveyManagerServiceV3.ManagerServiceV3Client Client = ServiceClient.GetClientV3();

                                Epi.Web.Common.Message.OrganizationResponse Result = Client.SetOrganization(Request);
                                MessagerichTextBox1.Document.Blocks.Clear();
                                OrganizationtextBox1.Clear();
                                GeneratedkeytextBox1.Clear();
                                if (Result.Message.ToString().Contains("Successfully"))
                                    {
                                    MessagerichTextBox1.Foreground = Brushes.Green;
                                    }
                                MessagerichTextBox1.AppendText(Result.Message.ToString());
                                }
                             
                        }
                        else
                        {

                            MessagerichTextBox1.AppendText("Please generate organization key.");

                        }
                    }
                    else
                    {

                        MessagerichTextBox1.AppendText("Organization Name is required.");

                    }
                }
                else
                {

                    MessagerichTextBox1.AppendText("Admin pass  is required and Should be a Guid.");

                }
            }
            catch (Exception ex)
            {

                MessagerichTextBox1.AppendText("Error occurred while trying to add new organization key. Please  try again. ");
            }

        }
        private void GenerateKey_Clik(object sender, RoutedEventArgs e)
        {

            PasswordGenerator PasswordGenerator = new PasswordGenerator();

            PasswordGenerator.Minimum = 8;
            PasswordGenerator.Maximum = 12;
            // this.GeneratedkeytextBox1.AppendText(PasswordGenerator.Generate().ToString());
            Guid OrganizationID = Guid.NewGuid();
            this.GeneratedkeytextBox1.Clear();
            this.GeneratedkeytextBox1.AppendText(OrganizationID.ToString());
        }
        private void GetKey_Clik(object sender, RoutedEventArgs e)
        {

            //SurveyManagerService.ManagerServiceClient client = ServiceClient.GetClient();
            //Epi.Web.Common.Message.OrganizationRequest Request = new Epi.Web.Common.Message.OrganizationRequest();

            this.ONameEditTextBox1.Clear();
            this.checkBox1.IsChecked = false;
            richTextBox1.Document.Blocks.Clear();
            try
            {

                if (!string.IsNullOrEmpty(passwordBox1.Password.ToString()) && IsGuid(passwordBox1.Password.ToString()))
                {
                        if (OnamelistBox1.Items.Count > 0)
                          {
                                if (!string.IsNullOrEmpty(OnamelistBox1.SelectedItem.ToString()))
                                    {
                                int ServiceVersion = ServiceClient.GetServiceVersion();

                                         if (ServiceVersion == 1)
                                    {
                                            SurveyManagerService.ManagerServiceClient client = ServiceClient.GetClient();
                                            Epi.Web.Common.Message.OrganizationRequest Request = new Epi.Web.Common.Message.OrganizationRequest();
                                            Request.Organization.Organization = this.OnamelistBox1.SelectedItem.ToString();
                                            Request.AdminSecurityKey = new Guid(passwordBox1.Password);
                                            Epi.Web.Common.Message.OrganizationResponse Result = client.GetOrganization(Request);
                                            EditOtextBox1.Clear();
                                            ONameEditTextBox1.Clear();
                                            if (Result.Message != null)
                                            {
                                                richTextBox1.AppendText(Result.Message.ToString());
                                            }

                                            if (Result.OrganizationList != null)
                                            {

                                                for (int i = 0; i < Result.OrganizationList.Count; i++)
                                                {

                                                   // EditOtextBox1.Text = Cryptography.Decrypt(Result.OrganizationList[i].OrganizationKey.ToString());
                                                    EditOtextBox1.Text = Result.OrganizationList[i].OrganizationKey.ToString() ;
                                                    this.ONameEditTextBox1.Text = Result.OrganizationList[i].Organization;
                                                    this.checkBox1.IsChecked = Result.OrganizationList[i].IsEnabled;
                                                }
                                
                                            }
                                       }
                                    else if (ServiceVersion == 2 )
                                
                                        {
                                        SurveyManagerServiceV2.ManagerServiceV2Client client = ServiceClient.GetClientV2();
                                        Epi.Web.Common.Message.OrganizationRequest Request = new Epi.Web.Common.Message.OrganizationRequest();
                                        Request.Organization.Organization = this.OnamelistBox1.SelectedItem.ToString();
                                        Request.AdminSecurityKey = new Guid(passwordBox1.Password);
                                        Epi.Web.Common.Message.OrganizationResponse Result = client.GetOrganization(Request);
                                        EditOtextBox1.Clear();
                                        ONameEditTextBox1.Clear();
                                        if (Result.Message != null)
                                            {
                                            richTextBox1.AppendText(Result.Message.ToString());
                                            }

                                        if (Result.OrganizationList != null)
                                            {

                                            for (int i = 0; i < Result.OrganizationList.Count; i++)
                                                {

                                                // EditOtextBox1.Text = Cryptography.Decrypt(Result.OrganizationList[i].OrganizationKey.ToString());
                                                EditOtextBox1.Text = Result.OrganizationList[i].OrganizationKey.ToString();
                                                this.ONameEditTextBox1.Text = Result.OrganizationList[i].Organization;
                                                this.checkBox1.IsChecked = Result.OrganizationList[i].IsEnabled;
                                                }

                                            }
                                
                                        }
                                    else if (ServiceVersion == 3 )
                                
                                        {
                                        SurveyManagerServiceV3.ManagerServiceV3Client client = ServiceClient.GetClientV3();
                                        Epi.Web.Common.Message.OrganizationRequest Request = new Epi.Web.Common.Message.OrganizationRequest();
                                        Request.Organization.Organization = this.OnamelistBox1.SelectedItem.ToString();
                                        Request.AdminSecurityKey = new Guid(passwordBox1.Password);
                                        Epi.Web.Common.Message.OrganizationResponse Result = client.GetOrganization(Request);
                                        EditOtextBox1.Clear();
                                        ONameEditTextBox1.Clear();
                                        if (Result.Message != null)
                                            {
                                            richTextBox1.AppendText(Result.Message.ToString());
                                            }

                                        if (Result.OrganizationList != null)
                                            {

                                            for (int i = 0; i < Result.OrganizationList.Count; i++)
                                                {

                                                // EditOtextBox1.Text = Cryptography.Decrypt(Result.OrganizationList[i].OrganizationKey.ToString());
                                                EditOtextBox1.Text = Result.OrganizationList[i].OrganizationKey.ToString();
                                                this.ONameEditTextBox1.Text = Result.OrganizationList[i].Organization;
                                                this.checkBox1.IsChecked = Result.OrganizationList[i].IsEnabled;
                                                }

                                            }
                                
                                        }// if 3
                                     }
                            }
                    
                        else
                            {

                                richTextBox1.AppendText("Please selet a organization name.");

                            }
                    }
                else
                    {
                        richTextBox1.AppendText("Please selet a organization name. And admin pass is required and Should be a Guid. ");
                    
                    }
               // }
              //  else
              //  {

               //     richTextBox1.AppendText("Admin pass  is required and Should be a Guid.");

               // }
            }
            catch (Exception ex)
            {

                richTextBox1.AppendText("Error occurred while trying to get new organization keys.  ");
            }


        }
   
        private void GetOrganizationNames_Clik(object sender, RoutedEventArgs e)
        {

            GetOrganizationNames();
            

        }
        public static bool IsGuid(string expression)
        {
            if (expression != null)
            {
                Regex guidRegEx = new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");

                return guidRegEx.IsMatch(expression);
            }
            return false;
        }

        //Copy_Clik
        private void Copy_Clik(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(EditOtextBox1.Text ))
            {
                Clipboard.SetText(EditOtextBox1.Text);
            }
        }
        private void Edit_Clik(object sender, RoutedEventArgs e)
        {
            //if (!string.IsNullOrEmpty(EditOtextBox1.Text))
            //{
                
                richTextBox1.Document.Blocks.Clear();
                try
                {

                    if (!string.IsNullOrEmpty(passwordBox1.Password.ToString()) && IsGuid(passwordBox1.Password.ToString()))
                    {


                    int ServiceVersion = ServiceClient.GetServiceVersion();

                    if (ServiceVersion == 1)
                        {
                            SurveyManagerService.ManagerServiceClient client = ServiceClient.GetClient();
                            Epi.Web.Common.Message.OrganizationRequest Request = new Epi.Web.Common.Message.OrganizationRequest();

                            Request.AdminSecurityKey = new Guid(passwordBox1.Password);
                            Request.Organization.OrganizationKey = EditOtextBox1.Text;
                            Epi.Web.Common.Message.OrganizationResponse Result = client.GetOrganizationByKey(Request);
                            if (Result.OrganizationList != null)
                                {

                                for (int i = 0; i < Result.OrganizationList.Count; i++)
                                    {

                                    this.ONameEditTextBox1.Text = Result.OrganizationList[i].Organization;
                                    this.checkBox1.IsChecked = Result.OrganizationList[i].IsEnabled;
                                    }

                                }
                            }

                        else if (ServiceVersion == 2)
                            {
                            SurveyManagerServiceV2.ManagerServiceV2Client client = ServiceClient.GetClientV2();
                            Epi.Web.Common.Message.OrganizationRequest Request = new Epi.Web.Common.Message.OrganizationRequest();

                            Request.AdminSecurityKey = new Guid(passwordBox1.Password);
                            Request.Organization.OrganizationKey = EditOtextBox1.Text;
                            Epi.Web.Common.Message.OrganizationResponse Result = client.GetOrganizationByKey(Request);
                            if (Result.OrganizationList != null)
                                {

                                for (int i = 0; i < Result.OrganizationList.Count; i++)
                                    {

                                    this.ONameEditTextBox1.Text = Result.OrganizationList[i].Organization;
                                    this.checkBox1.IsChecked = Result.OrganizationList[i].IsEnabled;
                                    }

                                }
                            }

                      
                    }
                    else
                    {

                        richTextBox1.AppendText("Admin pass  is required and Should be a Guid.");

                    }
                }
                catch (Exception ex)
                {

                    richTextBox1.AppendText("Error occurred while trying to get organization Info. ");
                }
            //}
            //else {
            //    richTextBox1.AppendText("Please select organization key.");
            
            //}
        }

        //Save_Click
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            MessagerichTextBox1.Document.Blocks.Clear();
            //SurveyManagerService.ManagerServiceClient client = ServiceClient.GetClient();
            Epi.Web.Common.Message.OrganizationRequest Request = new Epi.Web.Common.Message.OrganizationRequest();
            richTextBox1.Foreground = Brushes.Red;
            richTextBox1.Document.Blocks.Clear();


            try
            {
                if (!string.IsNullOrEmpty(passwordBox1.Password.ToString()) && IsGuid(passwordBox1.Password.ToString()))
                {


                int ServiceVersion = ServiceClient.GetServiceVersion();

                if (ServiceVersion == 1)
                    {
                                SurveyManagerService.ManagerServiceClient client = ServiceClient.GetClient();
                                if (checkBox1.IsChecked == true)
                                    {
                                    Request.Organization.IsEnabled = true;
                                    }
                                else
                                    {
                                    Request.Organization.IsEnabled = false;
                                    }
                                Request.AdminSecurityKey = new Guid(passwordBox1.Password);
                                Request.Organization.Organization = ONameEditTextBox1.Text;
                                //Request.Organization.OrganizationKey = Cryptography.Encrypt(EditOtextBox1.Text);
                                Request.Organization.OrganizationKey = EditOtextBox1.Text.ToString();
                                Epi.Web.Common.Message.OrganizationResponse Result = client.UpdateOrganizationInfo(Request);


                                if (Result.Message.ToString().Contains("Successfully"))
                                    {
                                    richTextBox1.Foreground = Brushes.Green;
                                    GetOrganizationNames();
                                    this.OnamelistBox1.SelectedItem = Request.Organization.Organization;
                                    }
                                richTextBox1.AppendText(Result.Message.ToString());

                                }

                            else if (ServiceVersion == 2)
                                {
                                SurveyManagerServiceV2.ManagerServiceV2Client client = ServiceClient.GetClientV2();
                                if (checkBox1.IsChecked == true)
                                    {
                                    Request.Organization.IsEnabled = true;
                                    }
                                else
                                    {
                                    Request.Organization.IsEnabled = false;
                                    }
                                Request.AdminSecurityKey = new Guid(passwordBox1.Password);
                                Request.Organization.Organization = ONameEditTextBox1.Text;
                                //Request.Organization.OrganizationKey = Cryptography.Encrypt(EditOtextBox1.Text);
                                Request.Organization.OrganizationKey = EditOtextBox1.Text.ToString();
                                Epi.Web.Common.Message.OrganizationResponse Result = client.UpdateOrganizationInfo(Request);


                                if (Result.Message.ToString().Contains("Successfully"))
                                    {
                                    richTextBox1.Foreground = Brushes.Green;
                                    GetOrganizationNames();
                                    this.OnamelistBox1.SelectedItem = Request.Organization.Organization;
                                    }
                                richTextBox1.AppendText(Result.Message.ToString());

                                }



                        
                }
                else
                {

                    richTextBox1.AppendText("Admin pass  is required and Should be a Guid.");

                }
            }
            catch (Exception ex)
            {

                richTextBox1.AppendText("Error occurred while updating organization info. Please  try again. ");
            }
        }

        public void GetOrganizationNames()
        {

           
            Epi.Web.Common.Message.OrganizationRequest Request = new Epi.Web.Common.Message.OrganizationRequest();

            richTextBox1.Document.Blocks.Clear();

            try
            {

                if (!string.IsNullOrEmpty(passwordBox1.Password.ToString()) && IsGuid(passwordBox1.Password.ToString()))
                {


                int ServiceVersion = ServiceClient.GetServiceVersion();

                if (ServiceVersion == 1)
                    {
                        Request.AdminSecurityKey = new Guid(passwordBox1.Password);
                        SurveyManagerService.ManagerServiceClient client = ServiceClient.GetClient();
                        Epi.Web.Common.Message.OrganizationResponse Result = client.GetOrganizationNames(Request);


                        OnamelistBox1.Items.Clear();
                        if (Result.Message != null)
                            {
                            richTextBox1.AppendText(Result.Message.ToString());
                            }
                        if (Result.OrganizationList != null)
                            {

                            for (int i = 0; i < Result.OrganizationList.Count; i++)
                                {

                                this.OnamelistBox1.Items.Add(Result.OrganizationList[i].Organization);

                                }
                            this.OnamelistBox1.SelectedIndex = 0;
                            }

                        }

                    else if (ServiceVersion == 2)
                        {
                        Request.AdminSecurityKey = new Guid(passwordBox1.Password);
                        SurveyManagerServiceV2.ManagerServiceV2Client client = ServiceClient.GetClientV2();
                        Epi.Web.Common.Message.OrganizationResponse Result = client.GetOrganizationNames(Request);


                        OnamelistBox1.Items.Clear();
                        if (Result.Message != null)
                            {
                            richTextBox1.AppendText(Result.Message.ToString());
                            }
                        if (Result.OrganizationList != null)
                            {

                            for (int i = 0; i < Result.OrganizationList.Count; i++)
                                {

                                this.OnamelistBox1.Items.Add(Result.OrganizationList[i].Organization);

                                }
                            this.OnamelistBox1.SelectedIndex = 0;
                            }

                        }



                }
                else
                {

                    richTextBox1.AppendText("Admin pass  is required and Should be a Guid.");

                }
            }
            catch (Exception ex)
            {

                richTextBox1.AppendText("Error occurred while trying to get all organization names. ");
            }

            
        
        }

        private void ViewConfigButton_Click(object sender, RoutedEventArgs e)
        {
            Page_Configuration Page_Config = new Page_Configuration();
            this.NavigationService.Navigate(Page_Config);
        }

        private void ResponseClient_Click(object sender, RoutedEventArgs e)
            {
            ResponseClient ResponseClient = new ResponseClient();
            this.NavigationService.Navigate(ResponseClient);
            }

        private void SurveyControls_Click(object sender, RoutedEventArgs e)
            {
            Page_GetSurveyControls SurveyControls = new Page_GetSurveyControls();
            this.NavigationService.Navigate(SurveyControls);
            }
        
    }
}
