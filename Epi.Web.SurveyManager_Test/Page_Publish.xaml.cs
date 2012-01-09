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

namespace Epi.Web.SurveyManager.Client
{
    /// <summary>
    /// Interaction logic for Page_Publish.xaml
    /// </summary>
    public partial class Page_Publish : Page
    {
        public Page_Publish()
        {
            InitializeComponent();
            TimeSpan t = new TimeSpan(10, 0, 0, 0);
            this.datePicker1.SelectedDate = DateTime.Now + t;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            SurveyManagerService.ManagerServiceClient client = new SurveyManagerService.ManagerServiceClient();

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

            try
            {
                Epi.Web.Common.Message.PublishResponse Result = client.PublishSurvey(Request);

                ServiceResponseTextBox.AppendText("is published: ");
                ServiceResponseTextBox.AppendText(Result.PublishInfo.IsPulished.ToString());
                ServiceResponseTextBox.AppendText("\nURL: ");
                ServiceResponseTextBox.AppendText(Result.PublishInfo.URL);
                ServiceResponseTextBox.AppendText("\nStatus Text: ");
                ServiceResponseTextBox.AppendText(Result.PublishInfo.StatusText);
            }
            catch (Exception ex)
            {
                ServiceResponseTextBox.AppendText("error:\n");
                ServiceResponseTextBox.AppendText(ex.ToString());
            }
        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            Page_Download page_Download = new Page_Download();
            this.NavigationService.Navigate(page_Download);
        }
    }
}
