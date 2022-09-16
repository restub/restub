using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using RestSharp;
using RestSharp.Serialization.Xml;
using RestSharp.Serializers;

namespace Restub.Tests
{
    public class StubRequestBase : IRestRequest
    {
        public bool AlwaysMultipartFormData { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ISerializer JsonSerializer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IXmlSerializer XmlSerializer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Action<Stream, IHttpResponse> AdvancedResponseWriter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Action<Stream> ResponseWriter { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<Parameter> Parameters => throw new NotImplementedException();
        public List<FileParameter> Files => throw new NotImplementedException();
        public Method Method { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Resource { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataFormat RequestFormat { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string RootElement { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string DateFormat { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string XmlNamespace { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ICredentials Credentials { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Timeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int ReadWriteTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Attempts => throw new NotImplementedException();
        public bool UseDefaultCredentials { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IList<DecompressionMethods> AllowedDecompressionMethods => throw new NotImplementedException();
        public Action<IRestResponse> OnBeforeDeserialization { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Action<IHttp> OnBeforeRequest { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public RequestBody Body { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IRestRequest AddBody(object obj, string xmlNamespace) => throw new NotImplementedException();
        public IRestRequest AddBody(object obj) => throw new NotImplementedException();
        public IRestRequest AddCookie(string name, string value) => throw new NotImplementedException();
        public IRestRequest AddDecompressionMethod(DecompressionMethods decompressionMethod) => throw new NotImplementedException();
        public IRestRequest AddFile(string name, string path, string contentType = null) => throw new NotImplementedException();
        public IRestRequest AddFile(string name, byte[] bytes, string fileName, string contentType = null) => throw new NotImplementedException();
        public IRestRequest AddFile(string name, Action<Stream> writer, string fileName, long contentLength, string contentType = null) => throw new NotImplementedException();
        public IRestRequest AddFileBytes(string name, byte[] bytes, string filename, string contentType = "application/x-gzip") => throw new NotImplementedException();
        public IRestRequest AddHeader(string name, string value) => throw new NotImplementedException();
        public IRestRequest AddHeaders(ICollection<KeyValuePair<string, string>> headers) => throw new NotImplementedException();
        public IRestRequest AddJsonBody(object obj) => throw new NotImplementedException();
        public IRestRequest AddJsonBody(object obj, string contentType) => throw new NotImplementedException();
        public IRestRequest AddObject(object obj, params string[] includedProperties) => throw new NotImplementedException();
        public IRestRequest AddObject(object obj) => throw new NotImplementedException();
        public IRestRequest AddOrUpdateHeader(string name, string value) => throw new NotImplementedException();
        public IRestRequest AddOrUpdateHeaders(ICollection<KeyValuePair<string, string>> headers) => throw new NotImplementedException();
        public IRestRequest AddOrUpdateParameter(Parameter parameter) => throw new NotImplementedException();
        public IRestRequest AddOrUpdateParameter(string name, object value) => throw new NotImplementedException();
        public IRestRequest AddOrUpdateParameter(string name, object value, ParameterType type) => throw new NotImplementedException();
        public IRestRequest AddOrUpdateParameter(string name, object value, string contentType, ParameterType type) => throw new NotImplementedException();
        public IRestRequest AddOrUpdateParameters(IEnumerable<Parameter> parameters) => throw new NotImplementedException();
        public IRestRequest AddParameter(Parameter p) => throw new NotImplementedException();
        public IRestRequest AddParameter(string name, object value) => throw new NotImplementedException();
        public virtual IRestRequest AddParameter(string name, object value, ParameterType type) => throw new NotImplementedException();
        public IRestRequest AddParameter(string name, object value, string contentType, ParameterType type) => throw new NotImplementedException();
        public IRestRequest AddQueryParameter(string name, string value) => throw new NotImplementedException();
        public IRestRequest AddQueryParameter(string name, string value, bool encode) => throw new NotImplementedException();
        public IRestRequest AddUrlSegment(string name, string value) => throw new NotImplementedException();
        public IRestRequest AddUrlSegment(string name, string value, bool encode) => throw new NotImplementedException();
        public IRestRequest AddUrlSegment(string name, object value) => throw new NotImplementedException();
        public IRestRequest AddXmlBody(object obj) => throw new NotImplementedException();
        public IRestRequest AddXmlBody(object obj, string xmlNamespace) => throw new NotImplementedException();
        public void IncreaseNumAttempts() => throw new NotImplementedException();
    }
}
