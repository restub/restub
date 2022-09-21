using System.Runtime.Serialization;

namespace Restub.Tests.Pochta.Otpravka
{
    [DataContract]
    public class OtpravkaFullName
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }

        [DataMember(Name = "middle-name")]
        public string MiddleName { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "original-fio")]
        public string OriginalFullName { get; set; }

        [DataMember(Name = "quality-code")]
        public string QualityCode { get; set; }

        [DataMember(Name = "surname")]
        public string Surname { get; set; }
    }
}
