using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace API.All.SYSTEM.CoreAPI.Authentication
{
    public class SamlValidator
    {
        // This is the secret key used for HS256 algorithm
        private const string SecretKey = "your_secret_key_here";

        public static bool ValidateSamlAssertion(string samlAssertion)
        {
            // Step 1: Parse the SAML assertion XML
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(samlAssertion);

            // Step 2: Extract the SAML signature value
            XmlNodeList signatureValueNodes = xmlDoc.GetElementsByTagName("SignatureValue");
            if (signatureValueNodes.Count != 1)
            {
                // Handle error: Invalid number of SignatureValue elements
                return false;
            }

            string? signatureValue = signatureValueNodes?[0]?.InnerText;

            if (signatureValue == null)
            {
                return false;
            }

            // Step 3: Verify the signature using HMAC-SHA256
            if (!VerifySignature(xmlDoc, signatureValue))
            {
                // Handle error: Signature verification failed
                return false;
            }

            // Additional validation steps can be added here based on your requirements

            return true;
        }

        private static bool VerifySignature(XmlDocument xmlDoc, string signatureValue)
        {
            // Step 1: Remove the Signature element from the XML
            XmlNodeList signatureNodes = xmlDoc.GetElementsByTagName("Signature");
            if (signatureNodes.Count != 1)
            {
                // Handle error: Invalid number of Signature elements
                return false;
            }

            if (signatureNodes == null || signatureNodes[0] == null || signatureNodes[0]?.ParentNode == null) return false;

            signatureNodes[0]?.ParentNode?.RemoveChild(signatureNodes[0]!);

            // Step 2: Canonicalize the XML
            string canonicalizedXml = xmlDoc.OuterXml;

            // Step 3: Hash the canonicalized XML using HMAC-SHA256 and the secret key
            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(SecretKey);
            using (HMACSHA256 hmac = new HMACSHA256(secretKeyBytes))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(canonicalizedXml));
                string computedSignature = Convert.ToBase64String(hashBytes);

                // Step 4: Compare the computed signature with the received signature
                return computedSignature == signatureValue;
            }
        }
    }
}
