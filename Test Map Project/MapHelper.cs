using System;
using System.Collections.Generic;
using System.Linq;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.Support.V4.App;
using Android.Content;

namespace Test_Map_Project
{
    class MapHelper
    {
        public static bool setInitialMapLocation(Location currentLocation, GoogleMap map, GoogleMap.IOnMapLongClickListener longClickListener)
        {
            //check if location is ready yet. 
            if (currentLocation != null)
            {
                CameraUpdate defaultLongLat = CameraUpdateFactory.NewLatLng(new LatLng(currentLocation.Latitude, currentLocation.Longitude));
                CameraUpdate zoomLevel = CameraUpdateFactory.ZoomTo(15);

                //is map ready?
                if (map != null)
                {
                    map.MoveCamera(defaultLongLat);
                    map.AnimateCamera(zoomLevel);
                    map.AddMarker(new MarkerOptions().SetPosition(new LatLng(currentLocation.Latitude, currentLocation.Longitude)).SetTitle("Steve's starting Location"));
                    map.SetOnMapLongClickListener(longClickListener);
                    return true;
                }
            }
            //correct path not hit, returning false.  

            return false;
        }

    }

    
}