using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Configuration;
using Epi.Web.Common.Exception;

namespace Epi.Web.SurveyManager.Client
{
    public class ServiceClient
    {

        public enum Version
            {
            SurveyManagerService = 1,
            SurveyManagerServiceV2 = 2,
            }
        public static SurveyManagerService.ManagerServiceClient GetClient(string pEndPointAddress, bool pIsAuthenticated, bool pIsWsHttpBinding = true)
        {
            SurveyManagerService.ManagerServiceClient result = null;
            try
            {
                if (pIsAuthenticated) // Windows Authentication
                {
                    System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();
                    binding.Name = "BasicHttpBinding";
                    binding.CloseTimeout = new TimeSpan(0, 1, 0);
                    binding.OpenTimeout = new TimeSpan(0, 1, 0);
                    binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
                    binding.SendTimeout = new TimeSpan(0, 1, 0);
                    binding.AllowCookies = false;
                    binding.BypassProxyOnLocal = false;
                    binding.HostNameComparisonMode = System.ServiceModel.HostNameComparisonMode.StrongWildcard;
                    binding.MaxBufferPoolSize = long.Parse( ConfigurationManager.AppSettings["MaxBufferPoolSize"]);//524288;
                    binding.MaxReceivedMessageSize = long.Parse(ConfigurationManager.AppSettings["MaxReceivedMessageSize"]); 
                    binding.MessageEncoding = System.ServiceModel.WSMessageEncoding.Text;
                    binding.TextEncoding = System.Text.Encoding.UTF8;
                    binding.TransferMode = System.ServiceModel.TransferMode.Buffered;
                    binding.UseDefaultWebProxy = true;
                    binding.ReaderQuotas.MaxDepth = int.Parse(ConfigurationManager.AppSettings["MaxDepth"]);//32;
                    binding.ReaderQuotas.MaxStringContentLength = int.Parse(ConfigurationManager.AppSettings["MaxStringContentLength"]);  //8192;
                    binding.ReaderQuotas.MaxArrayLength = int.Parse(ConfigurationManager.AppSettings["MaxArrayLength"]); //16384;
                    binding.ReaderQuotas.MaxBytesPerRead = int.Parse(ConfigurationManager.AppSettings["MaxBytesPerRead"]); //4096;
                    binding.ReaderQuotas.MaxNameTableCharCount = int.Parse(ConfigurationManager.AppSettings["MaxNameTableCharCount"]); //16384;

                    System.ServiceModel.EndpointAddress endpoint = new System.ServiceModel.EndpointAddress(pEndPointAddress);

                    if (endpoint.Uri.Scheme == "http")
                    {
                        binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.TransportCredentialOnly;
                        binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.Windows;
                        binding.Security.Transport.ProxyCredentialType = System.ServiceModel.HttpProxyCredentialType.None;
                        binding.Security.Transport.Realm = string.Empty;
                    }
                    else
                    {
                        binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                        binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.None;


                    }
                    binding.Security.Message.ClientCredentialType = System.ServiceModel.BasicHttpMessageCredentialType.UserName;

                   

                   

                    result = new SurveyManagerService.ManagerServiceClient(binding, endpoint);
                    System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                        (se, cert, chain, sslerror) =>
                        {
                            return true;
                        };


                    result.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
                    result.ChannelFactory.Credentials.Windows.ClientCredential = System.Net.CredentialCache.DefaultNetworkCredentials;
                }
                else
                {
                    if (pIsWsHttpBinding)
                    {
                        System.ServiceModel.WSHttpBinding binding = new System.ServiceModel.WSHttpBinding();
                        binding.Name = "WSHttpBinding";
                        binding.CloseTimeout = new TimeSpan(0, 1, 0);
                        binding.OpenTimeout = new TimeSpan(0, 1, 0);
                        binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
                        binding.SendTimeout = new TimeSpan(0, 1, 0);
                        binding.BypassProxyOnLocal = false;
                        binding.TransactionFlow = false;
                        binding.HostNameComparisonMode = System.ServiceModel.HostNameComparisonMode.StrongWildcard;
                        binding.MaxBufferPoolSize = long.Parse(ConfigurationManager.AppSettings["MaxBufferPoolSize"]);//524288;
                        binding.MaxReceivedMessageSize = long.Parse(ConfigurationManager.AppSettings["MaxReceivedMessageSize"]);
                        binding.MessageEncoding = System.ServiceModel.WSMessageEncoding.Text;
                        binding.TextEncoding = System.Text.Encoding.UTF8;
                        binding.UseDefaultWebProxy = true;
                        binding.AllowCookies = false;

                        binding.ReaderQuotas.MaxDepth = int.Parse(ConfigurationManager.AppSettings["MaxDepth"]);//32;
                        binding.ReaderQuotas.MaxStringContentLength = int.Parse(ConfigurationManager.AppSettings["MaxStringContentLength"]);  //8192;
                        binding.ReaderQuotas.MaxArrayLength = int.Parse(ConfigurationManager.AppSettings["MaxArrayLength"]); //16384;
                        binding.ReaderQuotas.MaxBytesPerRead = int.Parse(ConfigurationManager.AppSettings["MaxBytesPerRead"]); //4096;
                        binding.ReaderQuotas.MaxNameTableCharCount = int.Parse(ConfigurationManager.AppSettings["MaxNameTableCharCount"]); //16384;

                        binding.ReliableSession.Ordered = true;
                        binding.ReliableSession.InactivityTimeout = new TimeSpan(0, 10, 0);
                        binding.ReliableSession.Enabled = false;


                        System.ServiceModel.EndpointAddress endpoint = new System.ServiceModel.EndpointAddress(pEndPointAddress);

                       

                        if (endpoint.Uri.Scheme == "http")
                        {
                            binding.Security.Mode = System.ServiceModel.SecurityMode.Message;
                            binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.Windows;
                            binding.Security.Transport.ProxyCredentialType = System.ServiceModel.HttpProxyCredentialType.None;
                            binding.Security.Transport.Realm = string.Empty;
                        }
                        else
                        {
                            //binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                            //binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.None;


                        }


                        binding.Security.Message.ClientCredentialType = System.ServiceModel.MessageCredentialType.Windows;
                        binding.Security.Message.NegotiateServiceCredential = true;

                       

                        result = new SurveyManagerService.ManagerServiceClient(binding, endpoint);
                        System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                        (se, cert, chain, sslerror) =>
                        {
                            return true;
                        };


                    }
                    else
                    {
                        System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();
                        binding.Name = "BasicHttpBinding";
                        binding.CloseTimeout = new TimeSpan(0, 1, 0);
                        binding.OpenTimeout = new TimeSpan(0, 1, 0);
                        binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
                        binding.SendTimeout = new TimeSpan(0, 1, 0);
                        binding.AllowCookies = false;
                        binding.BypassProxyOnLocal = false;
                        binding.HostNameComparisonMode = System.ServiceModel.HostNameComparisonMode.StrongWildcard;
                        binding.MaxBufferPoolSize = long.Parse(ConfigurationManager.AppSettings["MaxBufferPoolSize"]);//524288;
                        binding.MaxReceivedMessageSize = long.Parse(ConfigurationManager.AppSettings["MaxReceivedMessageSize"]);
                        binding.MessageEncoding = System.ServiceModel.WSMessageEncoding.Text;
                        binding.TextEncoding = System.Text.Encoding.UTF8;
                        binding.TransferMode = System.ServiceModel.TransferMode.Buffered;
                        binding.UseDefaultWebProxy = true;
                        binding.ReaderQuotas.MaxDepth = int.Parse(ConfigurationManager.AppSettings["MaxDepth"]);//32;
                        binding.ReaderQuotas.MaxStringContentLength = int.Parse(ConfigurationManager.AppSettings["MaxStringContentLength"]);  //8192;
                        binding.ReaderQuotas.MaxArrayLength = int.Parse(ConfigurationManager.AppSettings["MaxArrayLength"]); //16384;
                        binding.ReaderQuotas.MaxBytesPerRead = int.Parse(ConfigurationManager.AppSettings["MaxBytesPerRead"]); //4096;
                        binding.ReaderQuotas.MaxNameTableCharCount = int.Parse(ConfigurationManager.AppSettings["MaxNameTableCharCount"]); //16384;
                        System.ServiceModel.EndpointAddress endpoint = new System.ServiceModel.EndpointAddress(pEndPointAddress);
                        if (endpoint.Uri.Scheme == "http")
                        {
                            binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.TransportCredentialOnly;
                            binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.Windows;
                            binding.Security.Transport.ProxyCredentialType = System.ServiceModel.HttpProxyCredentialType.None;
                            binding.Security.Transport.Realm = string.Empty;
                        }
                        else
                        {
                            binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                            binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.None;


                        }
                        

                        result = new SurveyManagerService.ManagerServiceClient(binding, endpoint);
                        System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                        (se, cert, chain, sslerror) =>
                        {
                            return true;
                        };


                    }
                }
            }
            catch (FaultException<CustomFaultException> cfe)
            {
                throw cfe;
            }
            catch (FaultException fe)
            {
                throw fe;
            }
            catch (SecurityNegotiationException sne)
            {
                throw sne;
            }
            catch (CommunicationException ce)
            {
                throw ce;
            }
            catch (TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                throw ex;
            }
                return result;
        }

        public static SurveyManagerService.ManagerServiceClient GetClient()
        {
            string pEndPointAddress = ConfigurationManager.AppSettings["EndPointAddress"];
            bool pIsAuthenticated = false;
            bool pIsWsHTTPBinding = true;
            string s = ConfigurationManager.AppSettings["Authentication_Use_Windows"];
            if (!String.IsNullOrEmpty(s))
            {
                if (s.ToUpper() == "TRUE")
                {
                    pIsAuthenticated = true;
                }
            }


            s = ConfigurationManager.AppSettings["WCF_BINDING_TYPE"];
            if (!String.IsNullOrEmpty(s))
            {
                if (s.ToUpper() == "WSHTTP")
                {
                    pIsWsHTTPBinding = true;
                }
                else
                {
                    pIsWsHTTPBinding = false;
                }
            }

            return GetClient(pEndPointAddress, pIsAuthenticated, pIsWsHTTPBinding); 
        }

        public static SurveyManagerServiceV2.ManagerServiceV2Client GetClientV2()
            {
            string pEndPointAddress = ConfigurationManager.AppSettings["EndPointAddress"];
            bool pIsAuthenticated = false;
            bool pIsWsHTTPBinding = true;
            string s = ConfigurationManager.AppSettings["Authentication_Use_Windows"];
            if (!String.IsNullOrEmpty(s))
                {
                if (s.ToUpper() == "TRUE")
                    {
                    pIsAuthenticated = true;
                    }
                }


            s = ConfigurationManager.AppSettings["WCF_BINDING_TYPE"];
            if (!String.IsNullOrEmpty(s))
                {
                if (s.ToUpper() == "WSHTTP")
                    {
                    pIsWsHTTPBinding = true;
                    }
                else
                    {
                    pIsWsHTTPBinding = false;
                    }
                }

            return GetClientV2(pEndPointAddress, pIsAuthenticated, pIsWsHTTPBinding);
            }
        public static SurveyManagerServiceV2.ManagerServiceV2Client GetClientV2(string pEndPointAddress, bool pIsAuthenticated, bool pIsWsHttpBinding = true)
            {
            SurveyManagerServiceV2.ManagerServiceV2Client result = null;
            try
                {
                if (pIsAuthenticated) // Windows Authentication
                    {
                    System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();
                    binding.Name = "BasicHttpBinding";
                    binding.CloseTimeout = new TimeSpan(0, 1, 0);
                    binding.OpenTimeout = new TimeSpan(0, 1, 0);
                    binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
                    binding.SendTimeout = new TimeSpan(0, 1, 0);
                    binding.AllowCookies = false;
                    binding.BypassProxyOnLocal = false;
                    binding.HostNameComparisonMode = System.ServiceModel.HostNameComparisonMode.StrongWildcard;
                    binding.MaxBufferPoolSize = long.Parse(ConfigurationManager.AppSettings["MaxBufferPoolSize"]);//524288;
                    binding.MaxReceivedMessageSize = long.Parse(ConfigurationManager.AppSettings["MaxReceivedMessageSize"]);
                    binding.MessageEncoding = System.ServiceModel.WSMessageEncoding.Text;
                    binding.TextEncoding = System.Text.Encoding.UTF8;
                    binding.TransferMode = System.ServiceModel.TransferMode.Buffered;
                    binding.UseDefaultWebProxy = true;
                    binding.ReaderQuotas.MaxDepth = int.Parse(ConfigurationManager.AppSettings["MaxDepth"]);//32;
                    binding.ReaderQuotas.MaxStringContentLength = int.Parse(ConfigurationManager.AppSettings["MaxStringContentLength"]);  //8192;
                    binding.ReaderQuotas.MaxArrayLength = int.Parse(ConfigurationManager.AppSettings["MaxArrayLength"]); //16384;
                    binding.ReaderQuotas.MaxBytesPerRead = int.Parse(ConfigurationManager.AppSettings["MaxBytesPerRead"]); //4096;
                    binding.ReaderQuotas.MaxNameTableCharCount = int.Parse(ConfigurationManager.AppSettings["MaxNameTableCharCount"]); //16384;
                    System.ServiceModel.EndpointAddress endpoint = new System.ServiceModel.EndpointAddress(pEndPointAddress);
                    if (endpoint.Uri.Scheme == "http")
                    {
                        binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.TransportCredentialOnly;
                        binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.Windows;
                        binding.Security.Transport.ProxyCredentialType = System.ServiceModel.HttpProxyCredentialType.None;
                        binding.Security.Transport.Realm = string.Empty;
                    }
                    else
                    {
                        binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                        binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.None;


                    }

                    binding.Security.Message.ClientCredentialType = System.ServiceModel.BasicHttpMessageCredentialType.UserName;

                    result = new SurveyManagerServiceV2.ManagerServiceV2Client(binding, endpoint);
                    result.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
                    result.ChannelFactory.Credentials.Windows.ClientCredential = System.Net.CredentialCache.DefaultNetworkCredentials;
                    }
                else
                    {
                    if (pIsWsHttpBinding)
                        {
                        System.ServiceModel.WSHttpBinding binding = new System.ServiceModel.WSHttpBinding();
                        binding.Name = "WSHttpBinding";
                        binding.CloseTimeout = new TimeSpan(0, 1, 0);
                        binding.OpenTimeout = new TimeSpan(0, 1, 0);
                        binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
                        binding.SendTimeout = new TimeSpan(0, 1, 0);
                        binding.BypassProxyOnLocal = false;
                        binding.TransactionFlow = false;
                        binding.HostNameComparisonMode = System.ServiceModel.HostNameComparisonMode.StrongWildcard;
                        binding.MaxBufferPoolSize = long.Parse(ConfigurationManager.AppSettings["MaxBufferPoolSize"]);//524288;
                        binding.MaxReceivedMessageSize = long.Parse(ConfigurationManager.AppSettings["MaxReceivedMessageSize"]);
                        binding.MessageEncoding = System.ServiceModel.WSMessageEncoding.Text;
                        binding.TextEncoding = System.Text.Encoding.UTF8;
                        binding.UseDefaultWebProxy = true;
                        binding.AllowCookies = false;

                        binding.ReaderQuotas.MaxDepth = int.Parse(ConfigurationManager.AppSettings["MaxDepth"]);//32;
                        binding.ReaderQuotas.MaxStringContentLength = int.Parse(ConfigurationManager.AppSettings["MaxStringContentLength"]);  //8192;
                        binding.ReaderQuotas.MaxArrayLength = int.Parse(ConfigurationManager.AppSettings["MaxArrayLength"]); //16384;
                        binding.ReaderQuotas.MaxBytesPerRead = int.Parse(ConfigurationManager.AppSettings["MaxBytesPerRead"]); //4096;
                        binding.ReaderQuotas.MaxNameTableCharCount = int.Parse(ConfigurationManager.AppSettings["MaxNameTableCharCount"]); //16384;

                        binding.ReliableSession.Ordered = true;
                        binding.ReliableSession.InactivityTimeout = new TimeSpan(0, 10, 0);
                        binding.ReliableSession.Enabled = false;

                        System.ServiceModel.EndpointAddress endpoint = new System.ServiceModel.EndpointAddress(pEndPointAddress);
                        if (endpoint.Uri.Scheme == "http")
                        {
                            binding.Security.Mode = System.ServiceModel.SecurityMode.Message;
                            binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.Windows;
                            binding.Security.Transport.ProxyCredentialType = System.ServiceModel.HttpProxyCredentialType.None;
                            binding.Security.Transport.Realm = string.Empty;

                        }
                        else
                        {
                           // binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                          //  binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.None;


                        }


                        binding.Security.Message.ClientCredentialType = System.ServiceModel.MessageCredentialType.Windows;
                        binding.Security.Message.NegotiateServiceCredential = true;

                      
                        result = new SurveyManagerServiceV2.ManagerServiceV2Client(binding, endpoint);


                        }
                    else
                        {
                        System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();
                        binding.Name = "BasicHttpBinding";
                        binding.CloseTimeout = new TimeSpan(0, 1, 0);
                        binding.OpenTimeout = new TimeSpan(0, 1, 0);
                        binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
                        binding.SendTimeout = new TimeSpan(0, 1, 0);
                        binding.AllowCookies = false;
                        binding.BypassProxyOnLocal = false;
                        binding.HostNameComparisonMode = System.ServiceModel.HostNameComparisonMode.StrongWildcard;
                        binding.MaxBufferPoolSize = long.Parse(ConfigurationManager.AppSettings["MaxBufferPoolSize"]);//524288;
                        binding.MaxReceivedMessageSize = long.Parse(ConfigurationManager.AppSettings["MaxReceivedMessageSize"]);
                        binding.MessageEncoding = System.ServiceModel.WSMessageEncoding.Text;
                        binding.TextEncoding = System.Text.Encoding.UTF8;
                        binding.TransferMode = System.ServiceModel.TransferMode.Buffered;
                        binding.UseDefaultWebProxy = true;
                        binding.ReaderQuotas.MaxDepth = int.Parse(ConfigurationManager.AppSettings["MaxDepth"]);//32;
                        binding.ReaderQuotas.MaxStringContentLength = int.Parse(ConfigurationManager.AppSettings["MaxStringContentLength"]);  //8192;
                        binding.ReaderQuotas.MaxArrayLength = int.Parse(ConfigurationManager.AppSettings["MaxArrayLength"]); //16384;
                        binding.ReaderQuotas.MaxBytesPerRead = int.Parse(ConfigurationManager.AppSettings["MaxBytesPerRead"]); //4096;
                        binding.ReaderQuotas.MaxNameTableCharCount = int.Parse(ConfigurationManager.AppSettings["MaxNameTableCharCount"]); //16384;
                        System.ServiceModel.EndpointAddress endpoint = new System.ServiceModel.EndpointAddress(pEndPointAddress);
                        if (endpoint.Uri.Scheme == "http")
                        {
                            binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.TransportCredentialOnly;
                            binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.Windows;
                            binding.Security.Transport.ProxyCredentialType = System.ServiceModel.HttpProxyCredentialType.None;
                            binding.Security.Transport.Realm = string.Empty;
                        }
                        else
                        {
                            binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                            binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.None;


                        }
                                               
                        result = new SurveyManagerServiceV2.ManagerServiceV2Client(binding, endpoint);
                        System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                           (se, cert, chain, sslerror) =>
                           {
                               return true;
                           };
                        }
                    }
                }
            catch (FaultException<CustomFaultException> cfe)
                {
                throw cfe;
                }
            catch (FaultException fe)
                {
                throw fe;
                }
            catch (SecurityNegotiationException sne)
                {
                throw sne;
                }
            catch (CommunicationException ce)
                {
                throw ce;
                }
            catch (TimeoutException te)
                {
                throw te;
                }
            catch (Exception ex)
                {
                throw ex;
                }
            return result;
            }

        public static int GetServiceVersion() {

        string EndPointAddress = ConfigurationManager.AppSettings["EndPointAddress"];
        int ServiceVersion = 0;

        if (EndPointAddress.Contains(((Version)2).ToString()))
            {
            ServiceVersion = (int) Version.SurveyManagerServiceV2;
            }
        else 
            {

            ServiceVersion = (int)Version.SurveyManagerService;
            }
           return ServiceVersion;
            
            }
        }


}
