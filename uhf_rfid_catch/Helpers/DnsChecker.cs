using System;
using System.Net.NetworkInformation;

namespace uhf_rfid_catch.Helpers
{
    public class DnsChecker
    {
        private static bool _finalStatus;

        readonly Ping _pingInit = new Ping();
        private readonly byte[] _buffer = new byte[32];
        private readonly int _timeout = 1000;
        readonly PingOptions _pingOptions = new PingOptions();
        private static readonly ConfigContext SettingsContext = new ConfigContext();
        readonly string _host = SettingsContext.Resolve("InternetCheckAddress");
        
        public DnsChecker()
        {
//            Console.WriteLine(_host);
            try
            {
                PingReply receivedPingReply = _pingInit.Send(_host, _timeout, _buffer, _pingOptions);
                if (receivedPingReply != null) _finalStatus = receivedPingReply.Status == IPStatus.Success;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool Status()
        {
            return _finalStatus;
        }
    }
}