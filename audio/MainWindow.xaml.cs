using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;

namespace audio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MediaPlayer mediaPlayer;
        private List<string> musicFiles;
        private int currentTrackIndex;
        private bool isPlaying;
        private bool isRepeatEnabled;
        private bool isShuffleEnabled;
        private Random random;

        public MainWindow()
        {
            InitializeComponent();

            mediaPlayer = new MediaPlayer();
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
            mediaPlayer.MediaOpened += MediaPlayer_MediaOpened;

            musicFiles = new List<string>();
            currentTrackIndex = -1;
            isPlaying = false;
            isRepeatEnabled = false;
            isShuffleEnabled = false;
            random = new Random();

            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            EventHandler Timer_Tick = null;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void OpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var folderPath = dialog.FileName;
                musicFiles = Directory.GetFiles((string)folderPath, "*.mp3").ToList();

                if (musicFiles.Count > 0)
                {
                    currentTrackIndex = 0;
                    PlayTrack(currentTrackIndex);
                }
            }
        }

        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            if (isRepeatEnabled)
            {
                PlayTrack(currentTrackIndex);
            }
            else if (isShuffleEnabled)
            {
                currentTrackIndex = random.Next(0, musicFiles.Count);
                PlayTrack(currentTrackIndex);
            }
            else if (currentTrackIndex < musicFiles.Count - 1)
            {
                currentTrackIndex++;
                PlayTrack(currentTrackIndex);
            }
            else
            {
                isPlaying = false;
                mediaPlayer.Stop();
            }
        }

        private void PlayTrack(int currentTrackIndex)
        {
            throw new NotImplementedException();
        }

        private void MediaPlayer_MediaOpened(object sender, EventArgs e)
        {
            // обновление информации о длительности музыки при открытии нового трека
            var duration = mediaPlayer.NaturalDuration.TimeSpan;
            DurationLabel.Content = duration.ToString(@"mm\:ss");
        }

        private void PlayPauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (isPlaying)
            {
                mediaPlayer.Pause();
                isPlaying = false;
                PlayPauseButton.Icon = new PackIcon() { Kind = PackIconKind.Play };
            }
            else
            {
                mediaPlayer.Play();
                isPlaying = true;
                PlayPauseButton.Icon = new PackIcon() { Kind = PackIconKind.Pause };
            }
        }

        private void PrevTrackButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentTrackIndex > 0)
            {
                currentTrackIndex--;
                PlayTrack(currentTrackIndex);
            }
        }

        private void NextTrackButton_Click(object sender, RoutedEventArgs e)
        {


        }

internal class CommonOpenFileDialog
    {
        public CommonOpenFileDialog()
        {
        }

            public bool IsFolderPicker { get; internal set; }
            public object FileName { get; internal set; }

            internal object ShowDialog()
            {
                throw new NotImplementedException();
            }
        }
    }
