﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using MasterReloBot.Distance;

namespace MasterReloBot
{
    //Methods that will retrieve the correct commute info.
    public class CommuteUtilities
    {
        //Get Commute Time for Cars
        public static async Task<string> GetCommuteTime(string origin, string destination)
        {
            string strRet = string.Empty;
            DistanceInfo distanceInfo = await GoogleAPIs.GetDistanceInfoAsync(origin, destination);
            if (null == distanceInfo)
            {
                strRet = string.Format("Sorry, I could not get the commute time for {0} to {1}", origin, destination);
            }
            else
            {
                strRet = string.Format("It will take about {0}", distanceInfo.rows[0].elements[0].duration.text);
            }
            return strRet;
        }

        //Get Commute Distance for Cars
        public static async Task<string> GetDistance(string origin, string destination)
        {
            string strRet = string.Empty;
            DistanceInfo distanceInfo = await GoogleAPIs.GetDistanceInfoAsync(origin, destination);
            if (null == distanceInfo)
            {
                strRet = string.Format("Sorry, I could not get the distance for {0} to {1}", origin, destination);
            }
            else
            {
                strRet = string.Format("It will take about {0}", distanceInfo.rows[0].elements[0].distance.text);
            }
            return strRet;
        }
    }
}

//Intents
//GetCommuteTime
//GetDistance
//GetTransportation
//GetAddress

//Entities
//Location
//Buses