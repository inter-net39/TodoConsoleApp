using System;

namespace MyApp
{
    public class TaskModel
    {
        public string Description { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public bool? AllDayAllDayActivity { get; set; } = false;
        public bool? ActivityRank { get; set; } = false;

        public TaskModel(string description, string startDate, string endDate, bool? activityRank)
        {
            Description = description;
            StartDate = DateTime.Parse(startDate);
            if (!string.IsNullOrEmpty(endDate))
            {
                EndDate = DateTime.Parse(endDate);
            }
            else
            {
                AllDayAllDayActivity = true;
            }
            ActivityRank = activityRank;
        }

        public override string ToString()
        {
            //TODO: Dodać hasValue


            return $"{Description.PadLeft(15)}, {StartDate.ToString("dd/MM/yyyy").PadLeft(10)}, {HasValue(EndDate).PadLeft(10)}, {HasValue(AllDayAllDayActivity).PadLeft(7)}, {HasValue(ActivityRank).PadLeft(7)}";
        }

        private string HasValue(object thing)
        {
            if (thing as DateTime? != null)
            {
                if ((thing as DateTime?).HasValue)
                {
                    return (thing as DateTime?).Value.ToString("dd/MM/yyy");
                }

                return " - ";
            }
            if (thing as bool? != null)
            {
                if ((thing as bool?).HasValue)
                {
                    return (thing as bool?).Value.ToString();
                }

                return " - ";
            }
            return "";
        }
    }
}