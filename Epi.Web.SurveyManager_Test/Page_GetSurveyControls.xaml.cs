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
    /// Interaction logic for Page_GetSurveyControls.xaml
    /// </summary>
    public partial class Page_GetSurveyControls : Page
        {
        public Page_GetSurveyControls()
            {
            InitializeComponent();
            }


        private void ManageSurveyButton_Click(object sender, RoutedEventArgs e)
            {
            Page_ManageSurvey page_ManageSurvey = new Page_ManageSurvey();
            this.NavigationService.Navigate(page_ManageSurvey);
            }

        private void AddOrganization_Click(object sender, RoutedEventArgs e)
            {
            Page_AddOrganization Page_AddOrganization = new Page_AddOrganization();
            this.NavigationService.Navigate(Page_AddOrganization);
            }

        private void button1_Click(object sender, RoutedEventArgs e)
            {
            DownLoad page_Download = new DownLoad();
            this.NavigationService.Navigate(page_Download);
            }

        private void button3_Click(object sender, RoutedEventArgs e)
            {
            Epi.Web.Common.Message.SurveyControlsResponse Result = new Common.Message.SurveyControlsResponse();
            try
                {
                SurveyManagerServiceV2.ManagerServiceV2Client client = ServiceClient.GetClientV2();
                Epi.Web.Common.Message.SurveyControlsRequest Request = new Common.Message.SurveyControlsRequest();
                Request.SurveyId = this.SurveyId.Text;
                  Result = client.GetSurveyControlList(Request);
                foreach (Epi.Web.Common.DTO.SurveyControlDTO ControlInfo in Result.SurveyControlList)
                    {
                    ControlsInfo.AppendText(string.Format("{0} - {1} - {2}\n", ControlInfo.ControlId, ControlInfo.ControlType, ControlInfo.ControlPrompt));
                    ControlsInfo.AppendText("\n-----------------------------------------------------\n");
                    }
                }
            catch (Exception ex)
                {
                ControlsInfo.AppendText("Error");
                //throw ex;



               
                }
            }

        }
    }
