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

    public static class OpenGPG
    {


        public static PgpSecretKey ReadSecretKey(Stream input)
        {

            var pgpSec = new PgpSecretKeyRingBundle(PgpUtilities.GetDecoderStream(input));

            // we just loop through the collection till we find a key suitable for encryption, in the real
            // world you would probably want to be a bit smarter about this.
            foreach (var keyRing in pgpSec.GetKeyRings())
            {
                foreach (var key in keyRing.SecretKeys)
                {
                    if (key.IsSigningKey)
                        return key;
                }
            }

            throw new ArgumentException("Can't find signing key in key ring.");

        }




        public static PgpSignature CreateSignature(Stream          InputStream,
                                                   Stream          OutputStream,
                                                   PgpSecretKey    SecretKey,
                                                   String          Passphrase,
                                                   HashAlgorithms  HashAlgorithm  = HashAlgorithms.Sha512,
                                                   Boolean         ArmoredOutput  = true,
                                                   UInt32          BufferSize     = 2*1024*1024) // 2 MByte
        {

            #region Init signature generator

            var SignatureGenerator  = new PgpSignatureGenerator(SecretKey.PublicKey.Algorithm,
                                                                HashAlgorithm);

            SignatureGenerator.InitSign(PgpSignatures.BinaryDocument,
                                        SecretKey.ExtractPrivateKey(Passphrase));

            #endregion

            #region Read input and update the signature generator

            var InputBuffer  = new Byte[BufferSize];
            var read         = 0;

            do
            {

                read = InputStream.Read(InputBuffer, 0, InputBuffer.Length);
                SignatureGenerator.Update(InputBuffer, 0, read);

            } while (read == BufferSize);

            InputStream.Close();

            #endregion

            #region Write signature to output stream

            BcpgOutputStream WrappedOutputStream = null;

            if (ArmoredOutput)
                WrappedOutputStream = new BcpgOutputStream(new ArmoredOutputStream(OutputStream));
            else
                WrappedOutputStream = new BcpgOutputStream(OutputStream);

            var Signature = SignatureGenerator.Generate();
            Signature.Encode(WrappedOutputStream);

            WrappedOutputStream.Flush();
            WrappedOutputStream.Close();

            // ArmoredOutputStream will not close the underlying stream!
            if (ArmoredOutput)
            {
                OutputStream.Flush();
                OutputStream.Close();
            }

            #endregion

            return Signature;

        }


        public static Task<JObject> SignGeoJSON(this Task<JObject>  JSON,
                                                String              OutputFile,
                                                PgpSecretKey        SecretKey,
                                                String              Passphrase,
                                                HashAlgorithms      HashAlgorithm  = HashAlgorithms.Sha512,
                                                Boolean             ArmoredOutput  = true,
                                                UInt32              BufferSize     = 2*1024*1024) // 2 MByte
        {

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
