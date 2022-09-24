using System.Runtime.Serialization;

namespace Restub.Tests.LocalServer
{
    [DataContract]
    public class LocalhostDummy
    {
        [DataMember(Name = "n")]
        public string Name { get; set; }

        [DataMember(Name = "s")]
        public int Size { get; set; }
    }
}
