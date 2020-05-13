using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Epi.Web.Common.Security;
using Epi.Web.Common.Message;
namespace Epi.Web.SurveyManager.Client
{
    /// <summary>
    /// Interaction logic for Page_AddUser.xaml
    /// </summary>
    public partial class Page_AddUser : Page
    {
        Guid _OrganizationKey; Guid _AdminKey; string _OrganizationName; string _UserName;

        //public Page_AddUser(Guid OrganizationKey, Guid AdminKey , string Organization )
        //{
        //    _OrganizationKey = OrganizationKey;
        //    _AdminKey = AdminKey;
        //     _OrganizationName = Organization;





        //    InitializeComponent();

        //    Cancel1.Visibility = Visibility.Hidden;

        //   // TextBlock_OrganizationName = new TextBlock();
        //    TextBlock_OrganizationName.Text = Organization;

        //}
        public Page_AddUser(Guid OrganizationKey, Guid AdminKey, string Organization, string UserName, bool ShowCancel)
        {
            _OrganizationKey = OrganizationKey;
            _AdminKey = AdminKey;
            _OrganizationName = Organization;
            _UserName = UserName;
             InitializeComponent();
            if (!ShowCancel )
            {
                Cancel1.Visibility = Visibility.Hidden;
               
            }
            else {

                Cancel1.Visibility = Visibility.Visible;
                

            }
            if (string.IsNullOrEmpty(UserName)) {
                this.AddEditUser.Content = "Add User";
            }
            else {
                this.AddEditUser.Content = "Update User";
                // Add select user info to the screen 

                Epi.Web.Common.Message.UserRequest Request = new Epi.Web.Common.Message.UserRequest();
                SurveyManagerServiceV4.ManagerServiceV4Client Client = ServiceClient.GetClientV4();

                var UserId = UserName.Substring(UserName.IndexOf("___")+3);
                var OrgId = Organization.Substring(Organization.IndexOf("___") + 3);
                Request.User.UserId = int.Parse(UserId);
                Request.Organization.OrganizationId = int.Parse(OrgId);
                var Result = Client.GetUserByUserId(Request);
                FName.Text = Result.User[0].FirstName;
                 LName.Text = Result.User[0].LastName;
                Email.Text = Result.User[0].EmailAddress;
                this.PhoneNum.Text = Result.User[0].PhoneNumber;
                if (Result.User[0].IsActive) {
                    IsActive.IsChecked = true;
                }
                else {

                    IsActive.IsChecked = false;
                }
                
            }
            TextBlock_OrganizationName.Text = Organization;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          
            Epi.Web.Common.Message.UserRequest Request = new Epi.Web.Common.Message.UserRequest();
            SurveyManagerServiceV4.ManagerServiceV4Client Client = ServiceClient.GetClientV4();
            Request.User.FirstName = FName.Text.ToString();
            Request.User.LastName = LName.Text.ToString();
            Request.User.EmailAddress = Email.Text.ToString();
            Request.User.PhoneNumber = this.PhoneNum.Text.ToString();
            Request.User.UserName = Email.Text.ToString().Split('@')[0];
            
            Request.User.UGuid = Guid.NewGuid();
            Request.User.PasswordHash = "NA";
            Request.User.ResetPassword = false;
            if (IsActive.IsChecked != null)
            {
                Request.User.IsActive = IsActive.IsChecked.Value;
            }
            else {
                Request.User.IsActive = false;

            }
            Request.Organization.OrganizationKey = _OrganizationKey.ToString();
          
            bool Result = false;
            if (string.IsNullOrEmpty(_UserName)) {
                  Result = Client.SetUserInfo(Request);
            }
            else {
                var UserId = _UserName.Substring(_UserName.IndexOf("___") + 3);
                var OrgId = _OrganizationName.Substring(_OrganizationName.IndexOf("___") + 3);
                Request.User.Operation = Common.Constants.Constant.OperationMode.UpdateUserInfo;
                Request.User.UserId =int.Parse( UserId);
                Request.Organization.OrganizationId = int.Parse(OrgId);
                Result = Client.UpdateUserInfo(Request);
            }
            Message.Text="";

            if (Result)
            {
                //Message.Foreground = Brushes.Green;
                //Message.Text = "Successfully added a user";

                Page_AddOrganization Page_AddUser = new Page_AddOrganization(_AdminKey.ToString(), _OrganizationName.ToString());
                this.NavigationService.Navigate(Page_AddUser);

            }
            else {
                Message.Foreground = Brushes.Red;
                Message.Text = "Error occurred while trying to add a user";
            }
           // Message.Text= (Result.Message.ToString());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Page_AddOrganization Page_AddUser = new Page_AddOrganization(_AdminKey.ToString(), _OrganizationName.ToString());
            this.NavigationService.Navigate(Page_AddUser);
        }
    }
}
