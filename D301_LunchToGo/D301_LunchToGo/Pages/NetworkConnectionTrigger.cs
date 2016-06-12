using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace D301_LunchToGo
{
    public static class NetworkConnectionTrigger
    {
        public static bool HasInternet()
        {
            var profile = NetworkInformation.GetInternetConnectionProfile();
            if (profile != null && profile.GetNetworkConnectivityLevel() ==
           NetworkConnectivityLevel.InternetAccess)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
