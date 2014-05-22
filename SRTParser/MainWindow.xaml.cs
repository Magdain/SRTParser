using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SRTParser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            subtitleList.Columns.Add(new DataGridTextColumn() { Header = "Start",       Binding = new Binding("Start") });
            subtitleList.Columns.Add(new DataGridTextColumn() { Header = "End",         Binding = new Binding("End") });
            //subtitleList.Columns.Add(new DataGridTextColumn() { Header = "Duration",    Binding = new Binding("Duration") });
            subtitleList.Columns.Add(new DataGridTextColumn() { Header = "Content",     Binding = new Binding("Content") });

            var subtitles = Subtitle.Parse("../../test.srt");
            for (var i = 0; i < subtitles.Count; ++i)
            {
                subtitleList.Items.Add((Subtitle)subtitles[i]);
            }
        }
    }
}
