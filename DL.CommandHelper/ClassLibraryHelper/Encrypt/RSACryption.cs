using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ClassLibraryHelper.Encrypt
{
    /// <summary> 
    /// RSA加密解密及RSA签名和验证
    /// </summary> 
    public class RSACryption
    {
        #region RSA 加密解密

        #region RSA 的密钥产生

        /// <summary>
        /// RSA 的密钥产生 产生私钥 和公钥 
        /// </summary>
        /// <param name="xmlKeys"></param>
        /// <param name="xmlPublicKey"></param>
        public void RSAKey(out string xmlKeys, out string xmlPublicKey)
        {
            var rsa = new RSACryptoServiceProvider();
            xmlKeys = rsa.ToXmlString(true);
            xmlPublicKey = rsa.ToXmlString(false);
        }

        #endregion

        #region RSA的加密函数

        //############################################################################## 
        //RSA 方式加密 
        //说明KEY必须是XML的行式,返回的是字符串 
        //在有一点需要说明！！该加密方式有 长度 限制的！！ 
        //############################################################################## 

        //RSA的加密函数  string
        public string RSAEncrypt(string xmlPublicKey, string mStrEncryptString)
        {
            byte[] plainTextBArray;
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPublicKey);
            plainTextBArray = (new UnicodeEncoding()).GetBytes(mStrEncryptString);
            byte[] cypherTextBArray = rsa.Encrypt(plainTextBArray, false);
            string result = Convert.ToBase64String(cypherTextBArray);
            return result;
        }

        //RSA的加密函数 byte[]
        public string RSAEncrypt(string xmlPublicKey, byte[] encryptString)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPublicKey);
            byte[] cypherTextBArray = rsa.Encrypt(encryptString, false);
            string result = Convert.ToBase64String(cypherTextBArray);
            return result;
        }

        #endregion

        #region RSA的解密函数

        //RSA的解密函数  string
        public string RSADecrypt(string xmlPrivateKey, string mStrDecryptString)
        {
            byte[] plainTextBArray;
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPrivateKey);
            plainTextBArray = Convert.FromBase64String(mStrDecryptString);
            byte[] dypherTextBArray = rsa.Decrypt(plainTextBArray, false);
            string result = (new UnicodeEncoding()).GetString(dypherTextBArray);
            return result;
        }

        //RSA的解密函数  byte
        public string RSADecrypt(string xmlPrivateKey, byte[] decryptString)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPrivateKey);
            byte[] dypherTextBArray = rsa.Decrypt(decryptString, false);
            string result = (new UnicodeEncoding()).GetString(dypherTextBArray);
            return result;
        }

        #endregion

        #endregion

        #region RSA数字签名

        #region 获取Hash描述表

        //获取Hash描述表 
        public bool GetHash(string mStrSource, ref byte[] hashData)
        {
            //从字符串中取得Hash描述 
            HashAlgorithm md5 = HashAlgorithm.Create("MD5");
            byte[] buffer = Encoding.GetEncoding("GB2312").GetBytes(mStrSource);
            hashData = md5.ComputeHash(buffer);

            return true;
        }

        //获取Hash描述表 
        public bool GetHash(string mStrSource, ref string strHashData)
        {
            //从字符串中取得Hash描述 
            HashAlgorithm md5 = HashAlgorithm.Create("MD5");
            byte[] buffer = Encoding.GetEncoding("GB2312").GetBytes(mStrSource);
            byte[] hashData = md5.ComputeHash(buffer);

            strHashData = Convert.ToBase64String(hashData);
            return true;
        }

        //获取Hash描述表 
        public bool GetHash(FileStream objFile, ref byte[] hashData)
        {
            //从文件中取得Hash描述 
            HashAlgorithm md5 = HashAlgorithm.Create("MD5");
            hashData = md5.ComputeHash(objFile);
            objFile.Close();

            return true;
        }

        //获取Hash描述表 
        public bool GetHash(FileStream objFile, ref string strHashData)
        {
            //从文件中取得Hash描述 
            byte[] hashData;
            HashAlgorithm MD5 = HashAlgorithm.Create("MD5");
            hashData = MD5.ComputeHash(objFile);
            objFile.Close();

            strHashData = Convert.ToBase64String(hashData);

            return true;
        }

        #endregion

        #region RSA签名

        //RSA签名 
        public bool SignatureFormatter(string pStrKeyPrivate,
                                       byte[] hashbyteSignature,
                                       ref byte[] encryptedSignatureData)
        {
            var rsa = new RSACryptoServiceProvider();

            rsa.FromXmlString(pStrKeyPrivate);
            var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
            //设置签名的算法为MD5 
            rsaFormatter.SetHashAlgorithm("MD5");
            //执行签名 
            encryptedSignatureData = rsaFormatter.CreateSignature(hashbyteSignature);

            return true;
        }

        //RSA签名 
        public bool SignatureFormatter(string pStrKeyPrivate,
                                       byte[] hashbyteSignature,
                                       ref string mStrEncryptedSignatureData)
        {
            var rsa = new RSACryptoServiceProvider();

            rsa.FromXmlString(pStrKeyPrivate);
            var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
            //设置签名的算法为MD5 
            rsaFormatter.SetHashAlgorithm("MD5");
            //执行签名 
            byte[] encryptedSignatureData = rsaFormatter.CreateSignature(hashbyteSignature);

            mStrEncryptedSignatureData = Convert.ToBase64String(encryptedSignatureData);

            return true;
        }

        //RSA签名 
        public bool SignatureFormatter(string pStrKeyPrivate,
                                       string mStrHashbyteSignature,
                                       ref byte[] encryptedSignatureData)
        {
            byte[] hashbyteSignature = Convert.FromBase64String(mStrHashbyteSignature);
            var rsa = new RSACryptoServiceProvider();

            rsa.FromXmlString(pStrKeyPrivate);
            var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
            //设置签名的算法为MD5 
            rsaFormatter.SetHashAlgorithm("MD5");
            //执行签名 
            encryptedSignatureData = rsaFormatter.CreateSignature(hashbyteSignature);

            return true;
        }

        //RSA签名 
        public bool SignatureFormatter(string pStrKeyPrivate,
                                       string mStrHashbyteSignature,
                                       ref string mStrEncryptedSignatureData)
        {
            byte[] hashbyteSignature = Convert.FromBase64String(mStrHashbyteSignature);
            var rsa = new RSACryptoServiceProvider();

            rsa.FromXmlString(pStrKeyPrivate);
            var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
            //设置签名的算法为MD5 
            rsaFormatter.SetHashAlgorithm("MD5");
            //执行签名 
            byte[] encryptedSignatureData = rsaFormatter.CreateSignature(hashbyteSignature);

            mStrEncryptedSignatureData = Convert.ToBase64String(encryptedSignatureData);

            return true;
        }

        #endregion

        #region RSA 签名验证

        public bool SignatureDeformatter(string pStrKeyPublic,
                                         byte[] hashbyteDeformatter,
                                         byte[] deformatterData)
        {
            var rsa = new RSACryptoServiceProvider();

            rsa.FromXmlString(pStrKeyPublic);
            var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
            //指定解密的时候HASH算法为MD5 
            rsaDeformatter.SetHashAlgorithm("MD5");

            if (rsaDeformatter.VerifySignature(hashbyteDeformatter, deformatterData))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SignatureDeformatter(string pStrKeyPublic,
                                         string pStrHashbyteDeformatter,
                                         byte[] deformatterData)
        {
            byte[] hashbyteDeformatter = Convert.FromBase64String(pStrHashbyteDeformatter);

            var rsa = new RSACryptoServiceProvider();

            rsa.FromXmlString(pStrKeyPublic);
            var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
            //指定解密的时候HASH算法为MD5 
            rsaDeformatter.SetHashAlgorithm("MD5");

            if (rsaDeformatter.VerifySignature(hashbyteDeformatter, deformatterData))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SignatureDeformatter(string pStrKeyPublic,
                                         byte[] hashbyteDeformatter,
                                         string pStrDeformatterData)
        {
            var rsa = new RSACryptoServiceProvider();

            rsa.FromXmlString(pStrKeyPublic);
            var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
            //指定解密的时候HASH算法为MD5 
            rsaDeformatter.SetHashAlgorithm("MD5");

            byte[] deformatterData = Convert.FromBase64String(pStrDeformatterData);

            if (rsaDeformatter.VerifySignature(hashbyteDeformatter, deformatterData))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SignatureDeformatter(string pStrKeyPublic,
                                         string pStrHashbyteDeformatter,
                                         string pStrDeformatterData)
        {
            byte[] hashbyteDeformatter = Convert.FromBase64String(pStrHashbyteDeformatter);
            var rsa = new RSACryptoServiceProvider();

            rsa.FromXmlString(pStrKeyPublic);
            var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
            //指定解密的时候HASH算法为MD5 
            rsaDeformatter.SetHashAlgorithm("MD5");

            byte[] deformatterData = Convert.FromBase64String(pStrDeformatterData);

            if (rsaDeformatter.VerifySignature(hashbyteDeformatter, deformatterData))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #endregion
    }
}