using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
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

using Test_Map_Project;



namespace Test_Map_Project
{
    [Activity(Label = "Test_Map_Project", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, IOnMapReadyCallback, ILocationListener, GoogleMap.IOnMapLongClickListener//, GoogleMap.IOnMapClickListener
    {
        private GoogleMap _map;
        private MapFragment _mapFragment;
        private Location _currentLocation;
        private LocationManager _locationManager;
        private string _locationProvider;
        private initializers _initializerBundle = new initializers();         //toss initializers into a bundle.
        private ArrayList _markerList = new ArrayList();

        //location listener functions

        public void OnLocationChanged(Location location)
        {
            //check if location is null
            if (location != null)
            {
                if (!_initializerBundle.getIsLocationInitialized())
                {
                    _initializerBundle.setIsLocationInitialized(MapHelper.setInitialMapLocation(_currentLocation, _map, this));
                }
                _currentLocation = location;
            }
        }

        //IOnMapLongClickListener implementation
        public void OnMapLongClick(LatLng point)
        {
            //update touch location
            _currentLocation.Longitude = point.Longitude;
            _currentLocation.Latitude = point.Latitude;
            //save markers (use text file example)
            if (_map != null)
            {
                //     Console.WriteLine("long clicky");
                Marker touchPoint = _map.AddMarker(new MarkerOptions()
                                .SetPosition(new LatLng(_currentLocation.Latitude, _currentLocation.Longitude))
                                .SetTitle("long click"));
                //note: initial marker will not be saved. Need to figure out how to access global vars here from another class. 
                _markerList.Add(touchPoint);
            }
        }


        public void OnProviderDisabled(string provider) { }

        public void OnProviderEnabled(string provider) { }

        public void OnStatusChanged(string provider, Availability status, Bundle extras) { }

        protected override void OnPause()
        {
            //   Console.WriteLine("on pause is called");

            base.OnPause();

            var path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);//System.Environment.GetFolderPath(Android.OS.Environment.GetExternalStoragePublicDirectory());
            string filename = path.AbsolutePath + "locations.txt";

            //write to file
            foreach (Marker curPoint in _markerList)
            {
                //if initial run, don't append.  else append. 
                using (var streamWriter = new StreamWriter(filename, _initializerBundle.getIsMapFileInitialized())) //don't append for now. 
                {
                    LatLng markerPos = curPoint.Position;
                    //ideally: back up all the information about the marker.  For now I want just long lat. 
                    streamWriter.WriteLine(markerPos.Latitude + "," + markerPos.Longitude + "," + curPoint.Title);
                    Console.WriteLine(markerPos.Latitude + "," + markerPos.Longitude + "," + curPoint.Title);
                }
            }
            if (!_initializerBundle.getIsMapFileInitialized())
            {
                _initializerBundle.setisMapFileInitialized(true);
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            var path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);//System.Environment.GetFolderPath(Android.OS.Environment.GetExternalStoragePublicDirectory());
            string filename = path.AbsolutePath + "locations.txt";
            string line;
            string[] parsedLine;
            LatLng markerPos;


            if (File.Exists(filename))
            {
                using (var myReader = new StreamReader(filename, true))
                {
                    while ((line = myReader.ReadLine()) != null)
                    {
                        parsedLine = line.Split(',');
                        markerPos = new LatLng(Double.Parse(parsedLine[0]), Double.Parse(parsedLine[1]));
                        Console.WriteLine(markerPos.Latitude + "," + markerPos.Longitude + "," + parsedLine[2]);
                    }

                }
            }
            else
            {
                Console.WriteLine("file doesn't exist");
            }

        }

        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            InitMapFragment();
            InitializeLocationManager();
        }

        //readies IOnMapReadyCallback
        public void OnMapReady(GoogleMap map)
        {
            _map = map;
            if (!_initializerBundle.getIsLocationInitialized())
            {
                _initializerBundle.setIsLocationInitialized(MapHelper.setInitialMapLocation(_currentLocation, _map, this));
                _map.SetOnMapLongClickListener(this);
            }
        }




        //helper functions
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

            Boolean isGPSEnabled = _locationManager.IsProviderEnabled(LocationManager.GpsProvider);
            if (isGPSEnabled)
            {
                if (_currentLocation == null)
                {
                    Console.WriteLine("GPS enabled, but current location null.");
                    try
                    {
                        _locationManager.RequestLocationUpdates(LocationManager.GpsProvider, 0, 0, this);
                    }
                    catch
                    {
                    }
                    if (_locationManager != null)
                    {
                        Console.WriteLine("GPS enabled, current location null, location manager not null");
                        try
                        {
                            _currentLocation = _locationManager.GetLastKnownLocation(LocationManager.GpsProvider);
                        }
                        catch
                        {
                        }

                        if (_currentLocation != null)
                        {
                            OnLocationChanged(_currentLocation); // Invokes the onLocationChanged method.
                        }
                        else {
                            Console.WriteLine("retrieveCoordinates(): Location retrieval is not ready.");
                        }
                    }
                }
            }

        }

        private void InitMapFragment()
        {

            _mapFragment = FragmentManager.FindFragmentByTag("map") as MapFragment;

            if (_mapFragment == null)
            {
                GoogleMapOptions mapOptions = new GoogleMapOptions()
                    .InvokeMapType(GoogleMap.MapTypeNormal)
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

        //bundled initializers
        private class initializers
        {
            private bool isLocationInitialized = false;
            private bool isMapFileInitialized = false;

            public bool getIsLocationInitialized()
            {
                return isLocationInitialized;
            }

            public void setIsLocationInitialized(bool setter)
            {
                isLocationInitialized = setter;
            }

            public bool getIsMapFileInitialized()
            {
                return isMapFileInitialized;
            }

            public void setisMapFileInitialized(bool setter)
            {
                isMapFileInitialized = setter;
            }


        }


    }

    
}

