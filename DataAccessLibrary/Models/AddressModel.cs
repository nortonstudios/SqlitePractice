using System.Net.NetworkInformation;

namespace DataAccessLibrary.Models
{
    public class AddressModel
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZIP { get; set; }
    }
}