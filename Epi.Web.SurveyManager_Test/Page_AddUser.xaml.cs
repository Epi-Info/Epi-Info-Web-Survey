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
            

          //  _ConfigurationAdminCode =  ConfigurationManager.AppSettings["AdminKey"];
            SurveyManagerService.ManagerServiceClient client = new SurveyManagerService.ManagerServiceClient();
            Epi.Web.Common.Message.OrganizationRequest Request = new Epi.Web.Common.Message.OrganizationRequest();
 
            
            Request.Organization.IsEnabled = true;
            Request.Organization.Organization = OrganizationtextBox1.Text;
            Request.Organization.OrganizationKey = new Guid(this.GeneratedkeytextBox1.Text);
            Request.Organization.AdminId = new Guid(passwordBox1.Password);

          //Epi.Web.Common.Message.OrganizationResponse Result = client.GetOrganization(Request);
            Epi.Web.Common.Message.OrganizationResponse Result = client.SetOrganization(Request);
          
            
        }
        private void GenerateKey_Clik(object sender, RoutedEventArgs e) {

            PasswordGenerator PasswordGenerator = new PasswordGenerator();

            PasswordGenerator.Minimum = 8;
            PasswordGenerator.Maximum = 12;
           // this.GeneratedkeytextBox1.AppendText(PasswordGenerator.Generate().ToString());
            Guid OrganizationID = Guid.NewGuid();
            this.GeneratedkeytextBox1.Clear();
            this.GeneratedkeytextBox1.AppendText(OrganizationID.ToString());
        }
    }
}
