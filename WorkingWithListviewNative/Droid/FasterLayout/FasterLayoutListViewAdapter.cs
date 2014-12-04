﻿using System;
using Android.Widget;
using Android.App;
using System.Collections.Generic;
using Android.Views;
using System.Collections;
using System.Linq;
using Xamarin.Forms.Platform.Android;

namespace WorkingWithListviewPerf.Droid
{
	/// <summary>
	/// This adapter uses a view defined in /Resources/Layout/FasterLayoutListViewCell.xml
	/// as the cell layout
	/// </summary>
	public class FasterLayoutListViewAdapter: BaseAdapter<DataSource> {

		readonly Activity context;
		IList<DataSource> tableItems = new List<DataSource>();

		//public IList<string> Items {get;set;}
		public IEnumerable<DataSource> Items {
			set { 
				tableItems = value.ToList();
			}
		}

		public FasterLayoutListViewAdapter(Activity context, FasterLayoutListView view)
		{
			this.context = context;
			tableItems = view.Items.ToList();
		}
	
		public override DataSource this[int position]
		{
			get
			{ // this'll break if called with a 'header' position
				return tableItems[position];
			}
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override int Count
		{
			get { return tableItems.Count; }
		}

		/// <summary>
		/// Grouped list: view could be a 'section heading' or a 'data row'
		/// </summary>
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var item = tableItems[position];

			View view = convertView;
			if (view == null) // no view to re-use, create new
				view = context.LayoutInflater.Inflate(Resource.Layout.FasterLayoutListViewCell, null);
			view.FindViewById<TextView>(Resource.Id.Text1).Text = item.Name;
			view.FindViewById<TextView>(Resource.Id.Text2).Text = item.Category;

//			var i = view.Resources.GetIdentifier (item.ImageFilename, "drawable", context.PackageName);
//			view.FindViewById<ImageView>(Resource.Id.Image).SetImageResource(i);

			// HACK: this makes for choppy scrolling :-(
			context.Resources.GetBitmapAsync (item.ImageFilename).ContinueWith((t) => {
				view.FindViewById<ImageView> (Resource.Id.Image).SetImageBitmap (t.Result);
			});

			return view;
		}
	}
}
