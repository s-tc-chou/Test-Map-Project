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

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            //unknown identifier resource, container is null. 
            var view = inflater.Inflate(Resource.Layout.UILongPressMenu, container, true);


            //button1 = view.FindViewById<ImageButton>(Resource.Id.imageButton1);
            //button1.Click += Button_Dismiss_Click;


            return base.OnCreateView(inflater, container, savedInstanceState);
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


        private void Button_Dismiss_Click(object sender, EventArgs e)
        {
            Dismiss();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            // Unwire event
            if (disposing)
                button1.Click -= Button_Dismiss_Click;
        }

    }
}