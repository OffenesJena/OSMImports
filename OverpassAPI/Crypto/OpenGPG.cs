using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using org.GraphDefined.Vanaheimr.Illias;

namespace org.GraphDefined.OpenDataAPI.OverpassAPI
{

    public static class OpenGPGJSON
    {

        public static Task<JObject> SignGeoJSON(this Task<JObject>  JSON,
                                                String              OutputFile,
                                                PgpSecretKey        SecretKey,
                                                String              Passphrase,
                                                HashAlgorithms      HashAlgorithm  = HashAlgorithms.Sha512,
                                                Boolean             ArmoredOutput  = true,
                                                UInt32              BufferSize     = 2*1024*1024) // 2 MByte
        {

            org.GraphDefined.Vanaheimr.BouncyCastle.OpenGPG.
                CreateSignature(new MemoryStream(JSON.Result.ToString().ToUTF8Bytes()),
                                File.OpenWrite(OutputFile),
                                SecretKey,
                                Passphrase,
                                HashAlgorithm,
                                ArmoredOutput,
                                BufferSize);

            return JSON;

        }


    }

}
