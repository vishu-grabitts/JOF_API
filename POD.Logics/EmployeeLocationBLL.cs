using POD.DAL;
using POD.Models;
using System;
using System.Collections.Generic;

namespace POD.Logics
{
    public class EmployeeLocationBLL : EmployeeLocationDLL
    { 
        public bool SaveEmployeeLocation(List<EmployeeLocation> locations)
        { 
            return SaveEmployeeLocation_DLL(locations);
        }

        public List<EmployeeLocationVM> GetEmployeeLocations(int empid, DateTime date)
        {
            return GetEmployeeLocations_DLL(empid, date);
        }

        public List<UserVM> GetUsers(int roleid, int companyid)
        {
            return GetUsers_DLL(roleid, companyid);
        }
    }
}
