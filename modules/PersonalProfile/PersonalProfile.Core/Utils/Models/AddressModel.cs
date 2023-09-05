using System;
using System.Collections.Generic;
using System.Text;

namespace bbk.netcore.mdl.PersonalProfile.Core.Utils.Models
{
    public class AddressModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string SuperiorId { get; set; }
    }

    public class Address
    {
        public List<AddressModel> Addresses { get; set; }
    }

    public class GetAddressInput
    {
        public string FilePath { get; set; }

        public string SuperiorId { get; set; }
    }
}
