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
            Page_Download Page_Download = new Page_Download();
            this.NavigationService.Navigate(Page_Download);
        }

        private void AddOrganization_Click(object sender, RoutedEventArgs e)
        {
            MessagerichTextBox1.Document.Blocks.Clear();
            SurveyManagerService.ManagerServiceClient client = new SurveyManagerService.ManagerServiceClient();
            Epi.Web.Common.Message.OrganizationRequest Request = new Epi.Web.Common.Message.OrganizationRequest();



            
            try
            {
                if (!string.IsNullOrEmpty(passwordBox1.Password.ToString()) && IsGuid(passwordBox1.Password.ToString()))
                {
                    if (!string.IsNullOrEmpty(OrganizationtextBox1.Text.ToString()))
                    {
                        if (!string.IsNullOrEmpty(GeneratedkeytextBox1.Text.ToString()))
                        {
                            Request.Organization.IsEnabled = true;
                            Request.Organization.AdminId = new Guid(passwordBox1.Password);
                            Request.Organization.Organization = OrganizationtextBox1.Text;
                            Request.Organization.OrganizationKey = Cryptography.Encrypt(this.GeneratedkeytextBox1.Text);
                            Epi.Web.Common.Message.OrganizationResponse Result = client.SetOrganization(Request);
                            MessagerichTextBox1.Document.Blocks.Clear();
                            OrganizationtextBox1.Clear();
                            GeneratedkeytextBox1.Clear();
                            MessagerichTextBox1.AppendText(Result.Message.ToString());
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

            SurveyManagerService.ManagerServiceClient client = new SurveyManagerService.ManagerServiceClient();
            Epi.Web.Common.Message.OrganizationRequest Request = new Epi.Web.Common.Message.OrganizationRequest();


            richTextBox1.Document.Blocks.Clear();
            try
            {

                if (!string.IsNullOrEmpty(passwordBox1.Password.ToString()) && IsGuid(passwordBox1.Password.ToString()))
                {
                    if (OnamelistBox1.Items.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(OnamelistBox1.SelectedItem.ToString()))
                        {
                            Request.Organization.Organization = this.OnamelistBox1.SelectedItem.ToString();
                            Request.Organization.AdminId = new Guid(passwordBox1.Password);
                            Epi.Web.Common.Message.OrganizationResponse Result = client.GetOrganization(Request);
                            OKeyslistBox1.Items.Clear();
                            if (Result.Message != null)
                            {
                                richTextBox1.AppendText(Result.Message.ToString());
                            }

                            if (Result.OrganizationList != null)
                            {

                                for (int i = 0; i < Result.OrganizationList.Count; i++)
                                {

                                    OKeyslistBox1.Items.Add(Cryptography.Decrypt(Result.OrganizationList[i].OrganizationKey.ToString()));

                                }
                                
                            }
                           
                        }
                        else
                        {

                            richTextBox1.AppendText("Please selet a organization name.");

                        }
                    }else{
                        richTextBox1.AppendText("Please selet a organization name.");
                    
                    }
                }
                else
                {

                    richTextBox1.AppendText("Admin pass  is required and Should be a Guid.");

                }
            }
            catch (Exception ex)
            {

                richTextBox1.AppendText("Error occurred while trying to get new organization keys.  ");
            }


        }
        private void GetOrganizationNames_Clik(object sender, RoutedEventArgs e)
        {
            
            SurveyManagerService.ManagerServiceClient client = new SurveyManagerService.ManagerServiceClient();
            Epi.Web.Common.Message.OrganizationRequest Request = new Epi.Web.Common.Message.OrganizationRequest();

            richTextBox1.Document.Blocks.Clear();
            try
            {

                if (!string.IsNullOrEmpty(passwordBox1.Password.ToString()) && IsGuid(passwordBox1.Password.ToString()))
                {

                    Request.Organization.AdminId = new Guid(passwordBox1.Password);
                    Epi.Web.Common.Message.OrganizationResponse Result = client.GetOrganizationInfo(Request);

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
            Clipboard.SetText(this.OKeyslistBox1.SelectedItem.ToString());
        }
    }
}
