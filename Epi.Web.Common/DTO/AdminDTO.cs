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
       }
    }
