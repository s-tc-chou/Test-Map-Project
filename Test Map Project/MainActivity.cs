using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Support.V4.App;
using Android.Locations;
using System.Linq;



namespace Test_Map_Project
{
    [Activity(Label = "Test_Map_Project", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, IOnMapReadyCallback, ILocationListener
    {
        //private GoogleMap _map;
        private MapFragment _mapFragment;
        private Location _currentLocation;
        private LocationManager _locationManager;
        private string _locationProvider;

        //location listener functions

        public void OnLocationChanged(Location location)
        {
            //check if location is null
            if (location != null)
            { 
                _currentLocation = location;
            }
        }

        public void OnProviderDisabled(string provider) { }

        public void OnProviderEnabled(string provider) { }

        public void OnStatusChanged(string provider, Availability status, Bundle extras) { }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            InitMapFragment();
            InitializeLocationManager();
            //InitDefaultLocation();
        }
    

        //---------------------------

        private void InitDefaultLocation()
        {
            //setup map for getmapasync
            //if (_map != null) return;
            //_locationManager.RequestLocationUpdates(LocationManager.GpsProvider, )
            //get current location for starters. 
            //Geocoder geocoder = new Geocode(rthis);
            //geocoder.GetFromLocationAsync(_currentLocation.Latitude, _currentLocation.Longitude, 10);
            //CameraUpdate defaultLongLat = CameraUpdateFactory.NewLatLng(new LatLng(_currentLocation.Latitude, _currentLocation.Longitude));
            //CameraUpdate defaultLongLat = CameraUpdateFactory.NewLatLng(new LatLng(40.76793169992044, -73.98180484771729));
            //CameraUpdate zoomLevel = CameraUpdateFactory.ZoomTo(15);
            //can't use map: not initialized with mapfragment.  Need to get from mapfragment first. 
            //_map.MoveCamera(defaultLongLat);
            //_map.AnimateCamera(zoomLevel);

        }

        void InitializeLocationManager()
        {
            _locationManager = (LocationManager)GetSystemService(LocationService);

            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };

            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                _locationProvider = string.Empty;
            }


            //toss in a check to see if GPS enabled

            try
            {
                _locationManager.RequestLocationUpdates(LocationManager.GpsProvider, 0, 0, this);
            }
            catch
            {
            }
            try
            {
                _currentLocation = _locationManager.GetLastKnownLocation(LocationManager.GpsProvider);
            }
            catch
            {
            }

        }

        private void InitMapFragment()
        {

            _mapFragment = FragmentManager.FindFragmentByTag("map") as MapFragment;

            if (_mapFragment == null)
            {
                GoogleMapOptions mapOptions = new GoogleMapOptions()
                    .InvokeMapType(GoogleMap.MapTypeHybrid)
                    .InvokeZoomControlsEnabled(true)
                    .InvokeCompassEnabled(true);

                Android.App.FragmentTransaction fragTx = FragmentManager.BeginTransaction();
                _mapFragment = MapFragment.NewInstance(mapOptions);
                fragTx.Add(Resource.Id.map, _mapFragment, "map");
                fragTx.Commit();
            }

            //init map too. 
            _mapFragment.GetMapAsync(this);
        }

        //readies IOnMapReadyCallback
        public void OnMapReady(GoogleMap map)
        {
            map
            //CameraUpdate defaultLongLat = CameraUpdateFactory.NewLatLng(new LatLng(40.76793169992044, -73.98180484771729));
            if (_currentLocation != null)
            {
                CameraUpdate defaultLongLat = CameraUpdateFactory.NewLatLng(new LatLng(_currentLocation.Latitude, _currentLocation.Longitude));
                CameraUpdate zoomLevel = CameraUpdateFactory.ZoomTo(15);

                map.MoveCamera(defaultLongLat);
                map.AnimateCamera(zoomLevel);

                //add marker, not sure if works. 
                map.AddMarker(new MarkerOptions().SetPosition(new LatLng(_currentLocation.Latitude, _currentLocation.Longitude)).SetTitle("Current Location"));
            }
        }

    }
}

