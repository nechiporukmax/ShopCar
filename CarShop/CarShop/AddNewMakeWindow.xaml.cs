﻿using CarShop.Entities;
using CarShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFAnimal.Extensions;

namespace CarShop
{
    /// <summary>
    /// Interaction logic for AddNewMakeWindow.xaml
    /// </summary>
    public partial class AddNewMakeWindow : Window
    {      
        private ObservableCollection<MakeViewModels> MakeVM { get; set; }
        private readonly EFcontext _context;

        public AddNewMakeWindow()
        {
            InitializeComponent();
            _context = new EFcontext();
            MakeVM = new ObservableCollection<MakeViewModels>();
            DBGrid.ItemsSource = MakeVM;
            FillDB();
        }
        void FillDB()
        {
            try {
                //DBGrid.Items.Clear();
                var query = _context.Makes.AsQueryable();
                var list = query.Select(at => new MakeViewModels
                {
                    Id = at.Id,
                    Name = at.Name
                }).ToList();
                MakeVM.Clear();
                MakeVM.AddRange(list);
                DBGrid.ItemsSource = MakeVM;
            }
            catch { }
       }
        private void BtnAddMake_Click(object sender, RoutedEventArgs e)
        {
            _context.Makes.Add(new Entities.Make { Name = txtMake.Text });
            _context. SaveChanges();
            FillDB();
        }

        private void DBGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
