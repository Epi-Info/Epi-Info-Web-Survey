using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Epi.Web.MVC.Models
    {
    public class FormModel
        {
        private List<FormInfoModel> _FormList;
        private List<OrganizationModel> _OrganizationList;
        private string  _UserLastName;
        private string _UserFirstName;
        private int _UserHighestRole;
        private string _SelectedForm;
        public List<FormInfoModel> FormList
            {
            get { return _FormList; }
            set { _FormList = value; }
            }
        public List<OrganizationModel> OrganizationList
            {

            get { return _OrganizationList; }
            set { _OrganizationList = value; }
            }
        public string UserLastName
            {
            get { return _UserLastName; }
            set { _UserLastName = value; }
            }
        public string UserFirstName
            {
            get { return _UserFirstName; }
            set { _UserFirstName = value; }
            }

        public int UserHighestRole
            {
            get { return _UserHighestRole; }
            set { _UserHighestRole = value; }
            }
        public string SelectedForm
            {
            get { return _SelectedForm; }
            set { _SelectedForm = value; }
            }
        }
    }