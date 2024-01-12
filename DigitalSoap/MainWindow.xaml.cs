using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
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
using DigitalClean;
using System.Windows.Media.Animation;
using System.IO;
using System.Collections;
using Microsoft.Win32;

namespace DigitalSoap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private char ConstNullChar = '\0';
        private double SpinnerAngle = 0;

        private string ProgramVersion = "0.1.1a";

        private BitmapImage CheckMarkCompleteImg = new BitmapImage();

        private bool CustomFolderIntelligentName = false;

        List<string> FolderPaths = new List<string>()
        {
            DigitalClean.Folders.USER_TEMPORARY
        };

        OpenFolderDialog FolderDialog = new OpenFolderDialog();

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                Core.Initialize();
                DigitalClean.DigitalClean.Initialize();
                
                //SET UI TO DEFAULT STATE
                CustomFolderListBox.Visibility = Visibility.Collapsed;

                //UPDATE UI WITH INITIALIZED SETTINGS
                UpdateCustomFolderListBox();

                this.Opacity = 0.0f;
                DoubleAnimation FadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5f));
                this.BeginAnimation(Window.OpacityProperty, FadeIn);

                MainTabControl.Height = this.Height - 112;
                RenderOptions.SetBitmapScalingMode(CustomCleanTabImg, BitmapScalingMode.NearestNeighbor);
                RenderOptions.SetBitmapScalingMode(PerformanceTabImg, BitmapScalingMode.NearestNeighbor);
                RenderOptions.SetBitmapScalingMode(StorageTabImg, BitmapScalingMode.NearestNeighbor);
                RenderOptions.SetBitmapScalingMode(ToolsTabImg, BitmapScalingMode.NearestNeighbor);
                RenderOptions.SetBitmapScalingMode(SystemTabImg, BitmapScalingMode.NearestNeighbor);
                RenderOptions.SetBitmapScalingMode(OptionsTabImg, BitmapScalingMode.NearestNeighbor);
                RenderOptions.SetBitmapScalingMode(CleanPreviewImg, BitmapScalingMode.NearestNeighbor);
                RenderOptions.SetBitmapScalingMode(CleanCompleteImg, BitmapScalingMode.Linear);

#if DEBUG
            VersionLabel.Content = "Debug v" + ProgramVersion;
#else
                VersionLabel.Content = "Release v" + ProgramVersion;
#endif

                SystemInfoLabel.Text = SystemInfo.GetSystemInfoString();
                SystemInfoLabel.TextAlignment = TextAlignment.Right;

                CheckMarkCompleteImg.BeginInit();
                CheckMarkCompleteImg.UriSource =
                    new Uri("pack://application:,,,/DigitalSoap;component/Resources/Images/Check_Good.png");
                CheckMarkCompleteImg.EndInit();

                FolderDialog.RootDirectory = "C:\\";
                FolderDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                FolderDialog.ValidateNames = true;
                FolderDialog.Multiselect = false;
                FolderDialog.ShowHiddenItems = true;

                HideCleanControls();
            }
            catch (Exception Ex)
            {
                string LogFileName = "ErrorLog_"
                    + DateTime.Now.Year.ToString()
                    + DateTime.Now.Month.ToString()
                    + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString()
                    + DateTime.Now.Second.ToString() + ".txt";
                using (StreamWriter Writer = new StreamWriter(LogFileName))
                {
                    Writer.WriteLine("A generic error has occurred during the initialization of the program.");
                    Writer.WriteLine("The specific details are as follow:\n");
                    Writer.WriteLine("Message: " + Ex.Message);
                    Writer.WriteLine("Inner Exception: " + Ex.InnerException);
                    Writer.WriteLine("Data\n{");
                    foreach (IDictionaryEnumerator Data in Ex.Data)
                    {
                        Writer.WriteLine("\t" + Data.Key + " = " + Data.Value);
                    }
                    Writer.WriteLine("}");
                }

                MessageBoxResult ErrorResult = MessageBox.Show("An error has occurred that has caused the program to fail during initialization." +
                    "Please refer to the log file:\n" + LogFileName + "\n\n" +
                    "Would you like to continue running the program anyway?", "Error", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (ErrorResult == MessageBoxResult.No)
                {
                    Environment.Exit(-1);
                }
            }
        }

        private void Main_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                MainTabControl.Height = this.Height - 128;
            }
            else
            {
                MainTabControl.Height = this.Height - 126;
            }
        }

        private void Main_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DigitalClean.DigitalClean.Status == State.SCANNING ||
                DigitalClean.DigitalClean.Status == State.CLEANING)
            {
                MessageBox.Show("Please wait for the current scan to complete before closing the" +
                    " application to prevent I/O errors. If the program is stuck during a scan," +
                    " it must be terminated manually through the control panel to release" +
                    " all open I/O streams.", "Warning", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                e.Cancel = true;
            }
        }

        private void WinExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WinMaxButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ?
                WindowState.Normal : WindowState.Maximized;
        }

        private void WinMinButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void TitleGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void GitHubLabel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/STOL4S") { UseShellExecute = true });
        }

        private void CleanCleanButton_Click(object sender, RoutedEventArgs e)
        {
            CleanFunction(true);
        }

        private void CleanFunction(bool EraseFiles)
        {
            ShowCleanControls();

            FolderPaths.Clear();
            FolderPaths = ConstructFolderList();

            Thread T = new Thread(() =>
            {
                FunctionTimer FT = new FunctionTimer();
                FT.Start();
                if (FolderPaths.Count > 0)
                {
                    //CLEAR PREVIOUS DATA
                    DigitalClean.DigitalClean.ScanData.Clear();
                    DigitalClean.DigitalClean.Status = State.SCANNING;

                    //CLEAR PREVIOUS CONTROLS
                    CleanDetailsLabel.Dispatcher.Invoke(() =>
                    {
                        CleanDetailsLabel.BeginAnimation(Label.OpacityProperty, null);
                        CleanSpinnerImg.BeginAnimation(Image.OpacityProperty, null);
                        CleanCompleteImg.BeginAnimation(Image.OpacityProperty, null);
                        CleanSep1.BeginAnimation(Separator.OpacityProperty, null);
                        CleanSep2.BeginAnimation(Separator.OpacityProperty, null);

                        CleanCompleteImg.Visibility = Visibility.Visible;
                        CleanCompleteImg.Opacity = 0;

                        CleanDetailsLabel.Content = "";
                        CleanSep1.Opacity = 0;
                        CleanSep2.Opacity = 0;
                        CleanDetailsLabel.Opacity = 0;
                        CleanDescriptionLabel.Opacity = 0.0f;
                        CleanNoteLabel.Opacity = 0.0f;
                        CleanStatusLabel.Opacity = 0.0f;
                        CleanSpinnerImg.Opacity = 0.0f;

                        DoubleAnimation OpacSz = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5f));
                        CleanDescriptionLabel.BeginAnimation(Label.OpacityProperty, OpacSz);
                        CleanNoteLabel.BeginAnimation(Label.OpacityProperty, OpacSz);
                        CleanStatusLabel.BeginAnimation(Label.OpacityProperty, OpacSz);
                        CleanSpinnerImg.BeginAnimation(Image.OpacityProperty, OpacSz);
                    });

                    //CALCULATE PROGRESS DELTA
                    float ProgressDelta = 100.0f / (float)FolderPaths.Count;
                    float TotalProgress = 0.0f;

                    //FOR EVERY FOLDER PATH, ADD DIRECTORY TO SCAN DATA
                    //AND CALCULATE DIRECTORY SIZE
                    int ScanCount = 0;
                    float ScanSize = 0;
                    foreach (string Path in FolderPaths)
                    {
                        string ScanParent = DigitalClean.Folders.ParentNameFromPath(Path);
                        string ScanCategory = DigitalClean.Folders.NodeNameFromPath(Path);

                        CleanStatusLabel.Dispatcher.Invoke(() =>
                        {
                            CleanStatusLabel.Content = ScanParent;
                        });

                        CleanDescriptionLabel.Dispatcher.Invoke(() =>
                        {
                            CleanDescriptionLabel.Content = ScanCategory;
                        });

                        CleanNoteLabel.Dispatcher.Invoke(() =>
                        {
                            CleanNoteLabel.Content = Path;
                        });

                        PointF DirSize = DigitalClean.DigitalClean.CalculateDirectorySize(Path);

                        //CHECK FOR ACCESS VIOLATION ERRORS
                        if (DirSize.Y != -1)
                        {
                            DigitalClean.DigitalClean.ScanData.Add(ScanParent + " - " + ScanCategory + " ("
                                + DirSize.Y.ToString() + " Files)", DirSize.X.ToString());
                            ScanCount += (int)DirSize.Y;
                            ScanSize += (float)DirSize.X;

                            if (EraseFiles)
                            {
                                string[] DirFiles = Directory.GetFiles(Path);
                                foreach (string DirFile in DirFiles)
                                {
                                    File.Delete(DirFile);
                                }
                            }
                        }

                        CleanProgressBar.Dispatcher.Invoke(() =>
                        {
                            //CleanProgressBar.Value += ProgressDelta;
                            TotalProgress += ProgressDelta;
                            DoubleAnimation AnimProg = new DoubleAnimation(TotalProgress, TimeSpan.FromSeconds(0.2f));
                            CleanProgressBar.BeginAnimation(ProgressBar.ValueProperty, AnimProg, HandoffBehavior.Compose);
                        });
                    }

                    FT.Stop();

                    //SCAN COMPLETE
                    //PRESENT DATA
                    this.Dispatcher.Invoke(() =>
                    {
                        DoubleAnimation OpacSz = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(2f));
                        OpacSz.BeginTime = TimeSpan.FromSeconds(0.5f);
                        DoubleAnimation OpacOut = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.5f));

                        StringAnimationUsingKeyFrames NoteAnim = new StringAnimationUsingKeyFrames();
                        NoteAnim.KeyFrames = new StringKeyFrameCollection()
                            {
                                new DiscreteStringKeyFrame(EraseFiles ? "Files have been removed" 
                                : "No files have been removed", KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.5f)))
                            };
                        StringAnimationUsingKeyFrames DescAnim = new StringAnimationUsingKeyFrames();
                        DescAnim.KeyFrames = new StringKeyFrameCollection()
                            {
                                new DiscreteStringKeyFrame("Elapsed Time: " + FT.ToSeconds() + " seconds", 
                                KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.5f)))
                            };
                        StringAnimationUsingKeyFrames StatusAnim = new StringAnimationUsingKeyFrames();
                        StatusAnim.KeyFrames = new StringKeyFrameCollection()
                            {
                                new DiscreteStringKeyFrame(EraseFiles ? "Clean Complete - "
                                    + (ScanSize / 1024.0f).ToString("N2") + " MB Cleaned" : "Scan Complete - "
                                    + (ScanSize / 1024.0f).ToString("N2") + " MB Scanned", 
                                    KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.5f)))
                            };

                        CleanStatusLabel.BeginAnimation(Label.OpacityProperty, OpacOut, HandoffBehavior.Compose);
                        CleanDescriptionLabel.BeginAnimation(Label.OpacityProperty, OpacOut, HandoffBehavior.Compose);
                        CleanNoteLabel.BeginAnimation(Label.OpacityProperty, OpacOut, HandoffBehavior.Compose);
                        CleanSpinnerImg.BeginAnimation(Image.OpacityProperty, OpacOut, HandoffBehavior.Compose);

                        CleanNoteLabel.BeginAnimation(Label.ContentProperty, NoteAnim, HandoffBehavior.Compose);
                        CleanDescriptionLabel.BeginAnimation(Label.ContentProperty, DescAnim, HandoffBehavior.Compose);
                        CleanStatusLabel.BeginAnimation(Label.ContentProperty, StatusAnim, HandoffBehavior.Compose);

                        CleanStatusLabel.BeginAnimation(Label.OpacityProperty, OpacSz, HandoffBehavior.Compose);
                        CleanDescriptionLabel.BeginAnimation(Label.OpacityProperty, OpacSz, HandoffBehavior.Compose);
                        CleanNoteLabel.BeginAnimation(Label.OpacityProperty, OpacSz, HandoffBehavior.Compose);
                        CleanCompleteImg.BeginAnimation(Image.OpacityProperty, OpacSz, HandoffBehavior.Compose);

                        CleanDetailsLabel.Visibility = Visibility.Visible;
                        CleanSep1.Visibility = Visibility.Visible;
                        CleanSep2.Visibility = Visibility.Visible;
                        CleanSep1.Opacity = 0;
                        CleanSep2.Opacity = 0;
                        CleanDetailsLabel.Opacity = 0;

                        CleanSep1.BeginAnimation(Separator.OpacityProperty, OpacSz);
                        CleanSep2.BeginAnimation(Separator.OpacityProperty, OpacSz);
                        CleanDetailsLabel.Content = EraseFiles ? "Clean Details - " + ScanCount.ToString()
                            + " Files Cleaned" : "Scan Details - " 
                            + ScanCount.ToString() + " Files Scanned";
                        CleanDetailsLabel.BeginAnimation(Label.OpacityProperty, OpacSz);

                        DoubleAnimation AnimProg = new DoubleAnimation(0, TimeSpan.FromSeconds(1.6f));
                        AnimProg.BeginTime = TimeSpan.FromSeconds(0.5f);
                        DoubleAnimation DelayProg = new DoubleAnimation(100, TimeSpan.FromSeconds(0.1f));

                        CleanProgressBar.BeginAnimation(ProgressBar.ValueProperty, DelayProg);
                        CleanProgressBar.BeginAnimation(ProgressBar.ValueProperty, AnimProg, HandoffBehavior.Compose);
                    });

                    for (int i = 0; i < DigitalClean.DigitalClean.ScanData.Count; i++)
                    {

                        CleanResultPanel.Dispatcher.Invoke(() =>
                        {
                            BitmapImage WindowsExplorerLogo = new BitmapImage();
                            WindowsExplorerLogo.BeginInit();
                            switch (DigitalClean.DigitalClean.ScanData.ToArray()[i].Key)
                            {
                                case var A when A.StartsWith("Windows"):
                                    WindowsExplorerLogo.UriSource =
                                        new Uri("pack://application:,,,/DigitalSoap;component/Resources/Images/FolderClosed.png");
                                    break;

                                case var A when A.StartsWith("System"):
                                    WindowsExplorerLogo.UriSource =
                                        new Uri("pack://application:,,,/DigitalSoap;component/Resources/Images/SystemInfo_Dark.png");
                                    break;

                                case var A when A.Contains("Internet"):
                                    WindowsExplorerLogo.UriSource =
                                        new Uri("pack://application:,,,/DigitalSoap;component/Resources/Images/WebAPI.png");
                                    break;
                            }
                            WindowsExplorerLogo.EndInit();

                            Image _Img = new Image();
                            _Img.Width = 16;
                            _Img.Height = 16;
                            _Img.Source = WindowsExplorerLogo;
                            _Img.VerticalAlignment = VerticalAlignment.Top;
                            _Img.Margin = new Thickness(11, (20 * i) + 9, 5, 0);

                            Label _Label = new Label();
                            _Label.Margin = new Thickness(-2, (20 * i) + 3, 5, 0);
                            _Label.Content = DigitalClean.DigitalClean.ScanData.ToArray()[i].Key;
                            _Label.Foreground = (System.Windows.Media.Brush)Application.Current.Resources["Dark_TextBrush"];
                            _Label.HorizontalAlignment = HorizontalAlignment.Stretch;

                            Label _Sz = new Label();
                            _Sz.Margin = new Thickness(5, (20 * i) + 3, 29, 0);
                            string FormattedSize = DigitalClean.DigitalClean.ScanData.ToArray()[i].Value.ToString();
                            FormattedSize = (float.Parse(FormattedSize) * 1.0f).ToString("N2");
                            bool IsMB = false;
                            if (float.Parse(FormattedSize) > 1024.0f)
                            {
                                float I = float.Parse(FormattedSize);
                                FormattedSize = (I / 1024.0f).ToString("N2");
                                IsMB = true;
                            }

                            _Sz.Content = FormattedSize;
                            _Sz.FontWeight = FontWeights.Bold;
                            _Sz.HorizontalAlignment = HorizontalAlignment.Right;
                            _Sz.HorizontalContentAlignment = HorizontalAlignment.Right;
                            _Sz.Foreground = (System.Windows.Media.Brush)Application.Current.Resources["Dark_TextLightBrush"];

                            Label _SzMark = new Label();
                            _SzMark.Margin = new Thickness(5, (20 * i) + 3, 5, 0);
                            _SzMark.Content = IsMB ? "MB" : "KB";
                            _SzMark.FontWeight = FontWeights.Bold;
                            _SzMark.HorizontalAlignment = HorizontalAlignment.Right;
                            _SzMark.HorizontalContentAlignment = HorizontalAlignment.Right;
                            _SzMark.Foreground = (System.Windows.Media.Brush)Application.Current.Resources["Dark_SecondaryDarkBrush"];

                            StackPanel SPanel = new StackPanel();
                            SPanel.Orientation = Orientation.Horizontal;
                            SPanel.Children.Add(_Img);
                            SPanel.Children.Add(_Label);

                            SPanel.Opacity = 0;
                            _Sz.Opacity = 0;
                            _SzMark.Opacity = 0;

                            DoubleAnimation OpacPanel = new DoubleAnimation(1, TimeSpan.FromSeconds(2f));
                            OpacPanel.BeginTime = TimeSpan.FromSeconds(0.5f);
                            SPanel.BeginAnimation(StackPanel.OpacityProperty, OpacPanel);

                            DoubleAnimation OpacSz = new DoubleAnimation(1, TimeSpan.FromSeconds(2f));
                            OpacSz.BeginTime = TimeSpan.FromSeconds(0.5f);
                            _Sz.BeginAnimation(Label.OpacityProperty, OpacSz);
                            _SzMark.BeginAnimation(Label.OpacityProperty, OpacSz);

                            CleanResultPanel.Children.Add(SPanel);
                            CleanResultPanel.Children.Add(_Sz);
                            CleanResultPanel.Children.Add(_SzMark);
                        });
                    }
                }
                else
                {
                    MessageBox.Show("No items have been selected to scan. " +
                        "Please select items from the list to scan.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);

                    this.Dispatcher.Invoke(() =>
                    {
                        HideCleanControls();
                        CleanNoScanPanel.Visibility = Visibility.Visible;
                    });
                }
                DigitalClean.DigitalClean.Status = State.IDLE;
            });
            T.SetApartmentState(ApartmentState.STA);
            T.Start();
        }

        private void CleanScanButton_Click(object sender, RoutedEventArgs e)
        {
            CleanFunction(false);   
        }

        private void HideCleanControls()
        {
            SpinnerAngle = -1.0f;
            CleanProgressBar.Value = 0;
            CleanSpinnerImg.Visibility = Visibility.Collapsed;
            CleanCompleteImg.Visibility = Visibility.Collapsed;
            CleanStatusLabel.Visibility = Visibility.Collapsed;
            CleanDescriptionLabel.Visibility = Visibility.Collapsed;
            CleanNoteLabel.Visibility = Visibility.Collapsed;
            CleanDetailsLabel.Visibility = Visibility.Collapsed;
            CleanSep1.Visibility = Visibility.Collapsed;
            CleanSep2.Visibility = Visibility.Collapsed;
        }

        private void ShowCleanControls()
        {
            SpinnerAngle = -1.0f;
            CleanProgressBar.Value = 0;
            DoubleAnimation AnimProg = new DoubleAnimation(0, TimeSpan.FromSeconds(0.5f));
            CleanProgressBar.BeginAnimation(ProgressBar.ValueProperty, AnimProg);
            CleanSpinnerImg.Visibility = Visibility.Visible;
            CleanCompleteImg.Visibility = Visibility.Collapsed;
            CleanStatusLabel.Visibility = Visibility.Visible;
            CleanDescriptionLabel.Visibility = Visibility.Visible;
            CleanNoteLabel.Visibility = Visibility.Visible;

            CleanSep1.Visibility = Visibility.Collapsed;
            CleanSep2.Visibility = Visibility.Collapsed;

            CleanResultPanel.Children.Clear();
            CleanNoScanPanel.Visibility = Visibility.Collapsed;

            CleanNoteLabel.BeginAnimation(Label.ContentProperty, null);
            CleanDescriptionLabel.BeginAnimation(Label.ContentProperty, null);
            CleanStatusLabel.BeginAnimation(Label.ContentProperty, null);
        }

        private List<string> ConstructFolderList()
        {
            List<string> Result = new List<string>();

            //MICROSOFT EDGE

            //INTERNET EXPLORER
            Result.Add(Clean_IAutoBox.IsChecked.Value ? DigitalClean.Folders.INET_AUTO : string.Empty);
            Result.Add(Clean_ICookiesBox.IsChecked.Value ? DigitalClean.Folders.INET_COOKIES : string.Empty);
            Result.Add(Clean_IHistoryBox.IsChecked.Value ? DigitalClean.Folders.INET_HISTORY : string.Empty);
            Result.Add(Clean_IImageBox.IsChecked.Value ? DigitalClean.Folders.INET_IMAGES : string.Empty);
            Result.Add(Clean_IDatabaseBox.IsChecked.Value ? DigitalClean.Folders.INET_DATABASE : string.Empty);
            Result.Add(Clean_IPasswordBox.IsChecked.Value ? DigitalClean.Folders.INET_PASSWORD : string.Empty);
            Result.Add(Clean_ITempBox.IsChecked.Value ? DigitalClean.Folders.INET_TEMP : string.Empty);

            //SYSTEM
            //Result.Add(Clean_SysClipBox.IsChecked.Value ? "CLEAR SYSTEM CLIPBOARD" : string.Empty);
            Result.Add(Clean_SysErrorBox.IsChecked.Value ? DigitalClean.Folders.WIN_REPORTS : string.Empty);
            Result.Add(Clean_SysEventBox.IsChecked.Value ? DigitalClean.Folders.WIN_EVENTLOGS : string.Empty);
            Result.Add(Clean_SysDumpBox.IsChecked.Value ? DigitalClean.Folders.WIN_MEMDUMP : string.Empty);
            Result.Add(Clean_SysRecycleBox.IsChecked.Value ? DigitalClean.Folders.USER_RECYCLE : string.Empty);
            Result.Add(Clean_SysLogBox.IsChecked.Value ? DigitalClean.Folders.WIN_SYSLOGS : string.Empty);
            Result.Add(Clean_SysTempBox.IsChecked.Value ? DigitalClean.Folders.USER_TEMPORARY : string.Empty);
            Result.Add(Clean_SysWebBox.IsChecked.Value ? DigitalClean.Folders.USER_WEB : string.Empty);
            Result.Add(Clean_SysWinLogBox.IsChecked.Value ? DigitalClean.Folders.WIN_LOGS : string.Empty);
            Result.Add(Clean_SysReportBox.IsChecked.Value ? DigitalClean.Folders.WIN_REPORTS : string.Empty);

            //WINDOWS EXPLORER
            Result.Add(Clean_WinDownloadBox.IsChecked.Value ? DigitalClean.Folders.WIN_DOWNLOADS : string.Empty);
            Result.Add(Clean_WinHistoryBox.IsChecked.Value ? DigitalClean.Folders.USER_HISTORY : string.Empty);
            Result.Add(Clean_WinIconBox.IsChecked.Value ? DigitalClean.Folders.USER_ICONS : string.Empty);
            Result.Add(Clean_WinRecentBox.IsChecked.Value ? DigitalClean.Folders.USER_RECENTS : string.Empty);
            Result.Add(Clean_WinSearchBox.IsChecked.Value ? DigitalClean.Folders.USER_SEARCH : string.Empty);
            Result.Add(Clean_WinThumbnailBox.IsChecked.Value ? DigitalClean.Folders.WIN_THUMBNAIL : string.Empty);
            Result.Add(Clean_WinUserThumbBox.IsChecked.Value ? DigitalClean.Folders.USER_THUMBNAIL : string.Empty);

            List<string> FinalResult = new List<string>();
            for (int i = 0; i < Result.Count; i++)
            {
                if (Result[i] != string.Empty)
                {
                    FinalResult.Add(Result[i]);
                }
            }

            return FinalResult;
        }

        private void CleanContext_CheckAllButton_Click(object sender, RoutedEventArgs e)
        {
            //MICROSOFT EDGE
            Clean_EAutoBox.IsChecked = true;
            Clean_ECookiesBox.IsChecked = true;
            Clean_EHistoryBox.IsChecked = true;
            Clean_EImageBox.IsChecked = true;
            Clean_EDatabaseBox.IsChecked = true;
            Clean_ESessionBox.IsChecked = true;
            Clean_EPasswordBox.IsChecked = true;
            Clean_ETempBox.IsChecked = true;

            //INTERNET EXPLORER
            Clean_IAutoBox.IsChecked = true;
            Clean_ICookiesBox.IsChecked = true;
            Clean_IHistoryBox.IsChecked = true;
            Clean_IImageBox.IsChecked = true;
            Clean_IDatabaseBox.IsChecked = true;
            Clean_IPasswordBox.IsChecked = true;
            Clean_ITempBox.IsChecked = true;

            //SYSTEM
            Clean_SysClipBox.IsChecked = true;
            Clean_SysErrorBox.IsChecked = true;
            Clean_SysEventBox.IsChecked = true;
            Clean_SysDumpBox.IsChecked = true;
            Clean_SysRecycleBox.IsChecked = true;
            Clean_SysLogBox.IsChecked= true;
            Clean_SysTempBox.IsChecked = true;
            Clean_SysWebBox.IsChecked = true;
            Clean_SysWinLogBox.IsChecked = true;
            Clean_SysReportBox.IsChecked = true;

            //WINDOWS EXPLORER
            Clean_WinDownloadBox.IsChecked = true;
            Clean_WinHistoryBox.IsChecked = true;
            Clean_WinIconBox.IsChecked = true;
            Clean_WinRecentBox.IsChecked = true;
            Clean_SysNotifBox.IsChecked = true;
            Clean_WinSearchBox.IsChecked = true;
            Clean_WinThumbnailBox.IsChecked = true;
            Clean_WinUserThumbBox.IsChecked = true;
        }

        private void CleanContext_UncheckAllButton_Click(object sender, RoutedEventArgs e)
        {
            // MICROSOFT EDGE
            Clean_EAutoBox.IsChecked = false;
            Clean_ECookiesBox.IsChecked = false;
            Clean_EHistoryBox.IsChecked = false;
            Clean_EImageBox.IsChecked = false;
            Clean_EDatabaseBox.IsChecked = false;
            Clean_ESessionBox.IsChecked = false;
            Clean_EPasswordBox.IsChecked = false;
            Clean_ETempBox.IsChecked = false;

            // INTERNET EXPLORER
            Clean_IAutoBox.IsChecked = false;
            Clean_ICookiesBox.IsChecked = false;
            Clean_IHistoryBox.IsChecked = false;
            Clean_IImageBox.IsChecked = false;
            Clean_IDatabaseBox.IsChecked = false;
            Clean_IPasswordBox.IsChecked = false;
            Clean_ITempBox.IsChecked = false;

            // SYSTEM
            Clean_SysClipBox.IsChecked = false;
            Clean_SysErrorBox.IsChecked = false;
            Clean_SysEventBox.IsChecked = false;
            Clean_SysDumpBox.IsChecked = false;
            Clean_SysRecycleBox.IsChecked = false;
            Clean_SysLogBox.IsChecked = false;
            Clean_SysTempBox.IsChecked = false;
            Clean_SysWebBox.IsChecked = false;
            Clean_SysWinLogBox.IsChecked = false;
            Clean_SysReportBox.IsChecked = false;

            // WINDOWS EXPLORER
            Clean_WinDownloadBox.IsChecked = false;
            Clean_WinHistoryBox.IsChecked = false;
            Clean_WinIconBox.IsChecked = false;
            Clean_WinRecentBox.IsChecked = false;
            Clean_SysNotifBox.IsChecked = false;
            Clean_WinSearchBox.IsChecked = false;
            Clean_WinThumbnailBox.IsChecked = false;
            Clean_WinUserThumbBox.IsChecked = false;
        }

        private void CustomFolderNameBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (CustomFolderNameBox.Text == "Untitled")
            {
                CustomFolderNameBox.Text = "";
            }
        }

        private void CustomFolderDescBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (CustomFolderDescBox.Text == "Enter a description...")
            {
                CustomFolderDescBox.Text = "";
            }
        }

        private void CustomFolderBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            //SHOW FOLDER DIALOG
            FolderDialog.ShowDialog();

            //IF THE USER SUCCESSFULLY SELECTED A FOLDER
            if (FolderDialog.FolderName != null &&
                FolderDialog.FolderName != string.Empty)
            {
                //FILL PATH TEXTBOX WITH FOLDER PATH
                CustomFolderPathBox.Text = FolderDialog.FolderName;

                //IF THE NAME TEXTBOX WAS LEFT AS THE
                //DEFAULT, EMPTY, OR PREVIOUSLY USED
                //AUTO-NAME GENERATION, THEN GENERATE
                //ANOTHER NAME FOR THE NEW PATH
                if (CustomFolderNameBox.Text == "Untitled" ||
                    CustomFolderNameBox.Text == string.Empty ||
                    CustomFolderIntelligentName == true)
                {
                    //SPLIT THE FOLDER PATH STRING BY THE BACKSLASH CHARACTER
                    string[] FolderShortName = FolderDialog.FolderName.Split('\\', 
                        StringSplitOptions.RemoveEmptyEntries);

                    //SET THE PARENT AND CHILD FOLDERS BASED OFF SPLIT STRING
                    string FolderParent = FolderShortName[FolderShortName.Length - 2];
                    string FolderChild = FolderShortName[FolderShortName.Length - 1];

                    //LIST OF PARENT FOLDER NAMES THAT SHOULD
                    //BE EXCLUDED FROM CUSTOM FOLDER NAMES
                    //THESE FOLDERS ARE MORE LIKELY TO BE
                    //SYSTEM FOLDERS AND NOT RELATED TO THE
                    //CUSTOM PATH THE USER IS SELECTING
                    List<string> ExcludedParentFolders = new List<string>()
                    {
                        "Roaming", "Local", "LocalLow", "Program Files", "Program Files (x86)",
                        "Windows", "C:\\", string.Empty
                    };

                    //CAPITALIZE FIRST LETTERS OF BOTH FOLDERS
                    //FOR BETTER APPEARANCE IN FOLDER LIST
                    FolderParent = FolderParent.CapitalizeFirst();
                    FolderChild = FolderChild.CapitalizeFirst();

                    //IF THE FOLDER NAMES ARE DIFFERENT FROM EACHOTHER
                    if (!ExcludedParentFolders.Contains(FolderParent) &&
                        !FolderChild.Contains(FolderParent))
                    {
                        //COMBINE THEM INTO A FULL NAME AND
                        //DISPLAY IT IN THE NAME TEXTBOX
                        CustomFolderNameBox.Text = FolderParent + " " + FolderChild;
                    }
                    //FOLDER PARENT AND CHILD HAD THE SAME NAME
                    //THIS IS UNCOMMON, BUT BOTH NAMES SHOULD
                    //NOT BE DISPLAYED, ONLY DISPLAY CHILD FOLDER
                    else
                    {
                        //DISPLAY ONLY THE CHILD FOLDER INTO THE TEXTBOX
                        CustomFolderNameBox.Text = FolderChild;
                    }

                    //FOLDER NAME HAS BEEN AUTOMATICALLY SET
                    //AND IF ANOTHER PATH IS CHOSEN BEFORE
                    //THE NAME IS MODIFIED, A NEW NAME
                    //WILL BE GENERATED FOR THAT FOLDER SELECTION
                    CustomFolderIntelligentName = true;
                }
                //USER MANUALLY ENTERED TEXT INTO THE
                //NAME TEXTBOX AND SHOULD NOT BE MODIFIED
                else
                {
                    CustomFolderIntelligentName = false;
                }
            }

            //INVALIDATE THE FOLDER BROWSE BUTTON
            //THIS MAY BE IMPORTANT, BUT IS UNKNOWN
            CustomFolderBrowseButton.InvalidateVisual();
        }

        private void CustomFolderNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //NAME HAS BEEN MANUALLY CHANGED
            //DO NOT INTELLIGENTLY CHANGE THE
            //NAME WHEN SELECTING A NEW FOLDER
            //KEEP THIS NAME
            CustomFolderIntelligentName = false;
        }

        private void CustomFolderAddButton_Click(object sender, RoutedEventArgs e)
        {
            //INITIALIZE POSSIBLE ERRORS
            bool FolderNameError = false;
            bool FolderPathError = false;

            //CHECK FOR FOLDER NAME ERRORS
            if (CustomFolderNameBox.Text == string.Empty ||
                CustomFolderNameBox.Text.Replace(' ', ConstNullChar) == string.Empty) 
            {
                FolderNameError = true;
            }

            //CHECK FOR FOLDER PATH ERRORS
            if (CustomFolderPathBox.Text == string.Empty ||
                CustomFolderPathBox.Text.Replace(' ', ConstNullChar) == string.Empty)
            {
                FolderPathError = true;
            }

            //BOTH FOLDER NAME AND PATH NAME ARE GOOD
            if (!FolderNameError && !FolderPathError)
            {
                //ADD CUSTOM FOLDER TO DIGITAL CLEAN DATA
                //IF DESCRIPTION TEXTBOX STILL CONTAINS
                //THE DEFAULT STRING, WRITE NULL
                //DESCRIPTION DATA
                DigitalClean.DigitalClean.CustomFolders.Add(new CustomFolder(CustomFolderNameBox.Text,
                    CustomFolderPathBox.Text, CustomFolderDescBox.Text != "Enter a description..." ? "" 
                    : CustomFolderDescBox.Text));

                //SAVE NEW DATA TO FILE
                DigitalClean.DigitalClean.SaveCustomFolders();

                UpdateCustomFolderListBox();
            }
            //FOLDER NAME IS BAD, PATH NAME IS GOOD
            else if (FolderNameError && !FolderPathError)
            {
                MessageBox.Show("An invalid or null folder name has been entered. Please check the entry and try again.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //FOLDER NAME IS GOOD, PATH NAME IS BAD
            else if (!FolderNameError && FolderPathError)
            {
                MessageBox.Show("An invalid or null folder path has been entered. Please check the entry and try again.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //FOLDER NAME IS BAD, PATH NAME IS BAD
            else if (FolderNameError && FolderPathError)
            {
                MessageBox.Show("An invalid or null folder name and folder path have both been entered. Please check the entries and try again.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateCustomFolderListBox()
        {
            //UPDATE THE LISTBOX WITH THIS INFORMATION
            CustomFolderListBox.Items.Clear();

            for (int i = 0; i < DigitalClean.DigitalClean.CustomFolders.Count; i++)
            {
                //CustomFolderListBox.Items.Add(new Label()
                //{
                //    Content = DigitalClean.DigitalClean.CustomFolders[i].Name,
                //    Foreground = (System.Windows.Media.Brush)Application.Current.Resources["Dark_TextLightBrush"]
                //});

                CustomFolderListBox.Items.Add(new CustomFolderDataContext(DigitalClean.DigitalClean.CustomFolders[i].Name,
                    "Test", "Test"));
            }

            //HIDE THE PANEL THAT SAYS "NO CUSTOM FOLDERS"
            CustomFolderNoFolderPanel.Visibility = Visibility.Collapsed;
            CustomFolderListBox.Visibility = Visibility.Visible;
        }
    }
}