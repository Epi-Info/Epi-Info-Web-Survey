using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Text;
//using BusinessObjects;
//using DataObjects.EntityFramework.ModelMapper;
//using System.Linq.Dynamic;
using Epi.Web.Interfaces.DataInterfaces;
using Epi.Web.Common.BusinessObject;
using Epi.Web.Common.Criteria;

namespace Epi.Web.EF
    {
    public class EntityStateDao  :IStateDao
        {
        public List<StateBO> GetStateList() {
        List<StateBO> List = new List<StateBO>();
        

        try
            {
            using (var Context = DataObjectFactory.CreateContext())
                {
                var Query = from state in Context.States
                            
                            select state;

                var DataRow = Query;
                foreach (var Row in DataRow)
                    {

                    List.Add(Mapper.Map(Row));

                    }
                }
            }
        catch (Exception ex)
            {
            throw (ex);
            }


        return List;
            
            
            }
        }
    }
