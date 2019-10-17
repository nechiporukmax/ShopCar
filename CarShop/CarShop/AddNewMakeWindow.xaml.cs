﻿using CarShop.Entities;
using CarShop.ViewModels;
using ServiceDLL.Concrete;
using ServiceDLL.Models;
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
        private ObservableCollection<MakeVM> MakeVM { get; set; }

        public AddNewMakeWindow()
        {
            InitializeComponent();
            MakeVM = new ObservableCollection<MakeVM>();
            DBGrid.ItemsSource = MakeVM;
            FillDB();
        }
        int pageNumber = 1;
        async void FillDB()
        {
            MakeApiService service = new MakeApiService();
            List<MakeVM> list = await service.GetMakesAsync();
            MakeVM.Clear();
            MakeVM.AddRange(list);

            var count = MakeVM.Count();
            int numberOfObjectsPerPage = 5;//int.Parse(cbCount.Text);
            int pages = (int)Math.Ceiling((double)count / (double)numberOfObjectsPerPage);
            GenerationBtn(pages, pageNumber);

            int begin = (pageNumber - 1) * numberOfObjectsPerPage;
            int end = pageNumber * numberOfObjectsPerPage;
            DBGrid.ItemsSource = MakeVM.Skip(numberOfObjectsPerPage * (pageNumber - 1))
                        .Take(numberOfObjectsPerPage);

        }
        private void BtnAddMake_Click(object sender, RoutedEventArgs e)
        {

            MakeApiService service = new MakeApiService();
            string str=  service.Create(new MakeAddModel { Name = txtMake.Text });
            str= str.Trim('"');
            MessageBox.Show(str);
            FillDB();
        }
        void GenerationBtn(int pages, int pageNumber)
        {
            #region Побудова кнопок           
            var sizeButton = new Size { Width = 50, Height = 30 };
            int count_b = 0;
            wpBTN.Children.Clear();
            if (pageNumber <= 5)
            {
                for (int i = 0; i < pages; i++)
                {
                    if (count_b < 7)
                    {

                        var b = new Button()
                        {
                            Content = $"{i + 1}",
                            Width = sizeButton.Width,
                            Height = sizeButton.Height,
                            Margin = new Thickness(5, 5, 5, 5),
                            Background = i == pageNumber - 1 ? Brushes.Green : Brushes.White
                        };
                        b.Click += B_Click; ;
                        wpBTN.Children.Add(b);
                        count_b++;

                    }
                    else { break; }

                }
            }
           
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    var b = new Button()
                    {
                        Content = $"{i + 1}",
                        Width = sizeButton.Width,
                        Height = sizeButton.Height,
                        Margin = new Thickness(5, 5, 5, 5),
                        Background = i == pageNumber - 1 ? Brushes.Green : Brushes.White
                    };
                    b.Click += B_Click; ;
                    wpBTN
                    .Children
                    .Add(b);
                }

                var l = new Label()
                {
                    Content = $"...",
                    Width = sizeButton.Width,
                    Height = sizeButton.Height,
                    Margin = new Thickness(5, 5, 5, 5),


                };
                wpBTN
               .Children
               .Add(l);

                for (int i = pageNumber - 4; i < pages; i++)
                {
                    if (count_b < 7)
                    {
                        var b = new Button()
                        {
                            Content = $"{i + 1}",
                            Width = sizeButton.Width,
                            Height = sizeButton.Height,
                            Margin = new Thickness(5, 5, 5, 5),
                            Background = i == pageNumber - 1 ? Brushes.Green : Brushes.White
                        };
                        b.Click += B_Click; ;
                        wpBTN.Children.Add(b);


                        count_b++;
                    }
                    else { break; }

                }

            }
            if ((pageNumber + 4) < pages)
            {

                var l = new Label()
                {
                    Content = $"...",
                    Width = sizeButton.Width,
                    Height = sizeButton.Height,
                    Margin = new Thickness(5, 5, 5, 5),
                };
                wpBTN
               .Children
               .Add(l);
            }
            if ((pageNumber + 4) < pages)
            {
                var b = new Button()
                {
                    Content = $"{pages}",
                    Width = sizeButton.Width,
                    Height = sizeButton.Height,
                    Margin = new Thickness(5, 5, 5, 5),
                    Background = pageNumber == pageNumber - 1 ? Brushes.Green : Brushes.White
                };
                b.Click += B_Click; ;
                wpBTN
             .Children
             .Add(b);
            }
            #endregion
        }

   

        private void B_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            pageNumber = Convert.ToInt32(btn.Content);
            FillDB();
        }

     

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (DBGrid.SelectedItem != null)
                {
                    int ind = 0;
                    ind = MakeVM.IndexOf(DBGrid.SelectedItem as MakeVM);
                    MakeApiService service = new MakeApiService();
                    int f = MakeVM[ind].Id;
                    service.Delete(new MakelDeleteVM { Id = f });
                    ////     //DataGrid(DBGrid.SelectedItem as MakeViewModels);
                    //var itemToRemove = _context.Makes.Where(x => x.Id == f).ToList(); //returns a single item.

                    //if (itemToRemove != null)
                    //{
                    //    _context.Makes.Remove(itemToRemove[0]);
                    //    _context.SaveChanges();
                    //}


                    //_context.SaveChanges();
                    FillDB();

                }
            }
            catch
            { }
            }
        }
    }

