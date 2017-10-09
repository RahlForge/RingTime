using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Media;
using Java.Util;
using Newtonsoft.Json;
using Java.Lang;

namespace RingTime
{
    [Activity(Label = "Set up RingTime")]
    public class RingTimeSetupActivity : Activity                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            
    {
        const int TIME_DIALOG_ID = 0;

        RadioButton everyDayButton;
        RadioButton certainDaysButton;
        RadioButton specificDateButton;
        RadioButton silenceButton;
        RadioButton vibrateButton;
        RadioButton ringerButton;

        LinearLayout specificDateLayout;
        LinearLayout certainDaysLayout;

        List<DayOfWeek> dow = new List<DayOfWeek>();

        AudioManager mgr;
        SeekBar ringerVolume;
        SeekBar notificationVolume;

        ToggleButton mon;
        ToggleButton tues;
        ToggleButton wed;
        ToggleButton thurs;
        ToggleButton fri;
        ToggleButton sat;
        ToggleButton sun;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.RingTimeSetup);

            var selectDateButton = FindViewById<Button>(Resource.Id.selectDateButton);
            selectDateButton.Click += SelectDateButton_Click;

            var setTimeButton = FindViewById<Button>(Resource.Id.ringTimeButton);
            setTimeButton.Click += (o, e) => ShowDialog(TIME_DIALOG_ID);

            var deleteButton = FindViewById<Button>(Resource.Id.deleteButton);
            deleteButton.Visibility = ViewStates.Gone;

            var createButton = FindViewById<Button>(Resource.Id.createButton);
            createButton.Click += CreateButton_Click;

            everyDayButton = FindViewById<RadioButton>(Resource.Id.everyDayButton);
            everyDayButton.Click += FrequencyButton_Click;

            certainDaysButton = FindViewById<RadioButton>(Resource.Id.certainDaysButton);
            certainDaysButton.Click += FrequencyButton_Click;

            specificDateButton = FindViewById<RadioButton>(Resource.Id.specificDateButton);
            specificDateButton.Click += FrequencyButton_Click;

            certainDaysLayout = FindViewById<LinearLayout>(Resource.Id.certainDaysLayout);
            specificDateLayout = FindViewById<LinearLayout>(Resource.Id.specificDateLayout);

            mon = FindViewById<ToggleButton>(Resource.Id.mondayButton);
            tues = FindViewById<ToggleButton>(Resource.Id.tuesdayButton);
            wed = FindViewById<ToggleButton>(Resource.Id.wednesdayButton);
            thurs = FindViewById<ToggleButton>(Resource.Id.thursdayButton);
            fri = FindViewById<ToggleButton>(Resource.Id.fridayButton);
            sat = FindViewById<ToggleButton>(Resource.Id.saturdayButton);
            sun = FindViewById<ToggleButton>(Resource.Id.sundayButton);

            CheckFrequency();

            mgr = (AudioManager)GetSystemService(AudioService);
            ringerVolume = FindViewById<SeekBar>(Resource.Id.ringerVolumeSeekBar);
            notificationVolume = FindViewById<SeekBar>(Resource.Id.notificationVolumeSeekBar);

            silenceButton = FindViewById<RadioButton>(Resource.Id.silenceButton);
            silenceButton.Click += VolumeOptionButton_Click;

            vibrateButton = FindViewById<RadioButton>(Resource.Id.vibrateButton);
            vibrateButton.Click += VolumeOptionButton_Click;

            ringerButton = FindViewById<RadioButton>(Resource.Id.ringerButton);
            ringerButton.Click += VolumeOptionButton_Click;
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            // Prepare the AlarmManager and new Intent for creating the alarm service
            AlarmManager manager = (AlarmManager)GetSystemService(AlarmService);
            Intent ringTimeIntent = new Intent(this, typeof(RingTimeReceiver));

            // Generate the new ringtime object
            RingTimeObject rto;
            var timeText = FindViewById<TextView>(Resource.Id.ringTimeText);
            if (certainDaysButton.Checked)
            {
                BuildDaysOfWeekList();
                rto = new RingTimeObject(DateTime.Parse(timeText.Text), ringerVolume.Progress, notificationVolume.Progress, dow); 
            }
            else if (specificDateButton.Checked)
            {
                var dateText = FindViewById<TextView>(Resource.Id.specificDateText);
                rto = new RingTimeObject(DateTime.Parse(timeText.Text), ringerVolume.Progress, notificationVolume.Progress, DateTime.Parse(dateText.Text));
            }
            else
                rto = new RingTimeObject(DateTime.Parse(timeText.Text), ringerVolume.Progress, notificationVolume.Progress);

            ringTimeIntent.PutExtra("ringtime", rto.ToBundle());

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(this, 0, ringTimeIntent, PendingIntentFlags.UpdateCurrent);
            manager.Set(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + 5 * 1000, pendingIntent);

            /*
            // Default to EveryDay for now
            int interval = 5 * 1000; // 1000 * 60 * 60 * 24;
            var ringTimeText = FindViewById<TextView>(Resource.Id.ringTimeText);
            DateTime ringTime = DateTime.Parse(ringTimeText.Text);

            // Create the calendar object to schedule the alarm
            Calendar calendar = Calendar.GetInstance(Java.Util.TimeZone.Default);
            calendar.TimeInMillis = JavaSystem.CurrentTimeMillis();
            calendar.Set(CalendarField.HourOfDay, ringTime.Hour);
            calendar.Set(CalendarField.Minute, ringTime.Minute);

            // Set the repeater
            //manager.Set(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + 5 * 1000, pendingIntent);
            manager.SetRepeating(AlarmType.RtcWakeup, calendar.TimeInMillis, interval, pendingIntent);
            */
        }

        private void VolumeOptionButton_Click(object sender, EventArgs e)
        {
            CheckVolumeOption();
        }

        private void BuildDaysOfWeekList()
        {
            dow.Clear();
            if (mon.Checked)
                dow.Add(DayOfWeek.Monday);
            if (tues.Checked)
                dow.Add(DayOfWeek.Tuesday);
            if (wed.Checked)
                dow.Add(DayOfWeek.Wednesday);
            if (thurs.Checked)
                dow.Add(DayOfWeek.Thursday);
            if (fri.Checked)
                dow.Add(DayOfWeek.Friday);
            if (sat.Checked)
                dow.Add(DayOfWeek.Saturday);
            if (sun.Checked)
                dow.Add(DayOfWeek.Sunday);
        }

        private void InitBar(SeekBar bar, Android.Media.Stream stream)
        {
            bar.Max = mgr.GetStreamMaxVolume(stream);
            bar.Progress = mgr.GetStreamVolume(stream);
            bar.SetOnSeekBarChangeListener(new VolumeListener(mgr, stream));
            bar.ProgressChanged += VolumeSeekBar_ProgressChanged;
        }

        private void VolumeSeekBar_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            if (e.Progress > 0)
                ringerButton.Checked = true;
            CheckVolumeOption();
        }

        private void FrequencyButton_Click(object sender, EventArgs e)
        {
            CheckFrequency();
        }

        private void CheckFrequency()
        {
            certainDaysLayout.Visibility = ViewStates.Gone;
            specificDateLayout.Visibility = ViewStates.Gone;
            if (certainDaysButton.Checked)
                certainDaysLayout.Visibility = ViewStates.Visible;
            else if (specificDateButton.Checked)
                specificDateLayout.Visibility = ViewStates.Visible;
        }

        private void CheckVolumeOption()
        {
            if (!ringerButton.Checked)
            {                
                ringerVolume.Progress = 0;
                notificationVolume.Progress = 0;

                if (silenceButton.Checked)
                    mgr.RingerMode = RingerMode.Silent;
                else
                    mgr.RingerMode = RingerMode.Vibrate;
            }
            else
                mgr.RingerMode = RingerMode.Normal;
        }

        private void SelectDateButton_Click(object sender, EventArgs e)
        {
            var dateText = FindViewById<TextView>(Resource.Id.specificDateText);
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                dateText.Text = time.ToShortDateString();
                mon.Checked = false;
                tues.Checked = false;
                wed.Checked = false;
                thurs.Checked = false;
                fri.Checked = false;
                sat.Checked = false;
                sun.Checked = false;
                switch(time.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        mon.Checked = true;
                        break;
                    case DayOfWeek.Tuesday:
                        tues.Checked = true;
                        break;
                    case DayOfWeek.Wednesday:
                        wed.Checked = true;
                        break;
                    case DayOfWeek.Thursday:
                        thurs.Checked = true;
                        break;
                    case DayOfWeek.Friday:
                        fri.Checked = true;
                        break;
                    case DayOfWeek.Saturday:
                        sat.Checked = true;
                        break;
                    case DayOfWeek.Sunday:
                        sun.Checked = true;
                        break;
                }
            });

            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void TimePickerCallback(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            string timeString = string.Format("{0}:{1}", e.HourOfDay, e.Minute.ToString().PadLeft(2, '0'));
            DateTime time = DateTime.Parse(timeString);            
            var timeText = FindViewById<TextView>(Resource.Id.ringTimeText);
            timeText.Text = time.ToShortTimeString();
        }

        protected override Dialog OnCreateDialog(int id)
        {
            if (id == TIME_DIALOG_ID)
                return new TimePickerDialog(this, TimePickerCallback, DateTime.Now.Hour, DateTime.Now.Minute, false);

            return null;
        }
    }

    public class VolumeListener : Java.Lang.Object, SeekBar.IOnSeekBarChangeListener
    {
        AudioManager audio;
        Stream theStream;
        public VolumeListener(AudioManager mgr, Android.Media.Stream stream)
        {
            // /!\ is it a correct way to pass information in the constructor ?
            theStream = stream;
            audio = mgr;
        }
        
        public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            if (fromUser)
            {
                string.Format("The user adjusted the value of the SeekBar to {0}", seekBar.Progress);

                // play song
                audio.SetStreamVolume(theStream, progress, VolumeNotificationFlags.PlaySound);

                //display the UI
                //audio.SetStreamVolume(theStream, progress, VolumeNotificationFlags.ShowUi);
            }
        }

        public void OnStartTrackingTouch(SeekBar seekBar)
        {
            System.Diagnostics.Debug.WriteLine("Tracking changes.");
        }

        public void OnStopTrackingTouch(SeekBar seekBar)
        {
            System.Diagnostics.Debug.WriteLine("Stopped tracking changes.");
        }
    }
}