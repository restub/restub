using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Restub.Toolbox;

namespace Restub.Tests.Cdek
{
    /// <summary>
    /// Delivery order.
    /// EN: https://api-docs.cdek.ru/33828802.html
    /// RU: https://api-docs.cdek.ru/29923926.html
    /// </summary>
    [DataContract]
    public class CdekOrderRequest
    {
        [DataMember(Name = "type")]
        public int? DeliveryType { get; set; } // default is 1 = online store

        [DataMember(Name = "number")]
        public string OrderNumber { get; set; }

        [DataMember(Name = "tariff_code")]
        public int TariffCode { get; set; }

        [DataMember(Name = "comment")]
        public string Comment { get; set; }

        [DataMember(Name = "developer_key")]
        public string DeveloperKey { get; set; } // for module developers only

        [DataMember(Name = "shipment_point")]
        public string ShipmentPoint { get; set; } // either ShipmentPoint or FromLocation should be specified

        [DataMember(Name = "delivery_point")]
        public string DeliveryPoint { get; set; } // either DeliveryPoint or ToLocation should be specified

        [DataMember(Name = "date_invoice"), JsonConverter(typeof(DateOnlyConverter))]
        public DateTime? InvoiceDate { get; set; } // for international online orders only

        [DataMember(Name = "shipper_name")]
        public string ShipperName { get; set; } // for international online orders only

        [DataMember(Name = "shipper_address")]
        public string ShipperAddress { get; set; } // for international online orders only

        [DataContract]
        public class Location
        {
            [DataMember(Name = "longitude")]
            public decimal Longitude { get; set; }

            [DataMember(Name = "latitude")]
            public decimal Latitude { get; set; }

            [DataMember(Name = "city")]
            public string City { get; set; } // "Москва"

            [DataMember(Name = "address")]
            public string Address { get; set; } // street address in the specified city
        }

        [DataMember(Name = "from_location")]
        public Location FromLocation { get; set; }

        [DataMember(Name = "to_location")]
        public Location ToLocation { get; set; }

        [DataContract]
        public class Package
        {
            [DataMember(Name = "weight")]
            public int Weight { get; set; } // grams

            [DataMember(Name = "height")]
            public int? Height { get; set; } // cm

            [DataMember(Name = "length")]
            public int? Length { get; set; } // cm

            [DataMember(Name = "width")]
            public int? Width { get; set; } // cm

            [DataMember(Name = "number")]
            public string Number { get; set; } // unique number of the package within the order, like order number in client's system

            [DataMember(Name = "comment")]
            public string Comments { get; set; } // required for delivery type of orders
        }

        [DataMember(Name = "packages")]
        public Package[] Packages { get; set; }

        [DataContract]
        public class Person
        {
            [DataMember(Name = "name")]
            public string Name { get; set; } // not required for online e-shop orders

            [DataMember(Name = "company")]
            public string Company { get; set; } // not required for online e-shop orders

            [DataMember(Name = "email")]
            public string Email { get; set; } // not required for online e-shop orders

            [DataMember(Name = "passport_series")]
            public string PassportSeries { get; set; } // optional

            [DataMember(Name = "passport_number")]
            public string PassportNumber { get; set; } // optional

            [DataMember(Name = "passport_date_of_issue"), JsonConverter(typeof(DateOnlyConverter))]
            public DateTime? PassportDate { get; set; } // optional

            [DataMember(Name = "passport_organization")]
            public string PassportOrganization { get; set; } // optional

            [DataMember(Name = "passport_date_of_birth"), JsonConverter(typeof(DateOnlyConverter))]
            public DateTime? PassportBirthDate { get; set; } // optional

            [DataMember(Name = "tin")]
            public string Tin { get; set; } // optional INN, Taxpayer Identification Number

            [DataContract]
            public class Phone
            {
                [DataMember(Name = "number")]
                public string Number { get; set; }

                [DataMember(Name = "additional")]
                public string Additional { get; set; }
            }

            [DataMember(Name = "phones")]
            public Phone[] Phones { get; set; } // not required for online e-shop orders
        }

        [DataMember(Name = "sender")]
        public Person Sender { get; set; } // not required for online e-shop orders

        [DataMember(Name = "recipient")]
        public Person Recipient { get; set; }
    }
}
