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
    public partial class Page_AddOrganization : Page
    {
        private static string _ConfigurationAdminCode;
        private string _AdminKey;
        public Page_AddOrganization(string AdminKey)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(AdminKey))
            {

                _AdminKey = AdminKey;
                passwordBox1.Password = _AdminKey;
                GetOrganizationNames();
            }
            

            this.WindowTitle = "Add Organization";

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
        public Page_AddOrganization()
        {
           
            InitializeComponent();
            
            this.WindowTitle = "Add Organization";

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
            richTextBox1.Document.Blocks.Clear();

            Epi.Web.Common.Message.OrganizationRequest Request = new Epi.Web.Common.Message.OrganizationRequest();


            richTextBox1.Foreground = Brushes.Red;



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
                            Request.Organization.OrganizationKey = this.GeneratedkeytextBox1.Text.ToString();
                            // Epi.Web.Common.Message.OrganizationResponse Result = client.SetOrganization(Request);

                            int ServiceVersion = ServiceClient.GetServiceVersion();

                            if (ServiceVersion == 1)
                            {
                                Page_AddUser Page_AddUser = new Page_AddUser(new Guid(this.GeneratedkeytextBox1.Text.ToString()), new Guid(passwordBox1.Password), OrganizationtextBox1.Text,"",false);
                                this.NavigationService.Navigate(Page_AddUser);

                                SurveyManagerService.ManagerServiceClient Client = ServiceClient.GetClient();

                                Epi.Web.Common.Message.OrganizationResponse Result = Client.SetOrganization(Request);
                                richTextBox1.Document.Blocks.Clear();
                                OrganizationtextBox1.Clear();
                                GeneratedkeytextBox1.Clear();
                                if (Result.Message.ToString().Contains("Successfully"))
                                {
                                    richTextBox1.Foreground = Brushes.Green;
                                }
                                richTextBox1.AppendText(Result.Message.ToString());
                            }
                            else if (ServiceVersion == 2)
                            {
                                SurveyManagerServiceV2.ManagerServiceV2Client Client = ServiceClient.GetClientV2();

                                Epi.Web.Common.Message.OrganizationResponse Result = Client.SetOrganization(Request);
                                richTextBox1.Document.Blocks.Clear();
                                OrganizationtextBox1.Clear();
                                GeneratedkeytextBox1.Clear();
                                if (Result.Message.ToString().Contains("Successfully"))
                                {
                                    richTextBox1.Foreground = Brushes.Green;
                                }
                                richTextBox1.AppendText(Result.Message.ToString());
                            }
                            else if (ServiceVersion == 3)
                            {


                                SurveyManagerServiceV3.ManagerServiceV3Client Client = ServiceClient.GetClientV3();

                                Epi.Web.Common.Message.OrganizationResponse Result = Client.SetOrganization(Request);
                                richTextBox1.Document.Blocks.Clear();
                                OrganizationtextBox1.Clear();
                                GeneratedkeytextBox1.Clear();
                                if (Result.Message.ToString().Contains("Successfully"))
                                {
                                    richTextBox1.Foreground = Brushes.Green;
                                }
                                richTextBox1.AppendText(Result.Message.ToString());
                            }
                            else if (ServiceVersion == 5)
                            {


                                SurveyManagerServiceV4.ManagerServiceV4Client Client = ServiceClient.GetClientV4();

                                Epi.Web.Common.Message.OrganizationResponse Result = Client.SetOrganization(Request);
                                richTextBox1.Document.Blocks.Clear();
                                OrganizationtextBox1.Clear();
                                GeneratedkeytextBox1.Clear();
                                if (Result.Message.ToString().Contains("Successfully"))
                                {
                                    richTextBox1.Foreground = Brushes.Green;
                                }
                                richTextBox1.AppendText(Result.Message.ToString());
                            }
                        }
                        else
                        {

                            richTextBox1.AppendText("Please generate organization key.");

                        }
                    }
                    else
                    {

                        richTextBox1.AppendText("Organization Name is required.");

                    }
                }
                else
                {

                    richTextBox1.AppendText("Admin key  is required and Should be a Guid.");

                }
            }
            catch (Exception ex)
            {

                richTextBox1.AppendText("Error occurred while trying to add new organization key. Please  try again. ");
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

                            
                                SurveyManagerServiceV4.ManagerServiceV4Client client = ServiceClient.GetClientV4();
                                Epi.Web.Common.Message.OrganizationRequest Request = new Epi.Web.Common.Message.OrganizationRequest();
                                Request.Organization.Organization = this.OnamelistBox1.SelectedItem.ToString().Remove( OnamelistBox1.SelectedItem.ToString().IndexOf("___"));
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
                    }

                    else
                    {

                        richTextBox1.AppendText("Please selet a organization name.");

                    }
                }
                else
                {
                    richTextBox1.AppendText("Please selet a organization name. And admin key is required and Should be a Guid. ");

                }
                // }
                //  else
                //  {

                //     richTextBox1.AppendText("Admin pass  is required and Should be a Guid.");

                // }

                 GetOrgUsers();
            }
            catch (Exception ex)
            {

                richTextBox1.AppendText("Error occurred while trying to get new organization keys.  ");
            }


        }

        private void GetOrganizationNames_Clik(object sender, RoutedEventArgs e)
        {

            //  GetOrgUsers();


        }
        void PasswordChangedHandler(Object sender, RoutedEventArgs args)
        {
            var value = this.passwordBox1.Password;
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
            if (!string.IsNullOrEmpty(EditOtextBox1.Text))
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

                    richTextBox1.AppendText("Admin key  is required and Should be a Guid.");

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
            richTextBox1.Document.Blocks.Clear();
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

                    richTextBox1.AppendText("Admin key  is required and Should be a Guid.");

                }
            }
            catch (Exception ex)
            {

                richTextBox1.AppendText("Error occurred while updating organization info. Please  try again. ");
            }
        }


        public void GetOrgUsers()
        {
            Epi.Web.Common.Message.UserRequest Request = new Epi.Web.Common.Message.UserRequest();
            try
            {

                SurveyManagerServiceV4.ManagerServiceV4Client client = ServiceClient.GetClientV4();
                Request.Organization.OrganizationKey = this.EditOtextBox1.Text.ToString();
                var Result = client.GetUserListByOrganization(Request);

                
                UserlistBox.Items.Clear();
                if (Result.Message != null)
                {
                    richTextBox1.AppendText(Result.Message.ToString());
                }
                if (Result.User != null)
                {

                    for (int i = 0; i < Result.User.Count; i++)
                    {

                        this.UserlistBox.Items.Add(Result.User[i].FirstName + " " + Result.User[i].LastName + "___" + Result.User[i].UserId);

                    }
                    this.UserlistBox.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText("Error occurred while getting Organization users. ");
            }
        }

        public void GetOrganizationNames()
        {
            richTextBox1.AppendText("Simo is here");

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

                                this.OnamelistBox1.Items.Add(Result.OrganizationList[i].Organization +"___"+ Result.OrganizationList[i].OrganizationId);

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
                    else if (ServiceVersion == 3)
                    {
                        Request.AdminSecurityKey = new Guid(passwordBox1.Password);
                        SurveyManagerServiceV3.ManagerServiceV3Client client = ServiceClient.GetClientV3();
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
                    else if (ServiceVersion == 4)
                    {

                        Request.AdminSecurityKey = new Guid(passwordBox1.Password);
                        SurveyManagerServiceV4.ManagerServiceV4Client client = ServiceClient.GetClientV4();
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
                        if (this.OnamelistBox1.SelectedItem != null)
                        {
                            GetOrgUsers();
                        }
                    }
                    else
                    {

                        richTextBox1.AppendText("Error occurred while trying to get all organization names.");
                    }



                }
                else
                {

                    richTextBox1.AppendText("Admin key  is required and Should be a Guid.");

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

        private void RichTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var OrgKeyIsGUid = IsGuid(this.EditOtextBox1.Text.ToString());
            var AdminKeyIsGUid = IsGuid(passwordBox1.Password);
            if (OrgKeyIsGUid && AdminKeyIsGUid)
            {
                Page_AddUser Page_AddUser = new Page_AddUser(new Guid(this.EditOtextBox1.Text.ToString()), new Guid(passwordBox1.Password), OnamelistBox1.SelectedItem.ToString(),"",true);
                this.NavigationService.Navigate(Page_AddUser);


            }
            else
            {
                richTextBox1.Foreground = Brushes.Red;
                richTextBox1.AppendText("Admin key  is required and Should be a Guid.");

            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var OrgKeyIsGUid = IsGuid(this.EditOtextBox1.Text.ToString());
            var AdminKeyIsGUid = IsGuid(passwordBox1.Password);
            if (OrgKeyIsGUid && AdminKeyIsGUid)
            {
                if (UserlistBox.SelectedItem != null) {
                    Page_AddUser Page_AddUser = new Page_AddUser(new Guid(this.EditOtextBox1.Text.ToString()), new Guid(passwordBox1.Password), OnamelistBox1.SelectedItem.ToString(), UserlistBox.SelectedItem.ToString(), true);
                    this.NavigationService.Navigate(Page_AddUser);

                }
                else {

                    richTextBox1.Foreground = Brushes.Red;
                    richTextBox1.AppendText("Please select a user.");
                }
            }
            else
            {
                richTextBox1.Foreground = Brushes.Red;
                richTextBox1.AppendText("Admin key  is required and Should be a Guid.");

            }
        }
    }
}