using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace LSPosMVC.Common.Paycollect
{
    public class Authorization
    {
        public static readonly string EMPTY_BODY_SHA256 = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";
        public static readonly string UNSIGNED_PAYLOAD = "UNSIGNED-PAYLOAD";
        public static readonly string X_OP_AUTHORIZATION_HEADER = "X-OP-Authorization";
        public static readonly string X_OP_DATE_HEADER = "X-OP-Date";
        public static readonly string X_OP_EXPIRES_HEADER = "X-OP-Expires";
        public static readonly string SCHEME = "OWS1";
        public static readonly string ALGORITHM = "OWS1-HMAC-SHA256";
        public static readonly string TERMINATOR = "ows1_request";
        public static readonly string yyyyMMdd = "yyyyMMdd";
        public static readonly string yyyyMMddTHHmmssZ = "yyyyMMddTHHmmssZ";

        private string httpMethod;
        private string uri;
        private IDictionary<string, string> queryParameters = new SortedDictionary<string, string>();
        private string algorithm;
        private string credential;
        private string region;
        private string service;
        private string terminator;
        private IDictionary<string, string> signedHeaders = new SortedDictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        private string signedHeaderNames;
        private string signature;
        private string accessKeyId;
        private string secretAccessKey;
        private int expires;
        private DateTime timeStamp;
        private byte[] payload;
        private IDictionary<string, string> debugInfo = new SortedDictionary<string, string>();

        public Authorization(string accessKeyId, string secretAccessKey, string region, string service,
                             string httpMethod, string uri, IDictionary<string, string> queryParameters, IDictionary<string, string> signedHeaders, byte[] payload)
            : this(accessKeyId, secretAccessKey, region, service, httpMethod, uri, queryParameters, signedHeaders, payload,
                  DateTime.ParseExact(signedHeaders[X_OP_DATE_HEADER], yyyyMMddTHHmmssZ, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal),
                  int.Parse(signedHeaders[X_OP_EXPIRES_HEADER])
                  )
        { }

        public Authorization(string accessKeyId, string secretAccessKey, string region, string service,
                             string httpMethod, string uri, IDictionary<string, string> queryParameters, IDictionary<string, string> signedHeaders, byte[] payload, DateTime dateTime, int expires)
            : this(ALGORITHM, accessKeyId, secretAccessKey, region, service, TERMINATOR, httpMethod, uri, queryParameters, signedHeaders, payload, dateTime, expires)
        { }

        public Authorization(
            string algorithm,
            string accessKeyId,
            string secretAccessKey,
            string region,
            string service,
            string terminator,
            string httpMethod,
            string uri,
            IDictionary<string, string> queryParameters,
            IDictionary<string, string> signedHeaders,
            byte[] payload,
            DateTime timeStamp,
            int expires)
        {
            this.algorithm = algorithm;
            this.accessKeyId = accessKeyId;
            this.secretAccessKey = secretAccessKey;
            this.timeStamp = timeStamp;
            this.region = region;
            this.service = service;
            this.terminator = terminator;
            this.credential = accessKeyId + "/" + timeStamp.ToString(yyyyMMdd, CultureInfo.InvariantCulture) + "/" + region + "/" + service + "/" + terminator;
            this.httpMethod = httpMethod;
            this.uri = uri;
            if (queryParameters != null) this.queryParameters = new SortedDictionary<string, string>(queryParameters);
            if (signedHeaders != null) this.signedHeaders = new SortedDictionary<string, string>(signedHeaders, StringComparer.InvariantCultureIgnoreCase);
            this.payload = payload;
            this.expires = expires;
            sign();
        }

        //For server only
        private static readonly Regex pAuthorization = new Regex("^([^ ]+) Credential=([^/]+/\\d{8}/[^/]+/[^/]+/[^,]+),SignedHeaders=([-_a-z0-9;]*),Signature=([a-f0-9]{64})$");
        private static readonly Regex pCredential = new Regex("^([^/]+)/(\\d{8})/([^/]+)/([^/]+)/([^,]+)$");
        private static readonly Regex pSignedHeaders = new Regex("([^;]+)");
        private static readonly Regex pSignature = new Regex("^[a-f0-9]{64}$");
        public Authorization(string authorizationHeader, string timeStamp, int expires)
        {
            try
            {
                this.timeStamp = DateTime.ParseExact(timeStamp, yyyyMMddTHHmmssZ, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
                this.expires = expires;
                if (authorizationHeader != null)
                {
                    Match mAuthorization = pAuthorization.Match(authorizationHeader);
                    if (mAuthorization.Success)
                    {
                        algorithm = mAuthorization.Groups[1].Value;
                        credential = mAuthorization.Groups[2].Value;
                        Match mCredential = pCredential.Match(credential);
                        if (mCredential.Success)
                        {
                            accessKeyId = mCredential.Groups[1].Value;
                            //timeStamp = yyyyMMdd.parse(mCredential.group(2));
                            region = mCredential.Groups[3].Value;
                            service = mCredential.Groups[4].Value;
                            terminator = mCredential.Groups[5].Value;
                        }
                        signedHeaderNames = mAuthorization.Groups[3].Value;
                        Match mHeaders = pSignedHeaders.Match(signedHeaderNames);
                        while (mHeaders.Success)
                        {
                            signedHeaders.Add(mHeaders.Groups[1].Value, "");
                            mHeaders = mHeaders.NextMatch();
                        }
                        signature = mAuthorization.Groups[4].Value;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static readonly Regex pAlgorithm = new Regex("^([^-]+)-(.+)$");
        public Authorization(string algorithm, string credential, string signedHeaderNames, string signature, string timeStamp, int expires)
        {
            try
            {
                this.timeStamp = DateTime.ParseExact(timeStamp, yyyyMMddTHHmmssZ, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
                this.expires = expires;
                this.algorithm = algorithm;
                if (credential != null && signedHeaderNames != null && signature != null)
                {
                    Match mCredential = pCredential.Match(credential);
                    if (mCredential.Success)
                    {
                        this.credential = credential;
                        accessKeyId = mCredential.Groups[1].Value;
                        //timeStamp = yyyyMMdd.parse(mCredential.group(2));
                        region = mCredential.Groups[3].Value;
                        service = mCredential.Groups[4].Value;
                        terminator = mCredential.Groups[5].Value;
                    }
                    this.signedHeaderNames = signedHeaderNames;
                    Match mHeaders = pSignedHeaders.Match(signedHeaderNames);
                    while (mHeaders.Success)
                    {
                        this.signedHeaders.Add(mHeaders.Groups[1].Value, "");
                        mHeaders = mHeaders.NextMatch();
                    }

                    Match mSignature = pSignature.Match(signature);
                    if (mSignature.Success) this.signature = signature;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public string HttpMethod { get { return httpMethod; } }
        public string Uri { get { return uri; } }
        public IDictionary<string, string> QueryParameters { get { return queryParameters; } }
        public string Algorithm { get { return algorithm; } }
        public string Credential { get { return credential; } }
        public string AccessKeyId { get { return accessKeyId; } }
        public string Region { get { return region; } }
        public string Service { get { return service; } }
        public string Terminator { get { return terminator; } }
        public IDictionary<string, string> SignedHeaders { get { return signedHeaders; } }
        public string Signature { get { return signature; } }
        public DateTime TimeStamp { get { return timeStamp; } }
        public string TimeStampString { get { return timeStamp.ToString(yyyyMMddTHHmmssZ, CultureInfo.InvariantCulture); } }
        public int Expires { get { return expires; } }
        public byte[] Payload { get { return payload; } }
        public bool IsExpired { get { return DateTime.UtcNow.CompareTo(timeStamp.AddSeconds(expires)) < 0; } }

        override public string ToString()
        {
            return algorithm + " Credential=" + credential + ",SignedHeaders=" + signedHeaderNames + ",Signature=" + signature;
        }

        public string ToQueryString()
        {
            try
            {
                return "X-OP-Algorithm=" + algorithm +
                        "&X-OP-Credential=" + uriEncode(credential, true) +
                        "&X-OP-Date=" + timeStamp.ToString(yyyyMMddTHHmmssZ, CultureInfo.InvariantCulture) +
                        "&X-OP-Expires=" + expires +
                        "&X-OP-SignedHeaders=" + uriEncode(signedHeaderNames, true) +
                        "&X-OP-Signature=" + signature;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public IDictionary<string, string> DebugInfo { get { return debugInfo; } }

        private void sign()
        {
            try
            {
                string canonicalUri = uriEncode(uri, false);

                StringBuilder canonicalQueryString = new StringBuilder();
                foreach (KeyValuePair<string, string> entry in queryParameters)
                {
                    if (canonicalQueryString.Length > 0) canonicalQueryString.Append("&");
                    canonicalQueryString.Append(uriEncode(entry.Key, true)).Append("=").Append(uriEncode(entry.Value, true));
                }

                StringBuilder canonicalHeaders = new StringBuilder();

                StringBuilder buf = new StringBuilder();

                foreach (KeyValuePair<string, string> entry in signedHeaders)
                {
                    canonicalHeaders.Append(entry.Key.ToLower()).Append(":").Append(entry.Value.Trim()).Append("\n");
                    if (buf.Length > 0) buf.Append(";");
                    buf.Append(entry.Key.ToLower());
                }
                signedHeaderNames = buf.ToString();

                string hashedPayload = (payload != null) ? (payload.Length > 0 ? hex(sha256Hash(payload)) : EMPTY_BODY_SHA256) : UNSIGNED_PAYLOAD;

                string canonicalRequest = httpMethod + "\n" +
                        canonicalUri + "\n" +
                        canonicalQueryString.ToString() + "\n" +
                        canonicalHeaders.ToString() + "\n" +
                        signedHeaderNames + "\n" +
                        hashedPayload;

                string timeStamp = this.timeStamp.ToUniversalTime().ToString(yyyyMMddTHHmmssZ, CultureInfo.InvariantCulture);

                string scope = this.timeStamp.ToString(yyyyMMdd, CultureInfo.InvariantCulture) + "/" + region + "/" + service + "/" + terminator;

                string stringToSign = algorithm + "\n" +
                        timeStamp + "\n" +
                        scope + "\n" +
                        hex(sha256Hash(canonicalRequest));

                byte[] dateKey = hmacSha256(SCHEME + secretAccessKey, this.timeStamp.ToString(yyyyMMdd, CultureInfo.InvariantCulture));
                byte[] dateRegionKey = hmacSha256(dateKey, region);
                byte[] dateRegionServiceKey = hmacSha256(dateRegionKey, service);
                byte[] signingKey = hmacSha256(dateRegionServiceKey, terminator);
                signature = hex(hmacSha256(signingKey, stringToSign));

                debugInfo.Add("canonicalRequest", canonicalRequest);
                debugInfo.Add("stringToSign", stringToSign);
                debugInfo.Add("dateKey", hex(dateKey));
                debugInfo.Add("dateRegionKey", hex(dateRegionKey));
                debugInfo.Add("dateRegionServiceKey", hex(dateRegionServiceKey));
                debugInfo.Add("signingKey", hex(signingKey));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static string uriEncode(string data, bool encodeSlash)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char ch in Encoding.UTF8.GetBytes(data))
            {
                if ((ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z')
                    || (ch >= '0' && ch <= '9')
                    || ch == '_' || ch == '-' || ch == '~' || ch == '.'
                    || (ch == '/' && !encodeSlash)) sb.Append(ch);
                else sb.Append("%").Append(string.Format("{0:X2}", (int)ch));
            }
            return sb.ToString();
        }

        private static byte[] hmacSha256(string key, string data) { return hmacSha256(Encoding.UTF8.GetBytes(key), data); }
        private static byte[] hmacSha256(byte[] key, string data)
        {
            var kha = KeyedHashAlgorithm.Create("HMACSHA256");
            kha.Key = key;
            return kha.ComputeHash(Encoding.UTF8.GetBytes(data));
        }

        private static byte[] sha256Hash(string data) { return sha256Hash(Encoding.UTF8.GetBytes(data)); }

        private static HashAlgorithm SHA256Algorithm = HashAlgorithm.Create("SHA-256");
        private static byte[] sha256Hash(byte[] data) { return SHA256Algorithm.ComputeHash(data); }

        private static string hex(byte[] data)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in data) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }
}