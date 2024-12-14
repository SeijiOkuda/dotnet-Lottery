using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private readonly Random random = new Random();

        private List<int> slot1Items;
        private List<int> slot2Items;
        private List<int> slot3Items;

        private DispatcherTimer timer1;
        private DispatcherTimer timer2;
        private DispatcherTimer timer3;

        private bool isSpinning1, isSpinning2, isSpinning3;

        public MainWindow()
        {
            InitializeComponent();
            InitializeSlots();
            InitializeTimers();
        }

        private void InitializeSlots()
        {
            // 数字リストを準備（1〜9のループ）
            slot1Items = GenerateSlotNumbers();
            slot2Items = GenerateSlotNumbers();
            slot3Items = GenerateSlotNumbers();

            // 初期表示
            UpdateSlotDisplay(Slot1Top, Slot1Middle, Slot1Bottom, slot1Items);
            UpdateSlotDisplay(Slot2Top, Slot2Middle, Slot2Bottom, slot2Items);
            UpdateSlotDisplay(Slot3Top, Slot3Middle, Slot3Bottom, slot3Items);
        }

        private List<int> GenerateSlotNumbers()
        {
            return new List<int> { 9, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1 }; // 環状リスト
        }

        private void InitializeTimers()
        {
            // 回転速度を速くするために間隔を50msに設定
            timer1 = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
            timer2 = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
            timer3 = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };

            timer1.Tick += (s, e) => Rotate(slot1Items, Slot1Top, Slot1Middle, Slot1Bottom);
            timer2.Tick += (s, e) => Rotate(slot2Items, Slot2Top, Slot2Middle, Slot2Bottom);
            timer3.Tick += (s, e) => Rotate(slot3Items, Slot3Top, Slot3Middle, Slot3Bottom);
        }

        private void Rotate(List<int> slotItems, TextBlock top, TextBlock middle, TextBlock bottom)
        {
            // 数字リストを回転
            int first = slotItems[0];
            slotItems.RemoveAt(0);
            slotItems.Add(first);

            // 表示を更新
            UpdateSlotDisplay(top, middle, bottom, slotItems);
        }

        private void UpdateSlotDisplay(TextBlock top, TextBlock middle, TextBlock bottom, List<int> slotItems)
        {
            top.Text = slotItems[3].ToString();     // 上の数字
            middle.Text = slotItems[4].ToString();  // 中央の数字
            bottom.Text = slotItems[5].ToString();  // 下の数字
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            // スロット回転開始
            isSpinning1 = isSpinning2 = isSpinning3 = true;

            timer1.Start();
            timer2.Start();
            timer3.Start();

            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;
        }

        private async void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (isSpinning1)
            {
                isSpinning1 = false;
                timer1.Stop();
                await Task.Delay(500); // 停止に遅延を加える
            }
            else if (isSpinning2)
            {
                isSpinning2 = false;
                timer2.Stop();
                await Task.Delay(500);
            }
            else if (isSpinning3)
            {
                isSpinning3 = false;
                timer3.Stop();
            }

            // 全停止後の処理
            if (!isSpinning1 && !isSpinning2 && !isSpinning3)
            {
                StartButton.IsEnabled = true;
                StopButton.IsEnabled = false;
                ShowResults();
            }
        }

        private void ShowResults()
        {
            // 中央の結果を取得
            int result1 = int.Parse(Slot1Middle.Text);
            int result2 = int.Parse(Slot2Middle.Text);
            int result3 = int.Parse(Slot3Middle.Text);

            MessageBox.Show($"Results: {result1}, {result2}, {result3}");
        }
    }
}
