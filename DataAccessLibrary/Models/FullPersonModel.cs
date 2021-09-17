using System.Collections.Generic;

namespace DataAccessLibrary.Models
{
    public class FullPersonModel
    {
        public BasicPersonModel Person { get; set; } 
        public List<AddressModel> Addresses { get; set; } = new List<AddressModel>();
        public List<EmployerModel> Employers { get; set; } = new List<EmployerModel>();
    }
}