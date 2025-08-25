using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace MedicialUse.Components
{
    /// <summary>
    /// Interaction logic for CalendarComponent.xaml
    /// </summary>
    public enum StartOfWeek
    {
        Monday,
        Sunday
    }

    public sealed partial class Calendar : UserControl
    {
        // ==== Dependency Properties ====
        public static readonly DependencyProperty DisplayDateProperty =
            DependencyProperty.Register(
                nameof(DisplayDate),
                typeof(DateTime),
                typeof(Calendar),
                new PropertyMetadata(DateTime.Today, OnDisplayDateChanged));

        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register(
                nameof(SelectedDate),
                typeof(DateTime?),
                typeof(Calendar),
                new PropertyMetadata(null, OnSelectedDateChanged));

        public static readonly DependencyProperty StartOfWeekProperty =
            DependencyProperty.Register(
                nameof(StartOfWeek),
                typeof(StartOfWeek),
                typeof(Calendar),
                new PropertyMetadata(StartOfWeek.Monday, OnStartOfWeekChanged));

        public DateTime DisplayDate
        {
            get => (DateTime)GetValue(DisplayDateProperty);
            set => SetValue(DisplayDateProperty, value);
        }

        public DateTime? SelectedDate
        {
            get => (DateTime?)GetValue(SelectedDateProperty);
            set => SetValue(SelectedDateProperty, value);
        }

        public StartOfWeek StartOfWeek
        {
            get => (StartOfWeek)GetValue(StartOfWeekProperty);
            set => SetValue(StartOfWeekProperty, value);
        }

        // ==== Events ====
        public event Action<DateTime> DayPressed;

        // ==== Visual settings ====
        private static readonly Color dayBg = Color.FromArgb(255, 240, 244, 248);
        private static readonly Color dayBgOtherMonth = Color.FromArgb(255, 230, 234, 238);
        private static readonly Color dayBgSelected = Color.FromArgb(255, 171, 195, 223);
        private static readonly Brush textBrush = Brushes.Black;

        private readonly List<Border> dayCells = new();

        public Calendar()
        {
            InitializeComponent();
            // Defaults
            if (DisplayDate == default) DisplayDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            RenderWeekdayHeader();
            RenderMonth();
        }

        private static void OnDisplayDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Calendar)d;
            control.RenderMonth();
        }

        private static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Calendar)d;
            control.HighlightSelection();
        }

        private static void OnStartOfWeekChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (Calendar)d;
            control.RenderWeekdayHeader();
            control.RenderMonth();
        }

        // ==== Rendering ====
        private void RenderWeekdayHeader()
        {
            weekdayHeader.Children.Clear();
            var culture = CultureInfo.CurrentCulture;
            var baseDay = StartOfWeek == StartOfWeek.Monday ? DayOfWeek.Monday : DayOfWeek.Sunday;

            for (int i = 0; i < 7; i++)
            {
                var dow = (DayOfWeek)(((int)baseDay + i) % 7);
                var name = culture.DateTimeFormat.AbbreviatedDayNames[(int)dow];
                weekdayHeader.Children.Add(new TextBlock
                {
                    Text = name,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontWeight = FontWeights.SemiBold,
                    Margin = new Thickness(0, 0, 0, 4)
                });
            }
        }

        private void ClearDays()
        {
            dayRow1.Children.Clear();
            dayRow2.Children.Clear();
            dayRow3.Children.Clear();
            dayRow4.Children.Clear();
            dayRow5.Children.Clear();
            dayRow6.Children.Clear();
            dayCells.Clear();
        }

        private void RenderMonth()
        {
            ClearDays();

            Caption.Text = $"{DisplayDate:yyyy.MM}";

            var firstOfMonth = new DateTime(DisplayDate.Year, DisplayDate.Month, 1);
            var daysInMonth = DateTime.DaysInMonth(DisplayDate.Year, DisplayDate.Month);

            int offset = GetDayOffset(firstOfMonth.DayOfWeek);

            int totalCells = offset + daysInMonth;
            int rowsNeeded = (int)Math.Ceiling(totalCells / 7.0);

            var startDate = firstOfMonth.AddDays(-offset);
            for (int i = 0; i < rowsNeeded * 7; i++) // Зөвхөн шаардлагатай хэмжээний cell
            {
                var date = startDate.AddDays(i);
                var isCurrentMonth = date.Month == DisplayDate.Month;

                var cell = CreateDayCell(date, isCurrentMonth);
                AddCellToGrid(cell, i);
            }

            // илүүдэл мөрүүдийг нуух
            dayRow1.Visibility = rowsNeeded >= 1 ? Visibility.Visible : Visibility.Collapsed;
            dayRow2.Visibility = rowsNeeded >= 2 ? Visibility.Visible : Visibility.Collapsed;
            dayRow3.Visibility = rowsNeeded >= 3 ? Visibility.Visible : Visibility.Collapsed;
            dayRow4.Visibility = rowsNeeded >= 4 ? Visibility.Visible : Visibility.Collapsed;
            dayRow5.Visibility = rowsNeeded >= 5 ? Visibility.Visible : Visibility.Collapsed;
            dayRow6.Visibility = rowsNeeded >= 6 ? Visibility.Visible : Visibility.Collapsed;

            HighlightSelection();
        }

        private int GetDayOffset(DayOfWeek firstDay)
        {
            if (StartOfWeek == StartOfWeek.Monday)
            {
                // Map Sunday(0) -> 6, Monday(1) -> 0, ..., Saturday(6) -> 5
                int idx = ((int)firstDay + 6) % 7;
                return idx;
            }
            else // Sunday start
            {
                return (int)firstDay; // Sunday=0, Monday=1, ...
            }
        }

        private Border CreateDayCell(DateTime date, bool isCurrentMonth)
        {
            var bg = isCurrentMonth ? new SolidColorBrush(dayBg) : new SolidColorBrush(dayBgOtherMonth);

            var cell = new Border
            {
                Width = 60,
                Height = 60,
                Margin = new Thickness(2, 3, 2, 3),
                Background = bg,
                CornerRadius = new CornerRadius(6),
                Tag = date,
                Opacity = isCurrentMonth ? 1.0 : 0.55,
                Cursor = isCurrentMonth ? Cursors.Hand : Cursors.Arrow
            };

            var tb = new TextBlock
            {
                Text = date.Day.ToString(),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 18,
                FontWeight = isCurrentMonth ? FontWeights.SemiBold : FontWeights.Normal,
                Foreground = textBrush
            };

            cell.Child = tb;

            // ЗӨВХӨН энэ сарын өдрүүдэд эвент холбоно
            if (isCurrentMonth)
                cell.MouseDown += DayCell_MouseDown;

            dayCells.Add(cell);
            return cell;
        }

        private void AddCellToGrid(Border cell, int index)
        {
            if (index < 7) dayRow1.Children.Add(cell);
            else if (index < 14) dayRow2.Children.Add(cell);
            else if (index < 21) dayRow3.Children.Add(cell);
            else if (index < 28) dayRow4.Children.Add(cell);
            else if (index < 35) dayRow5.Children.Add(cell);
            else dayRow6.Children.Add(cell);
        }

        // ==== Interaction ====
        private void DayCell_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var cell = (Border)sender;
            var date = (DateTime)cell.Tag;
            SelectedDate = date;
            DayPressed?.Invoke(date);
        }

        private void HighlightSelection()
        {
            foreach (var c in dayCells)
            {
                var date = (DateTime)c.Tag;
                bool selected = SelectedDate.HasValue && date.Date == SelectedDate.Value.Date;
                c.Background = new SolidColorBrush(selected ? dayBgSelected :
                    (date.Month == DisplayDate.Month ? dayBg : dayBgOtherMonth));
            }
        }

        private void PrevMonth_Click(object sender, RoutedEventArgs e)
        {
            DisplayDate = new DateTime(DisplayDate.Year, DisplayDate.Month, 1).AddMonths(-1);
        }

        private void NextMonth_Click(object sender, RoutedEventArgs e)
        {
            DisplayDate = new DateTime(DisplayDate.Year, DisplayDate.Month, 1).AddMonths(1);
        }
    }
}
