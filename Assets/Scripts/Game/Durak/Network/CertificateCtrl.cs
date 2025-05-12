using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class CertificateCtrl
    {
        public CertificateCtrl()
        {
            ServicePointManager.ServerCertificateValidationCallback =
                (sender, certificate, chain, sslPolicyErrors) =>
                {
                    var isOk = true;
                    // If there are errors in the certificate chain, look at each error to determine the cause.
                    if (sslPolicyErrors == SslPolicyErrors.None) 
                        return isOk;
                    
                    foreach (var t in chain.ChainStatus)
                    {
                        if (t.Status == X509ChainStatusFlags.RevocationStatusUnknown) 
                            continue;
                            
                        chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                        chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                        chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                        chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                        var chainIsValid = chain.Build((X509Certificate2) certificate);
                        Debug.Log($"ChainIsValid: {chainIsValid}");
                        if (!chainIsValid)
                            isOk = false;
                    }

                    return isOk;
                };
        }
        
        private enum CertificateProblem : uint
        {
            None = 0x00000000,
            CertExpired = 0x800B0101,
            CertValidityPeriodNesting = 0x800B0102,
            CertRole = 0x800B0103,
            CertPathLenConst = 0x800B0104,
            CertCritical = 0x800B0105,
            CertPurpose = 0x800B0106,
            CertIssuerChaining = 0x800B0107,
            CertMalformed = 0x800B0108,
            CertUntrustedRoot = 0x800B0109,
            CertChaining = 0x800B010A,
            CertRevoked = 0x800B010C,
            CertUntrustedTestRoot = 0x800B010D,
            CertRevocationFailure = 0x800B010E,
            CertCnNoMatch = 0x800B010F,
            CertWrongUsage = 0x800B0110,
            CertUntrustedCa = 0x800B0112,

            //https://docs.microsoft.com/ru-ru/office/troubleshoot/error-messages/generic-trust-failure-(0x800b010b)-error
            GeneralFailureOfTrust = 0x800B010B
        };
        
        private class StubCertificatePolicy : ICertificatePolicy
        {
            private const bool DefaultValidate = true;

            public bool CheckValidationResult(ServicePoint srvPoint, X509Certificate certificate, WebRequest request,
                int certificateProblem)
            {
                if ((CertificateProblem) certificateProblem != CertificateProblem.None)
                {
                    Debug.Log($"Certificate Problem with accessing {request?.RequestUri}");
                    Debug.Log($"Problem code 0x{certificateProblem:X8},");
                    Debug.LogError(GetProblemMessage((CertificateProblem) certificateProblem));
                    return true;
                }

                return DefaultValidate;
            }

            private static string GetProblemMessage(CertificateProblem problem)
            {
                var problemMessage = string.Empty;
                var problemCodeName = Enum.GetName(typeof(CertificateProblem), problem);
                problemMessage = !string.IsNullOrEmpty(problemCodeName)
                    ? $"{problemMessage} Certificate problem:{problem}  code:{problemCodeName}"
                    : "Unknown Certificate Problem";

                return problemMessage;
            }
        }
    }