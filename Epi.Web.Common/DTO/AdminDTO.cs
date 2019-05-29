using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epi.Web.Common.DTO
    {
   public class AdminDTO
       {
       private string _AdminEmail;
       private int _OrganizationId;
       private bool _IsActive;
       private bool _Notify;
       private Guid _AdminId;
       private string _FirstName;
       private string _LastName;
       private string _PhoneNumber;
       private string _AdressLine1;
       private string _AdressLine2;
       private string _City;
       private int _StateId;
       private string _Zip;
       private string _UserName;
       public string AdminEmail
           {
           get { return _AdminEmail; }
           set { _AdminEmail = value; }
           }

      
       public int OrganizationId
           {
           get { return _OrganizationId; }
           set { _OrganizationId = value; }
           }

      
       public bool IsActive
           {
           get { return _IsActive; }
           set { _IsActive = value; }
           }
       public bool Notify
           {
           get { return _Notify; }
           set { _Notify = value; }
           }
       public Guid AdminId
           {
           get { return _AdminId; }
           set { _AdminId = value; }
           }
       public string FirstName
           {
           get { return _FirstName; }
           set { _FirstName = value; }
           }
       
       public string LastName
           {
           get { return _LastName; }
           set { _LastName = value; }
           }
      
       public string PhoneNumber
           {
           get { return _PhoneNumber; }
           set { _PhoneNumber = value; }
           }
       public string AdressLine1
           {
           get { return _AdressLine1; }
           set { _AdressLine1 = value; }
           }
       public string AdressLine2
           {
           get { return _AdressLine2; }
           set { _AdressLine2 = value; }
           }
       
       public string City
           {
           get { return _City; }
           set { _City = value; }
           }

       
       public int StateId
           {
           get { return _StateId; }
           set { _StateId = value; }
           }
        
       public string Zip
           {
           get { return _Zip; }
           set { _Zip = value; }
           }
        public string UserName        {
            get { return _UserName; }
            set { _UserName = value; }
        }
    }
    }
