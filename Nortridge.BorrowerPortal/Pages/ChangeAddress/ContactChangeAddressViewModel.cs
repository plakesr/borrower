// <copyright file="ContactChangeAddressViewModel.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.ChangeAddress
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ContactChangeAddressViewModel
    {
        [Obsolete("Autoconvert only", error: true)]
        public ContactChangeAddressViewModel()
        {
        }

        public ContactChangeAddressViewModel(
            string street1,
            string street2,
            string city,
            string state,
            string zip,
            string county,
            string country)
        {
            this.Street1 = street1;
            this.Street2 = street2;
            this.City = city;
            this.State = state;
            this.Zip = zip;
            this.County = county;
            this.Country = country;
        }

        [MaxLength(40)]
        public string Street1 { get; set; }

        [MaxLength(40)]
        public string Street2 { get; set; }

        [MaxLength(25)]
        public string City { get; set; }

        [MaxLength(3)]
        public string State { get; set; }

        [MaxLength(12)]
        public string Zip { get; set; }

        [MaxLength(30)]
        public string County { get; set; }

        [MaxLength(50)]
        public string Country { get; set; }
    }
}
