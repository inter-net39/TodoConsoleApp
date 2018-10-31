using System;

namespace MyApp
{
    public class TaskModel
    {

        private DateTime? _startDate;
        private DateTime? _endDate;
        private bool _allDayAllDayActivity;
        private bool? _activityRank;

        public string Description { get; set; }

        public string StartDate
        {
            get
            {
                if (_startDate.HasValue)
                {
                    return _startDate.Value.ToString("dd/MM/yyyy");
                }
                return "";
            }
            set
            {
                if (Validate.MatchDate(value))
                {
                    _startDate = DateTime.Parse(value);
                }
                else
                {
                    _startDate = null;
                }
            }

        }
        public string EndDate
        {
            get
            {
                if (_endDate.HasValue)
                {
                    return _endDate.Value.ToString("dd/MM/yyyy");
                }
                return "";
            }
            set
            {
                if (Validate.MatchDate(value))
                {
                    _endDate = DateTime.Parse(value);
                    _allDayAllDayActivity = false;
                }
                else
                {
                    _endDate = null;
                    _allDayAllDayActivity = true;
                }
            }

        }
        public string AllDayAllDayActivity
        {
            get
            {
                if (_allDayAllDayActivity)
                {
                    return "Tak";
                }

                return "Nie";
            }
            set
            {
                if (Validate.MatchBool(value))
                {
                    _allDayAllDayActivity = bool.Parse(value);
                    _endDate = null;
                }
                else
                {
                    _allDayAllDayActivity = bool.Parse(value);
                }
            }

        }
        public string ActivityRank
        {
            get
            {
                if (_activityRank.HasValue)
                {
                    if (_activityRank.Value)
                    {
                        return "Ważne";
                    }

                    return "Nieważne";
                }

                return "Nieważne";
            }
            set
            {
                if (Validate.MatchBool(value))
                {
                    _activityRank = bool.Parse(value);
                }
                else if (value == "Ważne")
                {
                    _activityRank = true;
                }
                else if (value == "Nieważne")
                {
                    _activityRank = false;
                }
                else
                {
                    _activityRank = null;
                }
            }
        }
        public TaskModel(string description, string startDate, string endDate, string activityRank)
        {
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            ActivityRank = activityRank;
        }

        public string Export()
        {
            return $"{Description},{StartDate},{EndDate},{AllDayAllDayActivity},{MakeActivityBool()}";
        }

        private bool MakeActivityBool()
        {
            if (ActivityRank == "Ważne")
            {
                return true;
            }

            return false;
        }
    }
}