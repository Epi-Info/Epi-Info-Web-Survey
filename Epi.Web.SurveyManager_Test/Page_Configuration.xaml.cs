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

namespace Epi.Web.SurveyManager.Client
{
    /// <summary>
    /// Interaction logic for Page_EnDe.xaml
    /// </summary>
    public partial class Page_Configuration : Page
    {
         
        public Page_Configuration()
        {
            InitializeComponent();


           string s = ConfigurationManager.AppSettings["EndPointAddress"];
      
            if (!String.IsNullOrEmpty(s))
            {
                EndPointURLTextBox.Text = s;
            }

             

            s = ConfigurationManager.AppSettings["Authentication_Use_Windows"];
            if (!String.IsNullOrEmpty(s))
            {
                if (s.ToUpper() == "TRUE")
                {
                    this.YesRadioButton.IsChecked = true;
                }
                else
                {
                    this.NoRadioButton.IsChecked = true;
                }
            }


            s = ConfigurationManager.AppSettings["WCF_BINDING_TYPE"];
            if (!String.IsNullOrEmpty(s))
            {
                if (s.ToUpper() == "WSHTTP")
                {
                    this.wsHTTPRadioButton.IsChecked = true;
                }
                else
                {
                    this.BasicBindingRadioButton.IsChecked = true;
                }
            }


            s = ConfigurationManager.AppSettings["SHOW_TESTING_FEATURES"];
            if (!String.IsNullOrEmpty(s))
            {
                if (s.ToUpper() == "TRUE")
                {
                    this.ShowTestFeatruesCheckBox.IsChecked = true;
                }
                else
                {
                    this.ShowTestFeatruesCheckBox.IsChecked = false;
                }
            }
        }



        private void ShowTestFeatruesCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ConfigurationManager.AppSettings["SHOW_TESTING_FEATURES"] = "TRUE";
        }

        private void ShowTestFeatruesCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ConfigurationManager.AppSettings["SHOW_TESTING_FEATURES"] = "FALSE";
        }

        private void UpdateConfigButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you wish to update the configuration file?", "Confirm Update", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
            {
                //save to apply changes
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                if (!string.IsNullOrWhiteSpace(this.EndPointURLTextBox.Text))
                {
                    config.AppSettings.Settings["EndPointAddress"].Value = this.EndPointURLTextBox.Text;
                }

                if ((bool)this.YesRadioButton.IsChecked)
                {
                    config.AppSettings.Settings["Authentication_Use_Windows"].Value = "TRUE";
                }
                else
                {
                    config.AppSettings.Settings["Authentication_Use_Windows"].Value = "FALSE";
                }


                if ((bool)this.wsHTTPRadioButton.IsChecked)
                {
                    config.AppSettings.Settings["WCF_BINDING_TYPE"].Value = "WSHTTP";
                }
                else
                {
                    config.AppSettings.Settings["WCF_BINDING_TYPE"].Value = "BASIC";
                }



                if ((bool)this.ShowTestFeatruesCheckBox.IsChecked)
                {

                    config.AppSettings.Settings["SHOW_TESTING_FEATURES"].Value = "TRUE";


                }
                else
                {
                    config.AppSettings.Settings["SHOW_TESTING_FEATURES"].Value = "FALSE";
                }


                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }


        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            this.OutputConnectionTextBox.Text = Cryptography.Encrypt(this.InputConnectionTextBox.Text);
        }
        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            this.OutputConnectionTextBox.Text = Cryptography.Decrypt(this.InputConnectionTextBox.Text);
        }

        private void PingButton_Click(object sender, RoutedEventArgs e)
        {
            this.PingResultTextBox.Text = "";
            string pEndPointAddress = this.EndPointURLTextBox.Text;
            bool pIsAuthenticated = false;
            bool pIsWsHTTPBinding = true;

            if ((bool)this.YesRadioButton.IsChecked)
            {
                pIsAuthenticated = true;
            }

            if ((bool)this.wsHTTPRadioButton.IsChecked)
            {
                pIsWsHTTPBinding = true;
            }
            else
            {
                pIsWsHTTPBinding = false;
            }
            

            try
            {


            int ServiceVersion = ServiceClient.GetServiceVersion();

            if (ServiceVersion == 1)
                {
                    SurveyManagerService.ManagerServiceClient Client = ServiceClient.GetClient(pEndPointAddress, pIsAuthenticated, pIsWsHTTPBinding);
                    Epi.Web.Common.Message.OrganizationRequest Request = new Epi.Web.Common.Message.OrganizationRequest();
                     var Result  = Client.GetOrganization(Request);
                }
            else if (ServiceVersion == 2)
                {
                     SurveyManagerServiceV2.ManagerServiceV2Client Client = ServiceClient.GetClientV2(pEndPointAddress, pIsAuthenticated, pIsWsHTTPBinding);
                     Epi.Web.Common.Message.OrganizationRequest Request = new Epi.Web.Common.Message.OrganizationRequest();
                     var Result  = Client.GetOrganization(Request);
                 }
            else if (ServiceVersion == 3)
            {
                SurveyManagerServiceV3.ManagerServiceV3Client Client = ServiceClient.GetClientV3(pEndPointAddress, pIsAuthenticated, pIsWsHTTPBinding);
                Epi.Web.Common.Message.OrganizationRequest Request = new Epi.Web.Common.Message.OrganizationRequest();
                var Result = Client.GetOrganization(Request);
            }  
            this.PingResultTextBox.Text = "Successfully Created Service Client";
            
            this.PingResultTextBox.Text = "Successfully Created Service Client";
            }
            catch(Exception ex)
            {
                this.PingResultTextBox.Text = "Failed to create Service Client:\n\n" + ex.Message;
            }

        }

        private void YesRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            this.ProtocolGroupBox.IsEnabled = false;
            this.BasicBindingRadioButton.IsChecked = true;
        }

        private void NoRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            this.ProtocolGroupBox.IsEnabled = true;
        }
        
    }
}
