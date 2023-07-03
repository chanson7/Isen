using System.Linq;
using UnityEngine;

public static class UdpUtilities
{

    public static ushort GetFirstOpenUdpPort(int startingPort, int maxNumberOfPortsToCheck)
    {
        var range = Enumerable.Range(startingPort, maxNumberOfPortsToCheck);

        var portsInUse =
            from port in range
            join used in System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners()
            on port equals used.Port
            select port;

        var firstFreeUdpPortInRange = range.Except(portsInUse).FirstOrDefault();

        if (firstFreeUdpPortInRange > 0)
        {
            Debug.Log($"..Open port found at {firstFreeUdpPortInRange}");
            return (ushort)firstFreeUdpPortInRange;
        }
        else
        {
            Debug.LogError($"..No valid UDP ports available in the given range ({startingPort} to {startingPort + maxNumberOfPortsToCheck})");
            return 0;
        }

    }

}
