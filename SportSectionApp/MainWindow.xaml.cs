using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SportSectionApp
{
    public partial class MainWindow : Window
    {
        private List<Member> members = new List<Member>();
        private List<AttendanceRecord> attendanceRecords = new List<AttendanceRecord>();
        private List<Coach> coaches = new List<Coach>();
        private List<ScheduleItem> scheduleItems = new List<ScheduleItem>();

        private const string MembersFile = "members.txt";
        private const string AttendanceFile = "attendance.txt";
        private const string CoachesFile = "coaches.txt";
        private const string ScheduleFile = "schedule.txt";

        public MainWindow()
        {
            InitializeComponent();
            dpAttendanceDate.SelectedDate = DateTime.Today;

            // Установка начальных значений
            cmbDayOfWeek.SelectedIndex = (int)DateTime.Today.DayOfWeek - 1;
            if (cmbDayOfWeek.SelectedIndex < 0) cmbDayOfWeek.SelectedIndex = 0;

            LoadData();
            UpdateUI();
        }

        // Класс участника
        public class Member
        {
            public string FullName { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public Guid Id { get; set; } = Guid.NewGuid();
        }

        // Класс записи посещаемости
        public class AttendanceRecord
        {
            public Guid MemberId { get; set; }
            public string MemberName { get; set; }
            public DateTime Date { get; set; }
            public string Status { get; set; } = "Присутствовал";
        }

        // НОВЫЙ КЛАСС: Тренер
        public class Coach
        {
            public string FullName { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Specialization { get; set; }
            public int Experience { get; set; }
            public Guid Id { get; set; } = Guid.NewGuid();
        }

        // НОВЫЙ КЛАСС: Элемент расписания
        public class ScheduleItem
        {
            public string DayOfWeek { get; set; }
            public string Time { get; set; }
            public Guid CoachId { get; set; }
            public string CoachName { get; set; }
            public string GroupName { get; set; }
            public string Duration { get; set; } = "1.5 часа";
            public Guid Id { get; set; } = Guid.NewGuid();
        }

        // Загрузка всех данных
        private void LoadData()
        {
            LoadMembers();
            LoadAttendance();
            LoadCoaches();
            LoadSchedule();
        }

        private void LoadMembers()
        {
            members.Clear();
            if (File.Exists(MembersFile))
            {
                try
                {
                    var lines = File.ReadAllLines(MembersFile);
                    foreach (var line in lines)
                    {
                        var parts = line.Split('|');
                        if (parts.Length >= 4)
                        {
                            members.Add(new Member
                            {
                                Id = Guid.Parse(parts[0]),
                                FullName = parts[1],
                                Phone = parts[2],
                                Email = parts[3]
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки участников: {ex.Message}");
                }
            }
        }

        private void LoadAttendance()
        {
            attendanceRecords.Clear();
            if (File.Exists(AttendanceFile))
            {
                try
                {
                    var lines = File.ReadAllLines(AttendanceFile);
                    foreach (var line in lines)
                    {
                        var parts = line.Split('|');
                        if (parts.Length >= 4)
                        {
                            attendanceRecords.Add(new AttendanceRecord
                            {
                                MemberId = Guid.Parse(parts[0]),
                                MemberName = parts[1],
                                Date = DateTime.Parse(parts[2]),
                                Status = parts[3]
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки посещаемости: {ex.Message}");
                }
            }
        }

        // НОВЫЙ МЕТОД: Загрузка тренеров
        private void LoadCoaches()
        {
            coaches.Clear();
            if (File.Exists(CoachesFile))
            {
                try
                {
                    var lines = File.ReadAllLines(CoachesFile);
                    foreach (var line in lines)
                    {
                        var parts = line.Split('|');
                        if (parts.Length >= 6)
                        {
                            coaches.Add(new Coach
                            {
                                Id = Guid.Parse(parts[0]),
                                FullName = parts[1],
                                Phone = parts[2],
                                Email = parts[3],
                                Specialization = parts[4],
                                Experience = int.Parse(parts[5])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки тренеров: {ex.Message}");
                }
            }
        }

        // НОВЫЙ МЕТОД: Загрузка расписания
        private void LoadSchedule()
        {
            scheduleItems.Clear();
            if (File.Exists(ScheduleFile))
            {
                try
                {
                    var lines = File.ReadAllLines(ScheduleFile);
                    foreach (var line in lines)
                    {
                        var parts = line.Split('|');
                        if (parts.Length >= 7)
                        {
                            scheduleItems.Add(new ScheduleItem
                            {
                                Id = Guid.Parse(parts[0]),
                                DayOfWeek = parts[1],
                                Time = parts[2],
                                CoachId = Guid.Parse(parts[3]),
                                CoachName = parts[4],
                                GroupName = parts[5],
                                Duration = parts[6]
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки расписания: {ex.Message}");
                }
            }
        }

        // Сохранение всех данных
        private void SaveMembers()
        {
            try
            {
                var lines = members.Select(m => $"{m.Id}|{m.FullName}|{m.Phone}|{m.Email}");
                File.WriteAllLines(MembersFile, lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения участников: {ex.Message}");
            }
        }

        private void SaveAttendance()
        {
            try
            {
                var lines = attendanceRecords.Select(a => $"{a.MemberId}|{a.MemberName}|{a.Date}|{a.Status}");
                File.WriteAllLines(AttendanceFile, lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения посещаемости: {ex.Message}");
            }
        }

        // НОВЫЙ МЕТОД: Сохранение тренеров
        private void SaveCoaches()
        {
            try
            {
                var lines = coaches.Select(c => $"{c.Id}|{c.FullName}|{c.Phone}|{c.Email}|{c.Specialization}|{c.Experience}");
                File.WriteAllLines(CoachesFile, lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения тренеров: {ex.Message}");
            }
        }

        // НОВЫЙ МЕТОД: Сохранение расписания
        private void SaveSchedule()
        {
            try
            {
                var lines = scheduleItems.Select(s => $"{s.Id}|{s.DayOfWeek}|{s.Time}|{s.CoachId}|{s.CoachName}|{s.GroupName}|{s.Duration}");
                File.WriteAllLines(ScheduleFile, lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения расписания: {ex.Message}");
            }
        }

        // Обновление интерфейса
        private void UpdateUI()
        {
            // Участники
            dgMembers.ItemsSource = null;
            dgMembers.ItemsSource = members;

            cmbMemberForAttendance.ItemsSource = null;
            cmbMemberForAttendance.ItemsSource = members;

            // Посещаемость
            dgAttendance.ItemsSource = null;
            dgAttendance.ItemsSource = attendanceRecords.OrderByDescending(a => a.Date).ToList();

            // Тренеры
            dgCoaches.ItemsSource = null;
            dgCoaches.ItemsSource = coaches;

            cmbCoachForSchedule.ItemsSource = null;
            cmbCoachForSchedule.ItemsSource = coaches;

            cmbFilterCoach.ItemsSource = null;
            cmbFilterCoach.ItemsSource = coaches;

            // Расписание
            UpdateScheduleView();

            // Статистика
            UpdateStatistics();
        }

        private void UpdateStatistics()
        {
            if (members.Count > 0)
            {
                var today = DateTime.Today;
                var todayCount = attendanceRecords.Count(a => a.Date.Date == today);
                var totalVisits = attendanceRecords.Count;

                txtAttendanceStats.Text = $"Всего участников: {members.Count} | " +
                                         $"Тренеров: {coaches.Count} | " +
                                         $"Занятий в неделю: {scheduleItems.Count} | " +
                                         $"Посещений сегодня: {todayCount}";
            }
            else
            {
                txtAttendanceStats.Text = "Нет данных";
            }
        }

        // НОВЫЙ МЕТОД: Обновление вида расписания
        private void UpdateScheduleView(List<ScheduleItem> filteredItems = null)
        {
            dgSchedule.ItemsSource = null;
            dgSchedule.ItemsSource = (filteredItems ?? scheduleItems)
                .OrderBy(s => GetDayOrder(s.DayOfWeek))
                .ThenBy(s => s.Time)
                .ToList();
        }

        private int GetDayOrder(string day)
        {
            var days = new Dictionary<string, int>
            {
                {"Понедельник", 1},
                {"Вторник", 2},
                {"Среда", 3},
                {"Четверг", 4},
                {"Пятница", 5},
                {"Суббота", 6},
                {"Воскресенье", 7}
            };

            return days.ContainsKey(day) ? days[day] : 8;
        }
        private void btnAddMember_Click(object sender, RoutedEventArgs e)
        {
            // ... существующий код ...
        }

        private void btnRemoveMember_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, выбран ли участник в DataGrid
            if (dgMembers.SelectedItem is Member selectedMember)
            {
                // Подтверждение удаления
                var result = MessageBox.Show($"Вы действительно хотите удалить участника {selectedMember.FullName}?\n\n" +
                                             "Также будут удалены все записи о его посещаемости.",
                                             "Подтверждение удаления",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Удаляем участника из списка
                        members.Remove(selectedMember);

                        // Удаляем все записи посещаемости для этого участника
                        attendanceRecords.RemoveAll(a => a.MemberId == selectedMember.Id);

                        // Сохраняем изменения
                        SaveMembers();
                        SaveAttendance();

                        // Обновляем интерфейс
                        UpdateUI();

                        MessageBox.Show($"Участник {selectedMember.FullName} успешно удален",
                                       "Успех",
                                       MessageBoxButton.OK,
                                       MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении участника: {ex.Message}",
                                       "Ошибка",
                                       MessageBoxButton.OK,
                                       MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите участника для удаления",
                               "Информация",
                               MessageBoxButton.OK,
                               MessageBoxImage.Information);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveMembers();
            SaveAttendance();
            SaveCoaches();
            SaveSchedule();
            MessageBox.Show("Все данные сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            UpdateUI();
            MessageBox.Show("Данные загружены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnMarkAttendance_Click(object sender, RoutedEventArgs e)
        {
            // ... существующий код ...
        }

        private void btnAddNewMember_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем заполненность обязательных полей
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Введите ФИО участника", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Проверяем корректность email (необязательно, но желательно)
            if (!string.IsNullOrWhiteSpace(txtEmail.Text) &&
                !txtEmail.Text.Contains("@"))
            {
                MessageBox.Show("Введите корректный email адрес", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Создаем нового участника
            Member newMember = new Member
            {
                FullName = txtFullName.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Email = txtEmail.Text.Trim()
            };

            // Добавляем в список
            members.Add(newMember);

            // Сохраняем в файл
            SaveMembers();

            // Обновляем интерфейс
            UpdateUI();

            // Очищаем поля для нового ввода
            txtFullName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();

            // Переключаемся на вкладку участников для подтверждения
            var tabControl = this.FindName("TabControl") as TabControl;
            if (tabControl != null)
            {
                tabControl.SelectedIndex = 0; // Первая вкладка - Участники
            }

            MessageBox.Show($"Участник {newMember.FullName} успешно добавлен", "Успех",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnAddCoach_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new Window
            {
                Title = "Добавить тренера",
                Width = 450,
                Height = 300,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            var grid = new Grid { Margin = new Thickness(10) };
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var txtName = new TextBox { Margin = new Thickness(5) };
            var txtPhone = new TextBox { Margin = new Thickness(5) };
            var txtEmail = new TextBox { Margin = new Thickness(5) };
            var txtSpecialization = new TextBox { Margin = new Thickness(5) };
            var txtExperience = new TextBox { Margin = new Thickness(5) };

            // ФИО
            TextBlock nameLabel = new TextBlock { Text = "ФИО:", VerticalAlignment = VerticalAlignment.Center };
            Grid.SetRow(nameLabel, 0);
            Grid.SetColumn(nameLabel, 0);
            grid.Children.Add(nameLabel);

            Grid.SetRow(txtName, 0);
            Grid.SetColumn(txtName, 1);
            grid.Children.Add(txtName);

            // Телефон
            TextBlock phoneLabel = new TextBlock { Text = "Телефон:", VerticalAlignment = VerticalAlignment.Center };
            Grid.SetRow(phoneLabel, 1);
            Grid.SetColumn(phoneLabel, 0);
            grid.Children.Add(phoneLabel);

            Grid.SetRow(txtPhone, 1);
            Grid.SetColumn(txtPhone, 1);
            grid.Children.Add(txtPhone);

            // Email
            TextBlock emailLabel = new TextBlock { Text = "Email:", VerticalAlignment = VerticalAlignment.Center };
            Grid.SetRow(emailLabel, 2);
            Grid.SetColumn(emailLabel, 0);
            grid.Children.Add(emailLabel);

            Grid.SetRow(txtEmail, 2);
            Grid.SetColumn(txtEmail, 1);
            grid.Children.Add(txtEmail);

            // Специализация
            TextBlock specLabel = new TextBlock { Text = "Специализация:", VerticalAlignment = VerticalAlignment.Center };
            Grid.SetRow(specLabel, 3);
            Grid.SetColumn(specLabel, 0);
            grid.Children.Add(specLabel);

            Grid.SetRow(txtSpecialization, 3);
            Grid.SetColumn(txtSpecialization, 1);
            grid.Children.Add(txtSpecialization);

            // Стаж
            TextBlock expLabel = new TextBlock { Text = "Стаж (лет):", VerticalAlignment = VerticalAlignment.Center };
            Grid.SetRow(expLabel, 4);
            Grid.SetColumn(expLabel, 0);
            grid.Children.Add(expLabel);

            Grid.SetRow(txtExperience, 4);
            Grid.SetColumn(txtExperience, 1);
            grid.Children.Add(txtExperience);

            var btnOk = new Button
            {
                Content = "Добавить",
                Width = 100,
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Center
            };
            btnOk.Click += (s, args) =>
            {
                if (!string.IsNullOrWhiteSpace(txtName.Text))
                {
                    int experience = 0;
                    int.TryParse(txtExperience.Text, out experience);

                    coaches.Add(new Coach
                    {
                        FullName = txtName.Text,
                        Phone = txtPhone.Text,
                        Email = txtEmail.Text,
                        Specialization = txtSpecialization.Text,
                        Experience = experience
                    });
                    SaveCoaches();
                    UpdateUI();
                    addWindow.Close();
                }
                else
                {
                    MessageBox.Show("Введите ФИО тренера");
                }
            };

            Grid.SetRow(btnOk, 5);
            Grid.SetColumn(btnOk, 0);
            Grid.SetColumnSpan(btnOk, 2);
            grid.Children.Add(btnOk);

            addWindow.Content = grid;
            addWindow.ShowDialog();
        }

        private void btnRemoveCoach_Click(object sender, RoutedEventArgs e)
        {
            if (dgCoaches.SelectedItem is Coach selectedCoach)
            {
                var result = MessageBox.Show($"Удалить тренера {selectedCoach.FullName}?",
                    "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    // Удаляем тренера
                    coaches.Remove(selectedCoach);

                    // Удаляем связанные записи расписания
                    scheduleItems.RemoveAll(s => s.CoachId == selectedCoach.Id);

                    SaveCoaches();
                    SaveSchedule();
                    UpdateUI();
                }
            }
            else
            {
                MessageBox.Show("Выберите тренера для удаления");
            }
        }

        private void btnEditCoach_Click(object sender, RoutedEventArgs e)
        {
            if (dgCoaches.SelectedItem is Coach selectedCoach)
            {
                MessageBox.Show("Редактирование тренера будет реализовано в следующей версии",
                    "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Выберите тренера для редактирования");
            }
        }

        // ============ НОВЫЕ ОБРАБОТЧИКИ ДЛЯ РАСПИСАНИЯ ============

        private void btnAddSchedule_Click(object sender, RoutedEventArgs e)
        {
            if (cmbCoachForSchedule.SelectedItem is Coach selectedCoach &&
                !string.IsNullOrWhiteSpace(cmbDayOfWeek.Text) &&
                !string.IsNullOrWhiteSpace(txtTime.Text))
            {
                // Проверяем, не занято ли время у тренера
                var existingSchedule = scheduleItems.FirstOrDefault(s =>
                    s.CoachId == selectedCoach.Id &&
                    s.DayOfWeek == cmbDayOfWeek.Text &&
                    s.Time == txtTime.Text);

                if (existingSchedule != null)
                {
                    MessageBox.Show("Тренер уже занят в это время!", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                scheduleItems.Add(new ScheduleItem
                {
                    DayOfWeek = cmbDayOfWeek.Text,
                    Time = txtTime.Text,
                    CoachId = selectedCoach.Id,
                    CoachName = selectedCoach.FullName,
                    GroupName = string.IsNullOrWhiteSpace(txtGroup.Text) ? "Общая группа" : txtGroup.Text
                });

                SaveSchedule();
                UpdateUI();

                // Очищаем поля
                txtGroup.Clear();
                txtTime.Text = "18:00";

                MessageBox.Show("Занятие добавлено в расписание", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Заполните все обязательные поля: день, время и тренер");
            }
        }

        private void btnShowAllSchedule_Click(object sender, RoutedEventArgs e)
        {
            UpdateScheduleView();
        }

        private void btnShowTodaySchedule_Click(object sender, RoutedEventArgs e)
        {
            string today = DateTime.Today.DayOfWeek switch
            {
                DayOfWeek.Monday => "Понедельник",
                DayOfWeek.Tuesday => "Вторник",
                DayOfWeek.Wednesday => "Среда",
                DayOfWeek.Thursday => "Четверг",
                DayOfWeek.Friday => "Пятница",
                DayOfWeek.Saturday => "Суббота",
                DayOfWeek.Sunday => "Воскресенье",
                _ => "Понедельник"
            };

            var todaySchedule = scheduleItems.Where(s => s.DayOfWeek == today).ToList();
            UpdateScheduleView(todaySchedule);

            MessageBox.Show($"Расписание на {today}", "Информация",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnShowByCoach_Click(object sender, RoutedEventArgs e)
        {
            if (cmbFilterCoach.SelectedItem is Coach selectedCoach)
            {
                var coachSchedule = scheduleItems.Where(s => s.CoachId == selectedCoach.Id).ToList();
                UpdateScheduleView(coachSchedule);

                MessageBox.Show($"Расписание тренера {selectedCoach.FullName}",
                    "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Выберите тренера для фильтрации");
            }
        }
    }
}