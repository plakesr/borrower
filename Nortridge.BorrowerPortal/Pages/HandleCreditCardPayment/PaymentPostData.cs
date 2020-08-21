// <copyright file="PaymentPostData.cs" company="Nortridge Software">
// Copyright (c) Nortridge Software. All rights reserved.
// </copyright>

namespace Nortridge.BorrowerPortal.Pages.HandleCreditCardPayment
{
    public class PaymentPostData
    {
        // Status string, ex: Declined or Accepted
        public string Status { get; set; } // Accepted

        public string Customdata { get; set; } // 0387260AA59C5918 

        // Name on card
        public string Ccname { get; set; } // name44

        public double Amount { get; set; } // 0.33

        public string SubId { get; set; } // NORTR

        // Online Commerce Suite Gateway Account ID, ex: TEST0        public string AcctId { get; set; } // FMEPX

        // Last four of card number/check number prepended by asterisks, ex: ************4444
        public string AccountNumber { get; set; } // ************5454

        public string HistoryId { get; set; } // 1475110932

        // Online Commerce Suite Gateway Order ID
        public string OrderId { get; set; } // 1193844882

    /* Not used fields

        // Expiration month of CC
        public int ExpMon { get; set; } // 12

        // Expiration year of CC
        public int ExpYear { get; set; } // 2020

        // Client IP address
        public string Ci_ipaddress { get; set; } // **.**.**.**

        // Encoded post-back URL
        public string Postbackurl { get; set; } // 435F18C29B53878FF2CA90F4C3C6BF6C09B7696C27ABACC97CC50C63889C8CAFE6E84FA218AA5AEA6D674E9EA74D83E9

        // Id of used form for submit payment
        public string Formid { get; set; } // 449F1F12A44FC6BEDD67F4C0E7A1C66569E41AF732F38C5C064CCCBFF02CD7B3

        public string SessionId { get; set; } // E49FC2FBA6FF2DD9

        public string AuthNo { get; set; } // SALE:TEST:::1475110932:N::M

        public long RecurId { get; set; } // 0

        // Online Commerce Suite transaction ID
        public long TransId { get; set; } // 0

        public string PayType { get; set; } // MasterCard

        public string RefCode { get; set; } // 1475110932-TEST

        public int Result { get; set; } // 1

        // Always returned for accepted transactions
        public string Accepted { get; set; } // SALE:TEST:::1475110932:N::M

        public string AuthCode { get; set; } // TEST

    */
    }
}
