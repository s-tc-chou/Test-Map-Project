using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics.Drawables;
using Android.Graphics;


namespace Test_Map_Project.LongPressMenuFragmentUI
{
    class LongPressMenuFragment : Android.Support.V4.App.DialogFragment
    {
        ImageButton button1;
        ImageButton button2;
        ImageButton button3;
        ImageButton button4;
        ImageButton button5;
        ImageButton button6;
        int _buttonPressed = 0;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            //Dialog.RequestWindowFeature(WindowFeatures.NoTitle);
            var view = inflater.Inflate(Resource.Layout.UILongPressMenu, container, true);

            //button1.Click += Button_Dismiss_Click;
            button1Listener(view);
            button2Listener(view);
            button3Listener(view);
            button4Listener(view);
            button5Listener(view);
            button6Listener(view);

            //this.Windows.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
            return view;
        }


        /*public override void OnResume()
        {
            // Auto size the dialog based on it's contents
            Dialog.Window.SetLayout(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);

            // Make sure there is no background behind our view
            Dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));

            // Disable standard dialog styling/frame/theme: our custom view should create full UI
            //SetStyle(Android.Support.V4.App.DialogFragment.StyleNoFrame, Android.Resource.Style.Theme);

            base.OnResume();
        }*/


        /*--------
        Some notes for myself:
        OnClick Listener is more for java implementations.  C#/xamarin uses the delegate function to simulate the on click override.  
        While we can still implement the onclicklistener, its cleaner in xamarin to do it this way and it has better performance/memory usage.  
        --------*/
        private void button1Listener(View menuView)
        {
            button1 = menuView.FindViewById<ImageButton>(Resource.Id.imageButton1);

            button1.Click += delegate {
                _buttonPressed = 1;
            };

            button1.Click += delegate {
                Console.WriteLine("button1 clicked, variable " + _buttonPressed);
            };


            //(_buttonPressed);
            MainActivity myActivity = (MainActivity) Activity;
            myActivity.setMarker(_buttonPressed);

        }
        private void button2Listener(View menuView)
        {
            button2 = menuView.FindViewById<ImageButton>(Resource.Id.imageButton2);
            button2.Click += delegate {
                Console.WriteLine("button2 clicked, variable " + _buttonPressed);
                _buttonPressed = 2;
            };


        }
        private void button3Listener(View menuView)
        {
            button3 = menuView.FindViewById<ImageButton>(Resource.Id.imageButton3);
            button3.Click += delegate {
                Console.WriteLine("button3 clicked, variable " + _buttonPressed);
                _buttonPressed = 3;
            };

        }
        private void button4Listener(View menuView)
        {
            button4 = menuView.FindViewById<ImageButton>(Resource.Id.imageButton4);
            button4.Click += delegate {
                Console.WriteLine("button4 clicked, variable " + _buttonPressed);
                _buttonPressed = 4;
            };

        }
        private void button5Listener(View menuView)
        {
            button5 = menuView.FindViewById<ImageButton>(Resource.Id.imageButton5);
            button5.Click += delegate {
                Console.WriteLine("button5 clicked , variable " + _buttonPressed);
                _buttonPressed = 5;
            };

        }
        private void button6Listener(View menuView)
        {
            button6 = menuView.FindViewById<ImageButton>(Resource.Id.imageButton6);
            button6.Click += delegate {
                Console.WriteLine("button6 clicked, variable " + _buttonPressed);
                _buttonPressed = 6;
            };

        }

        public int getButtonPressed()
        {
            return _buttonPressed;
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

    }
}