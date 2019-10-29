﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Data;
using ServiceDLL.Concrete;
using ServiceDLL.Models;
using WPFAnimal.Extensions;

namespace CarShop
{
    /// <summary>
    /// Interaction logic for AddNewCarWindow.xaml
    /// </summary>
    public partial class AddNewCarWindow : Window
    {
        private ObservableCollection<FNameViewModel> FilterVM { get; set; }

        // public static List<Make> dataSource = new List<Make>();
        public AddNewCarWindow()
        {
            InitializeComponent();
            FilterVM = new ObservableCollection<FNameViewModel>();
            GetFilters();

        //SQLiteConnection con = new SQLiteConnection($"Data Source={dbName}");
        //    con.Open();
        //    string query = $"Select * from  tblCarMake";
        //    SQLiteCommand cmd = new SQLiteCommand(query, con);
        //    SQLiteDataReader reader = cmd.ExecuteReader();
        //    DataSet dataSet = new DataSet();
        //    SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(query, con);
        //    dataAdapter.Fill(dataSet);
        //    while (reader.Read())
        //    {
        //        //dataSource.Add(new Make { Name = reader["Make"].ToString(), CityID = int.Parse(reader["Id"].ToString()) });             
        //    }

            //   cbMake.ItemsSource = dataSet.Tables[0].DefaultView;
            //   cbMake.SelectedValuePath = dataSet.Tables[0].Columns["Id"].ToString();
            //   cbMake.DisplayMemberPath = dataSet.Tables[0].Columns["Make"].ToString();
            //cbMake.ItemsSource=make;           
        }
        //protected void autoCities_PatternChanged(object sender, AutoComplete.AutoCompleteArgs args)
        //{
        //    //check
        //    if (string.IsNullOrEmpty(args.Pattern))
        //        args.CancelBinding = true;
        //    else
        //        args.DataSource =GetCities(args.Pattern);
        //}
        //private static ObservableCollection<Make> GetCities(string Pattern)
        //{
        //    // match on contain (could do starts with)
        //    return new ObservableCollection<Make>(
        //        dataSource.Where((city, match) => city.Name.ToLower().Contains(Pattern.ToLower())));
        //}
      async  void GetFilters()
        {
            FilterApiService service = new FilterApiService();
            List<FNameViewModel> list = await service.GetFiltersAsync();
            FilterVM.Clear();
            FilterVM.AddRange(list);
            FillWP();



        }
        void FillWP()
        {
            wpFilters.Children.Clear();
            foreach (var item in FilterVM)
            {
                Label Name = new Label();
                Name.Content = item.Name;

                Name.VerticalAlignment = VerticalAlignment.Top;
                Name.Margin = new Thickness(25, 5, 15, 5);
                wpFilters.Children.Add(Name);
                foreach (var children in item.Children)
                {
                    CheckBox value = new CheckBox
                    {
                        Content = children.Name,
                        VerticalAlignment = VerticalAlignment.Bottom,
                        Margin = new Thickness(55, 0, 5, 5),Tag=children.Id
                    };
                    value.Click += Value_Click;
                    wpFilters.Children.Add(value);
                }
            }
        }

        private async void Value_Click(object sender, RoutedEventArgs e)
        {
            //CarApiService 
            List<CheckBox> box = new List<CheckBox>();
            foreach (var item in wpFilters.Children)
            {
                if (item.GetType() == typeof(CheckBox))

                    box.Add(item as CheckBox);
            }
            List<int> idValue = new List<int>();
            foreach (var item in box)
            {
                if (item.IsChecked == true)
                {
                    idValue.Add((int)(item.Tag));
                }
            }
            CarApiService service = new CarApiService();
          var list=  service.GetCarsByFilters(idValue.ToArray());

            int a = 0; 
            //FilterVM.Clear();
            //FilterVM.AddRange(list);
            //FillWP();
        }

        private void CbMake_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // MessageBox.Show(cbMake.SelectedValue.ToString());
        }
    }
}
